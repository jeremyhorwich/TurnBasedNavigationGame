using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_ExplodeForward : a_Explode
{
	private void OnEnable()
	{
		targetDir = Vector3Int.RoundToInt(transform.forward);
	}
}
