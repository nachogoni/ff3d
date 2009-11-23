using FF3D;

// Mono Framework
using System;
using System.Collections;
using System.Collections.Generic;

// Unity Framework
using UnityEngine;

// Estados del tanque
public enum FF3d_TankMode
{
    SearchingBox = 0,
    BeingAttacked,
    Attacking,
    Nothing
}

public enum FF3d_TankTorretStates
{
    Stay = 0,
    Doing,
}

public enum FF3d_TorretMode
{
    Nothing = 0,
    Rotating,
    Shooting
}



public class TankFF3D : TankBehaviour
{
    // Representacion del mapa
    int cols, rows;
    FF3d_CellTypes[,] cells = null;

    // Posicion del tanque
    int tankRow = 0, tankCol = 0;
    int LastTankRow = 0, LastTankCol = 0;

    // Flags & Enemies
    List<FF3d_PosRowCol> flags = new List<FF3d_PosRowCol>();
    List<FF3d_PosRowCol> enemies = new List<FF3d_PosRowCol>();
    Hashtable enemiesHash = new Hashtable();

    // Estado del tanque
    FF3d_TankTorretStates ActualState = FF3d_TankTorretStates.Stay;
    FF3d_TankMode PreviousMode = FF3d_TankMode.Nothing;
    public FF3d_TankMode ActualMode = FF3d_TankMode.Nothing;

    // Estado de la torreta
    FF3d_TankTorretStates actualTorretState = FF3d_TankTorretStates.Stay;
    FF3d_TorretMode actualTorretMode = FF3d_TorretMode.Nothing;
    float actualTorretAngle = 0f;
    float deltaTorretAngle = 10f;
    Vector3 torretShootAt = Vector3.zero;
    bool torretEnemyAtSight = false;
    int rotateTime = 0;
    float deltaRotateTime = 1f;
    float deltaTime = 0f;

    // Camino a seguir
    public List<FF3d_PosRowCol> path = new List<FF3d_PosRowCol>();
    FF3d_Rule lastRule = null;
    Vector3 goingTo;
    float deltaMoveTime = 3f;
    float deltaMTime = 0f;

    // Posicion del device
    Vector3 devicePos = Vector3.zero;
    ArrayList devicePoints = new ArrayList();
    bool doNotRemovePath = false;
    int radarRefresh = 0;
    
    public override TankProperties GetProperties()
    {
        TankProperties tp;

        tp.VisionDistance = 1;
        tp.VisionAngle = 1;
        tp.FirePower = 2;
        tp.FireRate = 4;
        tp.MovSpeed = 4;
        tp.Armor = 2;
        tp.RadDistance = 1;
        tp.RadRefresh = 1;
        tp.EnergyTotal = 2;
        tp.ShieldDuration = 1;
        tp.ShieldRechargeSpd = 1;

        return tp;

    }

