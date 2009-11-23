using FF3D;

// Mono Framework
using System;
using System.Collections;
using System.Collections.Generic;

// Unity Framework
using UnityEngine;


public class TankFF3D_PathFinder : TankBehaviour
{
    // Representacion del mapa
    int cols, rows;
    FF3d_CellTypes[,] cells = null;

    // Posicion del tanque
    int tankRow = 0, tankCol = 0;

    // Flags & Enemies
    List<FF3d_PosRowCol> flags = new List<FF3d_PosRowCol>();
    List<FF3d_PosRowCol> enemies = new List<FF3d_PosRowCol>();

    // Camino a seguir
    public List<FF3d_PosRowCol> path = new List<FF3d_PosRowCol>();
    FF3d_Rule lastRule = null;

    public override TankProperties GetProperties()
    {
        TankProperties tp;

        tp.VisionDistance = 1;
        tp.VisionAngle = 1;
        tp.FirePower = 1;
        tp.FireRate = 1;
        tp.MovSpeed = 4;
        tp.Armor = 1;
        tp.RadDistance = 4;
        tp.RadRefresh = 1;
        tp.EnergyTotal = 4;
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
        cells = new FF3d_CellTypes[rows, cols];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                cells[r, c] = (map.IsObstacle(r, c) == true ? FF3d_CellTypes.WALL : FF3d_CellTypes.FIELD);
            }
        }

        // TODO: Obtiene los Enemigos -> Vacio a menos que tenga alguno a la vista

        // Obtiene los Flags
        int flagCount = FlagManager.GetFlagCount();
        for (int flag = 0; flag < flagCount; flag++)
        {
            FlagManager.GetFlagRowCol(flag, out x, out y);
            flags.Add(new FF3d_PosRowCol(x, y));
        }

        // Actualizo el mapa
        UpdateFlagsIntoCells(flags);
        UpdateEnemiesIntoCells(enemies);

        // Actualizo la posicion del tanque
        UpdateTankPosition();

        // Prepara el camino hacia la bandera mas cercana
        CalculatePath(tankRow, tankCol, flags, enemies);

        // Se mueve
        StartMoving();

    }

    public override void Think()
    {
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
        foreach (FF3d_PosRowCol flag in flags)
        {
            s = s + "Flag (" + flag.rowValue + ";" + flag.colValue + ")\n";
        }
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
            MoveTo(path[0].rowValue, path[0].colValue, new MoveFinish(Driver));
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
            path.Remove(path[0]);
        }
        else
        {
            FF3d_PosRowCol remove = null;
            // No hay mas movimientos... hay que ir a la proxima bandera
            UpdateTankPosition();

            // Obtengo la posicion actual y comparon con las banderas
            foreach (FF3d_PosRowCol pos in flags)
            {
                if (pos.colValue == tankCol && pos.rowValue == tankRow)
                    remove = pos;
            }

            cells[tankRow, tankCol] = FF3d_CellTypes.FIELD;

            if (remove != null)
                flags.Remove(remove);

            while (flags.Count == 0)
            {
                row = UnityEngine.Random.Range(1, rows - 1);
                col = UnityEngine.Random.Range(1, cols - 1);

                if (cells[row, col] != FF3d_CellTypes.WALL)
                {
                    flags.Add(new FF3d_PosRowCol(row, col));
                    cells[row, col] = FF3d_CellTypes.FLAG;
                    //PrintMap();
                }
            }

            CalculatePath(tankRow, tankCol, flags, enemies);
            StartMoving();

        }
        return;
    }

}
