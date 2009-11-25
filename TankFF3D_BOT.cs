using UnityEngine;

// Mono Framework
using System;
using System.Collections;
using System.Collections.Generic;

public enum FF3d_TankMode
{
    Stop = 0,
    Processing,
    Moving
}

public class TankFF3D_BOT : TankBehaviour
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

    // Camino a seguir
    public List<FF3d_PosRowCol> path = new List<FF3d_PosRowCol>();
    FF3d_Rule lastRule = null;
    Vector3 goingTo;
    float deltaMoveTime = 5f;
    float deltaMTime = 0f;

    // Posicion del device
    Vector3 devicePos = Vector3.zero;
    ArrayList devicePoints = new ArrayList();
    int radarRefresh = 0;

    // Estado del tanque
    FF3d_TankMode tankstatus;

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

    // PathFinding
    private void CalculatePath(int row, int col, List<FF3d_PosRowCol> flags, List<FF3d_PosRowCol> enemies)
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

    private void UpdateTankPosition()
    {
        map.GetRowColAtWorldPos(transform.position, out tankRow, out tankCol);
    }

    private void UpdateFlagsIntoCells(List<FF3d_PosRowCol> flags)
    {
        foreach (FF3d_PosRowCol flag in flags)
        {
            cells[flag.rowValue, flag.colValue] = FF3d_CellTypes.FLAG;
        }
    }

    private void UpdateEnemiesIntoCells(List<FF3d_PosRowCol> enemies)
    {
        foreach (FF3d_PosRowCol enemy in enemies)
        {
            cells[enemy.rowValue, enemy.colValue] = FF3d_CellTypes.ENEMY;
        }
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

        // Estado del tanque
        tankstatus = FF3d_TankMode.Stop;
    }

    public override void Think()
    {
        if (tankstatus != FF3d_TankMode.Processing)
        {
            switch (tankstatus)
            {
                case FF3d_TankMode.Moving:
                    tankstatus = FF3d_TankMode.Processing;
                    moveTankToPosition();
                    break;
                case FF3d_TankMode.Stop:
                    //Si no tengo la pos del device me muevo a algun lugar random
                    generatePathTo(obtainRandowPoint());
                    //Si la tengo voy al device
                    tankstatus = FF3d_TankMode.Moving;
                    break;
                default:
                    tankstatus = FF3d_TankMode.Stop;
                    break;
            }
        }
    }

    void moveTankToPosition()
    {
        if (path != null && path.Count > 0)
        {
            MoveTo(path[0].rowValue, path[0].colValue, new MoveFinish(callbackMoveFinish));
            goingTo = map.GetWorldPosAtRowCol(path[0].rowValue, path[0].colValue);
            path.Remove(path[0]);
        }
        else
        {
            tankstatus = FF3d_TankMode.Stop;
        }

        return;
    }

    private FF3d_PosRowCol obtainRandowPoint()
    {
        int row=0, col=0;
        bool found = false;

        while (!found)
        {
            row = UnityEngine.Random.Range(1, rows - 1);
            col = UnityEngine.Random.Range(1, cols - 1);

            found = (cells[row, col] != FF3d_CellTypes.WALL);
        }

        return new FF3d_PosRowCol(row, col);
    }

    void generatePathTo(int row, int col)
    {
        generatePathTo(new FF3d_PosRowCol(row, col));
    }

    void generatePathTo(FF3d_PosRowCol pos)
    {
        if (path == null || path.Count == 0)
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
                    flags.Add(pos);
                    cells[pos.rowValue, pos.colValue] = FF3d_CellTypes.FLAG;
                }

                CalculatePath(tankRow, tankCol, flags, enemies);
            }
        }
    }

    void callbackMoveFinish()
    {
        tankstatus = FF3d_TankMode.Moving;
    }

    private Vector3 calculateDevicePosition()
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

            foreach (FF3d_DistancePoint d in devicePoints)
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

            x = (jb - (ja * (c.z - b.z) / (b.z - a.z))) / 2;
            x = x / ((c.x - b.x) - (b.x - a.x) * (c.z - b.z) / (b.z - a.z));

            z = (ja - 2 * x * (b.x - a.x)) / (2 * (b.z - a.z));

            return new Vector3(x, transform.position.y, z);
        }

        return Vector3.zero;
    }


}
