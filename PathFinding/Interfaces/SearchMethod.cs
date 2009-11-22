using System;
using System.Collections;
using System.Collections.Generic;

public interface SearchMethod
{
    void ApplyMethod(ArrayList openNodes, ArrayList closedNodes);
    String toString();
}