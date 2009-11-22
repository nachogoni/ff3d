using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Engine
{
    SearchMethod sm;
    String sol;
    ArrayList rules;
    Rule rule = null;
    Problem problem;
    int totalNodes = 0;
    int maxLevel = 0;
    int prunedCount = 0;
    int leafsCount = 0;
    float totalCost = 0.0f;
    int expandedNodes = 0;
    bool stop = false;
    CallBack callback = null;

    public void setLastRule(Rule rule)
    {
        this.rule = rule;
    }

    public int getPruned()
    {
        return prunedCount;
    }

    public int getLeafs()
    {
        return leafsCount;
    }

    public int getExpandedNodes()
    {
        return expandedNodes;
    }

    public int getTotalNodes()
    {
        return totalNodes;
    }

    public CallBack getCallback()
    {
        return callback;
    }

    public void setCallback(CallBack callback)
    {
        this.callback = callback;
    }

    public Engine(Problem problem)
    {
        this.problem = problem;
        this.rules = new ArrayList();
    }

    public void stopEngine()
    {
        this.stop = true;
        return;
    }

    public float getCost()
    {
        return totalCost;
    }

    public bool run(SearchMethod sm)
    {
        bool solved = false;

        Node currentNode = null;
        Rule currentRule = null;

        State init = problem.getInitState();

        if (init == null)
            return false;

        ArrayList openNodes = new ArrayList();
        ArrayList closedNodes = new ArrayList();
        ArrayList rules = problem.getRules();

        if (sm == null)
            return false;

        this.sm = sm;

        this.sol = "";
        this.rules.Clear();

        this.totalNodes = 0;
        this.maxLevel = 0;
        this.prunedCount = 0;
        this.totalCost = 0.0f;
        this.expandedNodes = 0;

        if (callback != null)
            callback.Init();

        currentNode = new Node(init, this.problem.getHeuristic(init), this.rule);
        openNodes.Add(currentNode);

        while (openNodes.Count > 0 && !solved && !stop)
        {

            // Agarro el primer nodo de abiertos
            currentNode = (Node)openNodes[0];
            // Si es gol, termino
            if (this.problem.isGoal(currentNode.getState()))
            {
                solved = true;
                continue;
            }

            //Remuevo al nodo de la lista de abiertos y lo meto en cerrados
            openNodes.RemoveAt(0);
            closedNodes.Add(currentNode);

            // Cantidad de nodos expandidos
            this.expandedNodes++;

            //Pido un iterador sobre las reglas y expando el currentNode
            foreach (Rule r in rules)
            {
                currentRule = r;

                // Si la regla es aplicable, expando el nodo
                if (currentRule.isApplicable(currentNode.getState()))
                {
                    //Creo un nuevo estado
                    State state = currentRule.apply(currentNode.getState());
                    //Aplico la regla sobre el estado y armo un nuevo nodo con el nuevo estado
                    Node aux = new Node(state, problem.getHeuristic(state),
                            currentRule.getCost(currentNode.getRule()) == float.MaxValue ? float.MaxValue : currentNode.getGSum() + currentRule.getCost(currentNode.getRule()),
                            currentNode, currentRule);

                    // Cantidad de nodos creados
                    this.totalNodes++;

                    // Nivel del nodo creado
                    if (this.maxLevel < aux.getLevel())
                        this.maxLevel = aux.getLevel();

                    //Busco en los padres y en todos los nodos de Abiertos y Cerrados
                    if (!searchParentState(currentNode, aux.getState()))
                    {
                        //Lo aniado a la lista de nodos abiertos
                        openNodes.Add(aux);
                    }
                    else
                    {
                        // Fue podado
                        this.prunedCount++;
                    }

                    if (callback != null)
                        callback.CallBackFunction(openNodes, closedNodes, aux, currentRule);
                }

            }

            //Llamo al metodo de busqueda
            this.sm.ApplyMethod(openNodes, closedNodes);
        }
        
        // Canitdad de nodos en abiertos -> # de hojas en el arbol 
        this.leafsCount = openNodes.Count;

        // Costo de la solucion -> costo de llegar al nodo gol 
        this.totalCost = currentNode.getGSum();

        if (solved)
        {
            printSolution(currentNode);
            //printSolution(openNodes.get(0));
        }

        return solved;
    }

    private bool searchParentState(Node node, State compareTo)
    {
        if (node == null)
            return false;

        if (compareTo.equals(node.getState()))
            return true;
        else
            return searchParentState(node.getParent(), compareTo);
    }

    private void printSolution(Node leaf)
    {
        if (leaf == null)
            return;
        printSolution(leaf.getParent());
        if (leaf.getParent() != null)
        {
            this.rules.Add(leaf.getRule());
            this.sol += leaf.toString();
        }
    }

    public ArrayList getRules()
    {
        return rules;
    }

    public int getMaxLevel()
    {
        return maxLevel;
    }

    public String getSolution()
    {
        return this.sol;
    }

    public String toString()
    {
        return this.sol;
    }

}