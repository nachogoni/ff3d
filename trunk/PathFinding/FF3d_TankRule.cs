using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FF3d_TankRule : FF3d_Rule{
	public FF3d_MoveTypes moveValue;
	
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

    public FF3d_TankRule(FF3d_MoveTypes move) {
        this.moveValue = move;
	}
	
	public bool isApplicable(FF3d_State state){
        return ((FF3d_TankState)state).isValidMove(this.moveValue);
	}
	
	public FF3d_State apply(FF3d_State state)
    {
		FF3d_TankState newState;

        if (!((FF3d_TankState)state).isValidMove(this.moveValue))
        {
			return null;
		}

		newState = ((FF3d_TankState)state).duplicate();
        newState.move(this.moveValue);
		
		return (FF3d_State)newState;
	}
	
	public float getCost(FF3d_Rule lastRule) {

        return ruleCosts[(int)( (lastRule == null) ? moveValue : ((FF3d_TankRule)lastRule).moveValue), (int)moveValue];

	}
	
	public String toString() {
        return moveValue.ToString();
	}

    public static int getRowIncrement(FF3d_MoveTypes move)
    {
        switch (move)
        {
            case FF3d_MoveTypes.DOWN:
            case FF3d_MoveTypes.DOWNLEFT:
            case FF3d_MoveTypes.DOWNRIGHT:
                return 1;
            case FF3d_MoveTypes.UP:
            case FF3d_MoveTypes.UPLEFT:
            case FF3d_MoveTypes.UPRIGHT:
                return -1;
        }

        return 0;
    }

    public static int getColIncrement(FF3d_MoveTypes move)
    {
        switch (move)
        {
            case FF3d_MoveTypes.LEFT:
            case FF3d_MoveTypes.DOWNLEFT:
            case FF3d_MoveTypes.UPLEFT:
                return -1;
            case FF3d_MoveTypes.RIGHT:
            case FF3d_MoveTypes.DOWNRIGHT:
            case FF3d_MoveTypes.UPRIGHT:
                return 1;
        }

        return 0;
    }

}
