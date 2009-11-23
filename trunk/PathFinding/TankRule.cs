using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TankRule : Rule{
	public MoveTypes moveValue;
	
    float [,] ruleCosts = new float [,] 
    
    {            // DOWN, UP,   LEFT, RIGHT, DOWNLEFT, DOWNRIGHT, UPLEFT, UPRIGHT
   /* DOWN      */ {1.0f, 3.0f, 2.0f, 2.0f,  1.5f,     1.5f,      2.5f,   2.5f},
   /* UP        */ {3.0f, 1.0f, 2.0f, 2.0f,  2.5f,     2.5f,      1.5f,   1.5f},
   /* LEFT      */ {2.0f, 2.0f, 1.0f, 3.0f,  1.5f,     2.5f,      1.5f,   2.5f},
   /* RIGHT     */ {2.0f, 2.0f, 3.0f, 1.0f,  2.5f,     1.5f,      2.5f,   1.5f},
   /* DOWNLEFT  */ {1.5f, 2.5f, 1.5f, 2.5f,  1.0f,     2.0f,      2.0f,   3.0f},
   /* DOWNRIGHT */ {1.5f, 2.5f, 2.5f, 1.5f,  2.0f,     1.0f,      3.0f,   2.0f},
   /* UPLEFT    */ {2.5f, 1.5f, 1.5f, 2.5f,  2.0f,     3.0f,      1.0f,   2.0f},
   /* UPRIGHT   */ {2.5f, 1.5f, 2.5f, 1.5f,  3.0f,     2.0f,      2.0f,   1.0f},
    };

    public TankRule(MoveTypes move) {
        this.moveValue = move;
	}
	
	public bool isApplicable(State state){
        return ((FF3d_TankState)state).isValidMove(this.moveValue);
	}
	
	public State apply(State state)
    {
		FF3d_TankState newState;

        if (!((FF3d_TankState)state).isValidMove(this.moveValue))
        {
			return null;
		}

		newState = ((FF3d_TankState)state).duplicate();
        newState.move(this.moveValue);
		
		return (State)newState;
	}
	
	public float getCost(Rule lastRule) {

        return ruleCosts[(int)( (lastRule == null) ? moveValue : ((TankRule)lastRule).moveValue), (int)moveValue];

	}
	
	public String toString() {
        return moveValue.ToString();
	}

    public static int getRowIncrement(MoveTypes move)
    {
        switch (move)
        {
            case MoveTypes.DOWN:
            case MoveTypes.DOWNLEFT:
            case MoveTypes.DOWNRIGHT:
                return 1;
            case MoveTypes.UP:
            case MoveTypes.UPLEFT:
            case MoveTypes.UPRIGHT:
                return -1;
        }

        return 0;
    }

    public static int getColIncrement(MoveTypes move)
    {
        switch (move)
        {
            case MoveTypes.LEFT:
            case MoveTypes.DOWNLEFT:
            case MoveTypes.UPLEFT:
                return -1;
            case MoveTypes.RIGHT:
            case MoveTypes.DOWNRIGHT:
            case MoveTypes.UPRIGHT:
                return 1;
        }

        return 0;
    }

}
