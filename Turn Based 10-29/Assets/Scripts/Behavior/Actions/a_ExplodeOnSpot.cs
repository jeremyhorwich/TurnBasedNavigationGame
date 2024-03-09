using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_ExplodeOnSpot : a_Explode
{   
	private void OnEnable()
	{
		targetDir = Vector3.zero;
	}
}
