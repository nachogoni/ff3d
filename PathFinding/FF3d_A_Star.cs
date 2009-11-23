using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FF3D
{

    public class FF3d_AStar : FF3d_SearchMethod
    {

        private void removeRepeatedNodes(ArrayList openNodes, ArrayList closedNodes)
        {
            ArrayList toRemove = new ArrayList();

            //Los nodos recien aniadidos a abiertos, chekeo que no esten en cerrados
            foreach (FF3d_Node node in openNodes)
            {
                foreach (FF3d_Node nodeClosed in closedNodes)
                {
                    if (nodeClosed.equals(node))
                    {
                        toRemove.Add(node);
                    }
                }
            }

            foreach (FF3d_Node node in toRemove)
                openNodes.Remove(node);
        }

        public void ApplyMethod(ArrayList openNodes, ArrayList closedNodes)
        {
            float min = float.MaxValue;
            int minNode = 0;
            FF3d_Node currNode, backup;

            removeRepeatedNodes(openNodes, closedNodes);

            /* Ubico como proximo a expander (primero en lista de abiertos) al que tiene menor f */
            for (int i = 0; i < openNodes.Count; i++)
            {
                currNode = (FF3d_Node)(openNodes[i]);

                if ((currNode.getH() != float.MaxValue) && (currNode.getH() + currNode.getGSum() < min))
                {
                    min = currNode.getH() + currNode.getGSum();
                    minNode = i;
                }
            }

            if (openNodes.Count > 0)
            {
                backup = (FF3d_Node)openNodes[0];
                currNode = (FF3d_Node)openNodes[minNode];
                openNodes.Insert(0, currNode);
                openNodes.Insert(minNode, backup);
            }
        }

        public String toString()
        {
            return "A*";
        }

    }

}