using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 5f;
    Rigidbody2D body;

	// Use this for initialization
	void Start ()
    {
        body = GetComponent<Rigidbody2D>();
        
	}
	
	void FixedUpdate ()
    {
        body.velocity = new Vector2(speed, body.velocity.y);
	}
    
}
