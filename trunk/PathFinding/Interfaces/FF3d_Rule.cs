using System;
using System.Collections.Generic;

public interface FF3d_Rule
{
	bool isApplicable(FF3d_State state);
	FF3d_State apply(FF3d_State state);
	float getCost(FF3d_Rule lastRule);
	String toString();
}