    public override void StartThink()
    {
        // Obtiene el mapa
        cols = map.Cols;
        rows = map.Rows;
        cells = new FF3d_CellTypes[rows, cols];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                cells[r, c] = (map.IsObstacle(r, c) == true ? FF3d_CellTypes.WALL : FF3d_CellTypes.FIELD);
            }
        }

        // Actualizo el mapa
        UpdateEnemiesIntoCells(enemies);

        // Actualizo la posicion del tanque
        UpdateTankPosition();

        // Pongo el modo de nada
        ActualState = FF3d_TankTorretStates.Stay;
        ActualMode = FF3d_TankMode.Nothing;
        actualTorretMode = FF3d_TorretMode.Rotating;
    }

    /*
     * Cambia el modo del tanque
     */
    private void changeMode(FF3d_TankMode s)
    {
        // Guardo el modo anterior por si quiero seguir con ese dsp
        if (ActualMode != s)
        {
            PreviousMode = ActualMode;
            ActualMode = s;
        }
        //actualTorretState = TankTorretStates.Stay;
    }

    /*
     * Modo de busqueda de device
     */
    void SearchingBoxMode()
    {
        int row, col;
        // Calculo el recorrido
        map.GetRowColAtWorldPos(devicePos, out row, out col);

        flags.Clear();
        flags.Add(new FF3d_PosRowCol(row, col));

        UpdateTankPosition();
        CalculatePath(tankRow, tankCol, flags, enemies);
        // Go for it
        StartMoving();
    }

    /* 
     * Modo defensivo
     */
    void BeeingAttackedMode()
    {
        //Activo el shield si esta disponible
        if (shieldInfo.timeForBeAvailable == 0)
            ActivateShield();
    }

    /*
     * Modo ofensivo
     */
    void AttackingMode()
    {
        actualTorretMode = FF3d_TorretMode.Shooting;
        
        //Si tengo algo en la vision le disparo

        if (torretEnemyAtSight)
        {
            Fire(torretShootAt, AttackingModeFinish);
        }
        else
            changeMode(PreviousMode);

        //Si no tengo busco en las pos de los enemigos y disparo
    }

    void AttackingModeFinish()
    {
        if (visionInfo.Length == 0)
        {
            torretEnemyAtSight = false;
        }
        torretFinish();
    }

    /*
     * Modo Stand By
     */
    void NothingMode()
    {
        Driver();
    }

    public override void OnShootShieldReceived(Vector3 dir)
    {
        int row, col;

        //Agrego al enemigo al mapa
        map.GetRowColAtWorldPos(dir, out row, out col);
        enemies.Add(new FF3d_PosRowCol(row, col));

        changeMode(FF3d_TankMode.BeingAttacked);
    }

    public override void OnShootReceived(Vector3 dir)
    {
        int row, col;

        //Agrego al enemigo al mapa
        map.GetRowColAtWorldPos(dir, out row, out col);
        enemies.Add(new FF3d_PosRowCol(row, col));

        if (shieldInfo.timeForBeAvailable == 0)
            ActivateShield();

        //changeMode(TankMode.BeingAttacked);
    }

    public override void Think()
    {
        // Segun el estado que haga tal o cual cosa
        if (ActualState == FF3d_TankTorretStates.Stay)
        {
            /*
            if (torretEnemyAtSight)
                changeMode(TankMode.Attacking);
             */

            switch (ActualMode)
            {
                case FF3d_TankMode.Attacking:
                    AttackingMode();
                    break;
                case FF3d_TankMode.BeingAttacked:
                    BeeingAttackedMode();
                    break;
                case FF3d_TankMode.SearchingBox:
                    Debug.Log("Buscando item en " + devicePos);
                    // Calculo el recorrido
                    SearchingBoxMode();
                    break;
                case FF3d_TankMode.Nothing:
                    NothingMode();
                    break;
                default:
                    Debug.Log("Modo invalido de tank");
                    break;
            }
            ActualState = FF3d_TankTorretStates.Doing;
        }

        /*if (actualTorretState == TankTorretStates.Stay)
        {
            switch(actualTorretMode)
            {
                case TorretMode.Rotating:
                    if (TimeForNextShoot > 0)
                        torretRotateMode();
                    break;
                case TorretMode.Shooting:
                    //actualTorretMode = TorretMode.Rotating;
                    break;
                case TorretMode.Nothing:
                    actualTorretMode = TorretMode.Rotating;
                    break;
                default:
                    Debug.Log("Modo invalido de torreta");
                    break;
            }

            actualTorretState = TankTorretStates.Doing;
        }*/

        // Si no tengo la posicion del device busco
        // el vector3.zero es una posicion invalida del tablero
        if (devicePos == Vector3.zero)
        {
            //Calculo la posicion
            if ((devicePos = calculateDevicePosition()) != Vector3.zero)
            {
                // Si la encuentro paso a buscar caja
                changeMode(FF3d_TankMode.SearchingBox);
                doNotRemovePath = true;

            }

        }

        deltaTime += Time.deltaTime;

        if (visionInfo.Length > 0)
        {
            getEnemies();
            Fire(torretShootAt, null);
        }
        else if (deltaTime >= deltaRotateTime)
        {
            torretRotateMode();
            deltaTime = 0;
        }

        UpdateTankPosition();
        if (((deltaMTime+=Time.deltaTime) >= deltaMoveTime) && (LastTankCol == tankCol) && (LastTankRow == tankRow))
        {
            deltaMTime = 0;
//            doNotRemovePath = false;
//            devicePos = Vector3.zero;
            Driver();
        }

        LastTankCol = tankCol;
        LastTankRow = tankRow;
        
    }

    void torretFinish()
    {
        actualTorretState = FF3d_TankTorretStates.Stay;
    }

    void torretShootingMode()
    {
        //Fire(torretShootAt, torretFinish);
    }

    void torretRotateMode()
    {
         actualTorretAngle = (actualTorretAngle + deltaTorretAngle) % 360;
         RotateTorret(actualTorretAngle, new RotateFinish(torretFinish));
        // Agrego los enemigos que haya
         getEnemies();
    }

    Vector3 calculateDevicePosition()
    {
        FF3d_DistancePoint p;
        Array points;

        if (radarRefresh == radarInfo.refreshNumber)
            return Vector3.zero;

        //Actualizo el refresh number
        radarRefresh = radarInfo.refreshNumber;

        p = new FF3d_DistancePoint(transform.position, radarInfo.distanceToObject);

        // Si no tiene el punto que lo ponga
        if (!devicePoints.Contains(p))
        {
            devicePoints.Add(p);
        }

        //Si tengo menos de 3 puntos no puedo triangular
        if (devicePoints.Count > 2)
        {
            FF3d_DistancePoint[] aux = new FF3d_DistancePoint[3];
            int i = 0;
            float x, z;

            foreach(FF3d_DistancePoint d in devicePoints)
            {
                aux[i++] = d;
            }

            float ja, jb, da, db, dc;
            Vector3 a, b, c;

            a = aux[0].getPoint();
            da = aux[0].getDistance();
            
            b = aux[1].getPoint();
            db = aux[1].getDistance();

            c = aux[2].getPoint();
            dc = aux[2].getDistance();

            jb = db * db - dc * dc - b.x * b.x + c.x * c.x - b.z * b.z + c.z * c.z;
            ja = da * da - db * db - a.x * a.x + b.x * b.x - a.z * a.z + b.z * b.z;

            x = (jb - (ja * (c.z - b.z) / (b.z - a.z)))/2;
            x = x / ((c.x - b.x) - (b.x - a.x) * (c.z - b.z) / (b.z - a.z));

            z = (ja - 2 * x * (b.x - a.x)) / (2 * (b.z - a.z));

            return new Vector3(x,transform.position.y,z);
        }

        return Vector3.zero;
    }

    void UpdateTankPosition()
    {

        map.GetRowColAtWorldPos(transform.position, out tankRow, out tankCol);
    }

    void UpdateFlagsIntoCells(List<FF3d_PosRowCol> flags)
    {
        foreach (FF3d_PosRowCol flag in flags)
        {
            cells[flag.rowValue, flag.colValue] = FF3d_CellTypes.FLAG;
        }

    }

    void UpdateEnemiesIntoCells(List<FF3d_PosRowCol> enemies)
    {
        foreach (FF3d_PosRowCol enemy in enemies)
        {
            cells[enemy.rowValue, enemy.colValue] = FF3d_CellTypes.ENEMY;
        }

    }

    // EnemyFinding

    void getEnemies()
    {
        torretEnemyAtSight = false;

        if (visionInfo.Length > 0 )
        {
            torretEnemyAtSight = true;
            torretShootAt = visionInfo[0].position;

            for (int i = 0; i < visionInfo.Length; i++)
            {
                FF3d_EnemyType enemy;

                if (!(enemiesHash.ContainsKey(visionInfo[i].name)))
                {
                    enemiesHash.Add(visionInfo[i].name, new FF3d_EnemyType(visionInfo[i].position));
                }

                enemy = (FF3d_EnemyType)(enemiesHash[visionInfo[i].name]);

                torretShootAt = enemy.newPos(visionInfo[0].position, transform.position);
            }
        }

        return;
    }

        
    // PathFinding

    void CalculatePath(int row, int col, List<FF3d_PosRowCol> flags, List<FF3d_PosRowCol> enemies)
    {
        // Probando el PathFinder!
        FF3d_Problem problem = new FF3d_PathFinder(cells, row, col, flags, enemies, new FF3d_MinDistance());
        FF3d_Engine engine = new FF3d_Engine(problem);
        ArrayList solution;

        if (lastRule != null)
            engine.setLastRule(lastRule);

        if (engine.run(new FF3D.FF3d_AStar()))
        {
            solution = engine.getRules();

            if (solution.Count > 0)
            {
                lastRule = (FF3d_Rule)solution[0];
                foreach (FF3d_Rule rule in solution)
                {
                    if (((FF3d_TankRule)rule).moveValue != ((FF3d_TankRule)lastRule).moveValue)
                        path.Add(new FF3d_PosRowCol(row, col));
                    lastRule = rule;
                    row += FF3d_TankRule.getRowIncrement(((FF3d_TankRule)rule).moveValue);
                    col += FF3d_TankRule.getColIncrement(((FF3d_TankRule)rule).moveValue);
                }
                path.Add(new FF3d_PosRowCol(row, col));
            }
        }
        return;
    }

    void StartMoving()
    {
        if (path != null && path.Count > 0)
        {
            MoveTo(path[0].rowValue, path[0].colValue, new MoveFinish(tankFinish));
            goingTo = map.GetWorldPosAtRowCol(path[0].rowValue, path[0].colValue);
            path.Remove(path[0]);
        }

        return;
    }

    void tankFinish()
    {
        ActualState = FF3d_TankTorretStates.Stay;
    }

    void Driver()
    {
        int row = 0, col = 0;

        if ((path == null || path.Count == 0) && !doNotRemovePath)
        {
            
            UpdateTankPosition();

            if (path == null)
                path = new List<FF3d_PosRowCol>();

            path.Clear();
            flags.Clear();

            while (path.Count == 0)
            {
                while (flags.Count == 0)
                {
                    row = UnityEngine.Random.Range(1, rows - 1);
                    col = UnityEngine.Random.Range(1, cols - 1);

                    if (cells[row, col] != FF3d_CellTypes.WALL)
                    {
                        flags.Add(new FF3d_PosRowCol(row, col));
                        cells[row, col] = FF3d_CellTypes.FLAG;
                    }
                }

                CalculatePath(tankRow, tankCol, flags, enemies);
            }

            StartMoving();

        }
        else
        {
            MoveTo(path[0].rowValue, path[0].colValue, new MoveFinish(tankFinish));
            goingTo = map.GetWorldPosAtRowCol(path[0].rowValue, path[0].colValue);
            path.Remove(path[0]);
        }

        return;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (goingTo != null)
        {
            Gizmos.DrawLine(transform.position, goingTo);
            if (path.Count > 0)
            {
                Gizmos.DrawLine(goingTo, map.GetWorldPosAtRowCol(path[0].rowValue, path[0].colValue));
                for (int i = 1; i < (path.Count - 1); i++)
                    Gizmos.DrawLine(map.GetWorldPosAtRowCol(path[i - 1].rowValue, path[i - 1].colValue), map.GetWorldPosAtRowCol(path[i].rowValue, path[i].colValue));
            }
        }
    }

}