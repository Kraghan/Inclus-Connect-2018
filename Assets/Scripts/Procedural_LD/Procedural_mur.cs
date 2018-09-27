using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Procedural_mur : MonoBehaviour
{

	public Sprite[] Mur_random;

	void Random_wall ()
	{
		int rng = 0;

		rng = Random.Range (0, Mur_random.Length);

		GetComponent<SpriteRenderer> ().sprite = Mur_random [rng];
	}

	void Start ()
	{
		Random_wall ();
	}
}
