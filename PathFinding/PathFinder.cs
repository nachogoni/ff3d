using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : Problem
{
    CellTypes[,] actualMap = null;
    int rowValue = 0;
    int colValue = 0;
    List<PosRowCol> goalsPos;
    List<PosRowCol> trapsPos;
    Heuristics heuristic;

    public PathFinder(CellTypes[,] cells, int row, int col, List<PosRowCol> goals, List<PosRowCol> traps, Heuristics heuristic)
    {
        if (heuristic != null)
            this.heuristic = heuristic;
        else
            this.heuristic = new MinDistance();

        setInitState(cells, row, col, goals, traps);
    }

    public void setInitState(CellTypes[,] cells, int row, int col, List<PosRowCol> goals, List<PosRowCol> traps)
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
        actualMap = (CellTypes[,])cells.Clone();
        
    }

    public State getInitState()
    {
        State state = new TankState(actualMap, rowValue, colValue, goalsPos);

        return state;
    }

    public bool isGoal(State state)
    {
        return ((TankState)state).isGoal();
    }

    public ArrayList getRules()
    {
        ArrayList rules = new ArrayList();

        rules.Add(new TankRule(MoveTypes.DOWN));
        rules.Add(new TankRule(MoveTypes.UP));
        rules.Add(new TankRule(MoveTypes.LEFT));
        rules.Add(new TankRule(MoveTypes.RIGHT));
        rules.Add(new TankRule(MoveTypes.DOWNLEFT));
        rules.Add(new TankRule(MoveTypes.DOWNRIGHT));
        rules.Add(new TankRule(MoveTypes.UPLEFT));
        rules.Add(new TankRule(MoveTypes.UPRIGHT));

        return rules;
    }

    public float getHeuristic(State state)
    {
        // TODO: armar una heuristica
        return heuristic.getHeuristic(state);
    }


}