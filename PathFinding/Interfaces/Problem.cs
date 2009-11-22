using System;
using System.Collections;
using System.Collections.Generic;

public interface Problem
{
    State getInitState();
    bool isGoal(State state);
    ArrayList getRules();
    float getHeuristic(State state);
}