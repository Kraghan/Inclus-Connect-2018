using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QTEType
{
    MICRO,
    LIGHT,
    ACCELERO,
    BUTTON
}

public class QuickTimeEvent : MonoBehaviour
{

    public QTEType[] QTENeeded = new QTEType[1];
    protected static PlayerDatas player = null;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindObjectOfType<PlayerDatas>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerData data = player.GetWeakestPlayer(QTENeeded);

        TimeManager.StartSlowMotion(data.GetInitialSlowMotionFactor());
    }
}
