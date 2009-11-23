using System;
using System.Collections;
using System.Collections.Generic;

public class MinDistance : Heuristics
{

	public float getHeuristic(State state)
    {
        FF3d_TankState tankState = ((FF3d_TankState)state);

        List<PosRowCol> flagsPos = tankState.getFlags();
        
        PosRowCol pos = tankState.getRowCol();
        
        int minDist = int.MaxValue, dist = 0;

        foreach(PosRowCol flag in flagsPos)
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