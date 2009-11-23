using UnityEngine;
using System;
using System.Collections.Generic;

public class FF3d_Node {
	
	private static int id;
	
	private FF3d_State state;
	private float gSum;
	private float h;
	private FF3d_Rule rule;
	private FF3d_Node parent;
	private int nodeId;
	private int level;
	
	// Genera el nodo raiz
	public FF3d_Node(FF3d_State state, float h, FF3d_Rule rule) {
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

    public FF3d_Rule getRule()
    {
        return this.rule;
    }

	public void setRule(FF3d_Rule rule) {
		this.rule = rule;
	}

	public int getNodeId() {
		return nodeId;
	}

	public FF3d_Node(FF3d_State state, float h, float gSum, FF3d_Node parent, FF3d_Rule rule) {
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
		return ((FF3d_Node)(other)).getState().equals(this.state);
	}
	
	public FF3d_State getState()
    {
		return state;
	}

	public void setState(FF3d_State state) {
		this.state = state;
	}

	public float getGSum() {
		return gSum;
	}

	public void setGSum(float sum) {
		gSum = sum;
	}

	public FF3d_Node getParent() {
		return parent;
	}

	public void setParent(FF3d_Node parent) {
		this.parent = parent;
	}
	
	public FF3d_Node getRoot() {
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
