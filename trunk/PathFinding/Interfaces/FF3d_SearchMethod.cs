using System;
using System.Collections;
using System.Collections.Generic;

public interface FF3d_SearchMethod
{
    void ApplyMethod(ArrayList openNodes, ArrayList closedNodes);
    String toString();
}