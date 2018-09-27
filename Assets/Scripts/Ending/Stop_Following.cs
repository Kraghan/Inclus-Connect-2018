using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop_Following : MonoBehaviour {

	public Cinemachine.CinemachineVirtualCamera lacamera;
	//public Transform Followme;
	//public Transform Player;

	void OnTriggerEnter2D (Collider2D other)
	{
		//Followme.position = Player.position; 
		lacamera.m_Follow = null;
	}
}
