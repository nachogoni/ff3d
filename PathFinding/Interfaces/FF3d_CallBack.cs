using System;
using System.Collections;
using System.Collections.Generic;

public interface FF3d_CallBack
{
    void Init();
    void CallBackFunction(ArrayList openNodes, ArrayList closedNodes, FF3d_Node currentNode, FF3d_Rule currentRule);
}