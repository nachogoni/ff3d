using System;
using System.Collections;
using System.Collections.Generic;

public class FF3d_NoHeuristic : FF3d_Heuristics
{

    public float getHeuristic(FF3d_State state)
    {
        return 1.0f;
    }

    public String toString()
    {
        return "NoHeuristic";
    }
}