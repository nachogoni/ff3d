using System;
using System.Collections;
using System.Collections.Generic;

public interface FF3d_Heuristics
{
    float getHeuristic(FF3d_State state);
    String toString();
}