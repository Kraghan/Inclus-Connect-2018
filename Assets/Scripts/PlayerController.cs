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

        // Use this for initialization
        void Start()
        {
            m_body = GetComponent<Rigidbody2D>();

        }

        void FixedUpdate()
        {
            m_body.velocity = new Vector2(m_speed, m_body.velocity.y);
        }

    }
}