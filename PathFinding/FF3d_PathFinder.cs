using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FF3d_PathFinder : FF3d_Problem
{
    FF3d_CellTypes[,] actualMap = null;
    int rowValue = 0;
    int colValue = 0;
    List<FF3d_PosRowCol> goalsPos;
    List<FF3d_PosRowCol> trapsPos;
    FF3d_Heuristics heuristic;

    public FF3d_PathFinder(FF3d_CellTypes[,] cells, int row, int col, List<FF3d_PosRowCol> goals, List<FF3d_PosRowCol> traps, FF3d_Heuristics heuristic)
    {
        if (heuristic != null)
            this.heuristic = heuristic;
        else
            this.heuristic = new FF3d_MinDistance();

        setInitState(cells, row, col, goals, traps);
    }

    public void setInitState(FF3d_CellTypes[,] cells, int row, int col, List<FF3d_PosRowCol> goals, List<FF3d_PosRowCol> traps)
    {
        if (cells == null || goals == null || traps == null || row > cells.GetLength(0) ||
            row < 0 || col > cells.GetLength(1) || col < 0)
        {
            actualMap = null;
            rowValue = -1;
            colValue = -1;
        }

        rowValue = row;
        colValue = col;
        goalsPos = goals;
        trapsPos = traps;
        actualMap = (FF3d_CellTypes[,])cells.Clone();
        
    }

    public FF3d_State getInitState()
    {
        FF3d_State state = new FF3d_TankState(actualMap, rowValue, colValue, goalsPos);

        return state;
    }

    public bool isGoal(FF3d_State state)
    {
        return ((FF3d_TankState)state).isGoal();
    }

    public ArrayList getRules()
    {
        ArrayList rules = new ArrayList();

        rules.Add(new FF3d_TankRule(FF3d_MoveTypes.DOWN));
        rules.Add(new FF3d_TankRule(FF3d_MoveTypes.UP));
        rules.Add(new FF3d_TankRule(FF3d_MoveTypes.LEFT));
        rules.Add(new FF3d_TankRule(FF3d_MoveTypes.RIGHT));
        rules.Add(new FF3d_TankRule(FF3d_MoveTypes.DOWNLEFT));
        rules.Add(new FF3d_TankRule(FF3d_MoveTypes.DOWNRIGHT));
        rules.Add(new FF3d_TankRule(FF3d_MoveTypes.UPLEFT));
        rules.Add(new FF3d_TankRule(FF3d_MoveTypes.UPRIGHT));

        return rules;
    }

    public float getHeuristic(FF3d_State state)
    {
        // TODO: armar una heuristica
        return heuristic.getHeuristic(state);
    }


}