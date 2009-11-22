using System;
using System.Collections.Generic;

public interface Rule
{
	bool isApplicable(State state);
	State apply(State state);
	float getCost(Rule lastRule);
	String toString();
}