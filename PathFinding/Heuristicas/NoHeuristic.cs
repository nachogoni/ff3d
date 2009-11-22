using System;
using System.Collections;
using System.Collections.Generic;

public class NoHeuristic : Heuristics
{

    public float getHeuristic(State state)
    {
        return 1.0f;
    }

    public String toString()
    {
        return "NoHeuristic";
    }
}