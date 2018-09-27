using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Procedural_deco : MonoBehaviour
{

	public int Probabilite_apparition = 33;

	void Activate_Deco ()
	{
		if (Random.Range (0, 100) > Probabilite_apparition)
			this.gameObject.SetActive(false);
		else
			return;		
	}

	void Start ()
	{
		Activate_Deco ();
	}
}
