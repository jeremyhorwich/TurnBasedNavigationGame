using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
	//Consider using a scriptable object for each of these types and adding that to the Actor method that way? 
	//Would still need to add as monobehaviour for things like dynamic targets though...

	int Priority { get; }
	bool ActConditionIsMet { get; }
	void Act();

}
