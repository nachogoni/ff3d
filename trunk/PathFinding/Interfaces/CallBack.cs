using System;
using System.Collections;
using System.Collections.Generic;

public interface CallBack
{
    void Init();
    void CallBackFunction(ArrayList openNodes, ArrayList closedNodes, Node currentNode, Rule currentRule);
}