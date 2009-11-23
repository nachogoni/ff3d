using System;
using System.Collections;
using System.Collections.Generic;

public class FF3d_MinDistance : FF3d_Heuristics
{

	public float getHeuristic(FF3d_State state)
    {
        FF3d_TankState tankState = ((FF3d_TankState)state);

        List<FF3d_PosRowCol> flagsPos = tankState.getFlags();
        
        FF3d_PosRowCol pos = tankState.getRowCol();
        
        int minDist = int.MaxValue, dist = 0;

        foreach(FF3d_PosRowCol flag in flagsPos)
        {
            if ((dist = Math.Abs(flag.colValue - pos.colValue) + Math.Abs(flag.rowValue - pos.rowValue)) < minDist)
            {
                minDist = dist;
            }
        }

        return minDist;
	}

    public String toString()
    {
		return "MinDistance";
	}
}