using FF3D;

// Mono Framework
using System;
using System.Collections;
using System.Collections.Generic;

// Unity Framework
using UnityEngine;


public class TankFF3D_IrVolver : TankBehaviour
{
    // Representacion del mapa
    int cols, rows;
    CellTypes[,] cells = null;

    // Posicion del tanque
    int tankRow = 0, tankCol = 0;

    // Torreta
    float actualAngle = 0;

    // Flags & Enemies
    List<PosRowCol> flags = new List<PosRowCol>();
    List<PosRowCol> enemies = new List<PosRowCol>();
    Hashtable enemiesHash = new Hashtable();

    // Camino a seguir
    public List<PosRowCol> path = new List<PosRowCol>();
    Rule lastRule = null;
    Vector3 goingTo;
    
    public override TankProperties GetProperties()
    {
        TankProperties tp;

        tp.VisionDistance = 1;
        tp.VisionAngle = 1;
        tp.FirePower = 1;
        tp.FireRate = 1;
        tp.MovSpeed = 4;
        tp.Armor = 1;
        tp.RadDistance = 1;
        tp.RadRefresh = 1;
        tp.EnergyTotal = 3;
        tp.ShieldDuration = 1;
        tp.ShieldRechargeSpd = 1;

        return tp;

    }

    public override void StartThink()
    {
        int x, y;

        // Obtiene el mapa
        cols = map.Cols;
        rows = map.Rows;
        cells = new CellTypes[rows, cols];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                cells[r, c] = (map.IsObstacle(r, c) == true ? CellTypes.WALL : CellTypes.FIELD);
            }
        }

        // TODO: Obtiene los Enemigos -> Vacio a menos que tenga alguno a la vista

        // Obtiene los Flags
        int flagCount = FlagManager.GetFlagCount();
        for (int flag = 0; flag < flagCount; flag++)
        {
            FlagManager.GetFlagRowCol(flag, out x, out y);
            flags.Add(new PosRowCol(x, y));
        }

        // Actualizo el mapa
        UpdateFlagsIntoCells(flags);
        UpdateEnemiesIntoCells(enemies);

        // Actualizo la posicion del tanque
        UpdateTankPosition();

        for (int i = 0; i < 50; i++)
        {
            path.Add(new PosRowCol(19, 6));
            path.Add(new PosRowCol(19, 20));
            path.Add(new PosRowCol(6, 20));
            path.Add(new PosRowCol(6, 6));
        }

        StartMoving();

    }


    public override void Think()
    {
//        Debug.Log(Time.realtimeSinceStartup +  " estoy en " + transform.position.x);
    }

    public void OnFireFinish()
    {
    }

    public void OnMoveFinish()
    {

    }

    void UpdateTankPosition()
    {
        map.GetRowColAtWorldPos(transform.position, out tankRow, out tankCol);
    }

    void UpdateFlagsIntoCells(List<PosRowCol> flags)
    {
        foreach (PosRowCol flag in flags)
        {
            cells[flag.rowValue, flag.colValue] = CellTypes.FLAG;
        }

    }

    void UpdateEnemiesIntoCells(List<PosRowCol> enemies)
    {
        foreach (PosRowCol enemy in enemies)
        {
            cells[enemy.rowValue, enemy.colValue] = CellTypes.ENEMY;
        }

    }

    public void PrintMap()
    {
        string s = "";
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                s = s + " " + cells[r, c];
            }
            s = s + "\n";
        }
        Debug.Log(s);
    }

    public void PrintFlags()
    {
        string s = "";
        foreach (PosRowCol flag in flags)
        {
            s = s + "Flag (" + flag.rowValue + ";" + flag.colValue + ")\n";
        }
    }

    // EnemyFinding

    int a = 0;

    void getEnemies()
    {

        if (visionInfo.Length > 0 && ((a++ % 20) == 0))
        {
            for (int i = 0; i < visionInfo.Length; i++)
            {
                EnemyType enemy;

                if (!(enemiesHash.ContainsKey(visionInfo[i].name)))
                {
                    enemiesHash.Add(visionInfo[i].name, new EnemyType(visionInfo[i].position));
                }

                enemy = (EnemyType)(enemiesHash[visionInfo[i].name]);

               // Debug.Log(enemy.pos.x + " " + enemy.pos.z);

                Vector3 newpos = enemy.newPos(visionInfo[i].position, transform.position);

//                Fire(visionInfo[i].position, null);

                Fire(newpos, null);

              //  Debug.Log("Fire to: " + newpos.x + " " + newpos.z + " estando en " + visionInfo[i].position.x + " " + visionInfo[i].position.z);

            }

        }

        return;
    }


    // BoxFinding
    // TODO


    // PathFinding

    void CalculatePath(int row, int col, List<PosRowCol> flags, List<PosRowCol> enemies)
    {
        // Probando el PathFinder!
        Problem problem = new PathFinder(cells, row, col, flags, enemies, new MinDistance());
        Engine engine = new Engine(problem);
        ArrayList solution;

        if (lastRule != null)
            engine.setLastRule(lastRule);

        if (engine.run(new FF3D.AStar()))
        {
            solution = engine.getRules();

            if (solution.Count > 0)
            {
                lastRule = (Rule)solution[0];
                foreach (Rule rule in solution)
                {
                    if (((TankRule)rule).moveValue != ((TankRule)lastRule).moveValue)
                        path.Add(new PosRowCol(row, col));
                    lastRule = rule;
                    row += TankRule.getRowIncrement(((TankRule)rule).moveValue);
                    col += TankRule.getColIncrement(((TankRule)rule).moveValue);
                }
                path.Add(new PosRowCol(row, col));
            }
        }
        return;
    }

    void StartMoving()
    {
        if (path != null && path.Count > 0)
        {
            MoveTo(path[0].rowValue, path[0].colValue, new MoveFinish(Driver));
            goingTo = map.GetWorldPosAtRowCol(path[0].rowValue, path[0].colValue);
            path.Remove(path[0]);
        }

        return;
    }

    void Driver()
    {
        int row = 0, col = 0;

        if (path == null)
            return;

        if (path.Count > 0)
        {
            MoveTo(path[0].rowValue, path[0].colValue, new MoveFinish(Driver));
            goingTo = map.GetWorldPosAtRowCol(path[0].rowValue, path[0].colValue); 
            path.Remove(path[0]);
        }
        else
        {
            PosRowCol remove = null;
            // No hay mas movimientos... hay que ir a la proxima bandera
            UpdateTankPosition();

            // Obtengo la posicion actual y comparon con las banderas
            foreach (PosRowCol pos in flags)
            {
                if (pos.colValue == tankCol && pos.rowValue == tankRow)
                    remove = pos;
            }

            cells[tankRow, tankCol] = CellTypes.FIELD;

            if (remove != null)
                flags.Remove(remove);

            while (flags.Count == 0)
            {
                row = UnityEngine.Random.Range(1, rows - 1);
                col = UnityEngine.Random.Range(1, cols - 1);

                if (cells[row, col] != CellTypes.WALL)
                {
                    flags.Add(new PosRowCol(row, col));
                    cells[row, col] = CellTypes.FLAG;
                    PrintMap();
                }
            }

            CalculatePath(tankRow, tankCol, flags, enemies);
            StartMoving();

        }
        return;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (path.Count > 0 && goingTo != null)
        {
            Gizmos.DrawLine(transform.position, goingTo);
            Gizmos.DrawLine(goingTo, map.GetWorldPosAtRowCol(path[0].rowValue, path[0].colValue));
            for (int i = 1; i < (path.Count - 1); i++)
                Gizmos.DrawLine(map.GetWorldPosAtRowCol(path[i - 1].rowValue, path[i - 1].colValue), map.GetWorldPosAtRowCol(path[i].rowValue, path[i].colValue));
        }
    }
}
