using UnityEngine;
using System;
using System.Collections.Generic;

public class Node {
	
	private static int id;
	
	private State state;
	private float gSum;
	private float h;
	private Rule rule;
	private Node parent;
	private int nodeId;
	private int level;
	
	// Genera el nodo raiz
	public Node(State state, float h, Rule rule) {
		this.state = state;
		this.gSum = 0.0f;
		this.h = h;
		this.parent = null;
		this.nodeId = id = 0;
		this.rule = rule;
		this.level = 0;
	}
	
	public float getH() {
		return h;
	}

	public void setH(float h) {
		this.h = h;
	}

    public Rule getRule()
    {
        return this.rule;
    }

	public void setRule(Rule rule) {
		this.rule = rule;
	}

	public int getNodeId() {
		return nodeId;
	}

	public Node(State state, float h, float gSum, Node parent, Rule rule) {
		this.state=state;
		this.gSum = gSum;
		this.h = h;
		this.parent = parent;
		this.level = parent.getLevel() + 1;
		this.rule = rule;
		this.nodeId = id++;
	}
	
	public int getLevel() {
		return level;
	}

	public bool equals(System.Object other) {		
		return ((Node)(other)).getState().equals(this.state);
	}
	
	public State getState()
    {
		return state;
	}

	public void setState(State state) {
		this.state = state;
	}

	public float getGSum() {
		return gSum;
	}

	public void setGSum(float sum) {
		gSum = sum;
	}

	public Node getParent() {
		return parent;
	}

	public void setParent(Node parent) {
		this.parent = parent;
	}
	
	public Node getRoot() {
		if (this.parent == null) {
			return this;
		}
		return this.parent.getRoot();
	}
	
	public String toString() {
		if (this.rule == null)
			return "Initial State: " + this.state.toString();
		else
            return "\nId:" + this.getNodeId() + " H: " + this.getH() + " G: " + this.getGSum() + " Rule: " + this.rule.toString() +
                " Parent RULE: " + ((this.parent.getRule() != null) ? this.parent.getRule().toString() : "-") + " State: " + this.state.toString();
	}
}
