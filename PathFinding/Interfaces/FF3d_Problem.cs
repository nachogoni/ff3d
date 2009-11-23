using System;
using System.Collections;
using System.Collections.Generic;

public interface FF3d_Problem
{
    FF3d_State getInitState();
    bool isGoal(FF3d_State state);
    ArrayList getRules();
    float getHeuristic(FF3d_State state);
}