using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripting
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float       m_speed = 5f;
        private Rigidbody2D m_body;
        private ArduInput   m_inputs;

        // Use this for initialization
        void Start()
        {
            m_body      = GetComponent<Rigidbody2D>();
            m_inputs    = GetComponent<ArduInput>();
        }

        void FixedUpdate()
        {
            // ArduInput example : 
                // Check if button is active
            if (m_inputs.buttonOn)
            {

            }

                // Check if button has been activated on this frame
            if(m_inputs.buttonJustOn)
            {

            }


            m_body.velocity = new Vector2(m_speed, m_body.velocity.y);
        }

    }
}