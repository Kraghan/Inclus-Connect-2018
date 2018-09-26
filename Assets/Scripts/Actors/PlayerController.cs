﻿using System.Collections;
using System.Collections.Generic;
using Scripting.GameManagers;
using UnityEngine;

namespace Scripting.Actors
{
    internal enum EPlayerStates
    {
        Idle,
        Running,
        Sprinting,
        Jumping,
    }
    
    internal class PlayerController : ActorFSM
    {
        /// The player speed 
        [SerializeField]
        private float       m_speed = 5f;

        // Simple Jump
        [SerializeField]
        float m_simpleJumpDistance = 5f;


        [SerializeField]
        float m_simpleJumpDuration = 0.8f;

        /// The rigid Body
        private Rigidbody2D m_body;

        /// The inputs
        private ArduInput   m_inputs;
        internal ArduInput  inputs {get {return m_inputs;}}


        /// Jumping state datas
        // Jump destination
        Vector3 m_jumpDestination = Vector3.zero;

        // Jump duration
        float m_jumpDuration = 1f;

        /// Sprinting state
        // Sprint coefficient
        [SerializeField]
        float m_sprintCoefficient = 5f;

        /// Trail on good actions
        public TrailRenderer trail { get; private set; }

        /// Start
        void Start()
        {
            // Get body
            m_body      = GetComponent<Rigidbody2D>();

            // Get inputs
            m_inputs    = GetComponent<ArduInput>();

            // Register as current player
            Managers.instance.playerManager.player = this;

            // FMS
            m_actions = new System.Action[]{
/* IDLE     */  OnIdleState,      
/* RUNNING  */  OnRunningState,
/* SPRINTING*/  OnSprintingState,
/* JUMPING  */  OnJumpingState
            };

            // Get trail
            trail = GetComponent<TrailRenderer>();
        }

        /// Physics update
        void OnRunningState()
        {
            // ArduInput example : 
                // Check if button is active
            if (m_inputs.buttonOn)
            {
            }

                // Check if button has been activated on this frame
            if(m_inputs.buttonJustOn)
            {
                JumpTo(transform.position + Vector3.right * m_simpleJumpDistance, m_simpleJumpDuration);
            }

            m_body.velocity = new Vector2(m_speed, m_body.velocity.y);
        }

        /// Sprinting state
        void OnSprintingState()
        {
            if (m_firstFrameInState == true)
            {
                m_speed *= m_sprintCoefficient;
            }

            m_body.velocity = new Vector2(m_speed, m_body.velocity.y);
        }
        
        /// Idle State
        void OnIdleState()
        {
            if (m_firstFrameInState == true)
            {
                m_body.velocity = Vector3.zero;
            }

            // For now, always run
            m_nextState = (int)EPlayerStates.Running;

        }

        /// Orders to jump to a point
        internal void JumpTo(Vector3 p_position, float p_duration)
        {
            m_jumpDestination = p_position;
            m_jumpDuration = p_duration;

            // Jump state
            ForceState((int)EPlayerStates.Jumping);
        }

        void OnJumpingState()
        {
            if (m_firstFrameInState == true)
            {
                // Datas
                Vector3 position = transform.position;
                float dx = m_jumpDestination.x - position.x;
                float dy = m_jumpDestination.y - position.y;

                // Compute gravity
                float gravity = m_body.gravityScale * Physics.gravity.y;

                // Compte velocity
                float velX = dx / m_jumpDuration;
                float velY = -gravity * m_jumpDuration * 0.5f + dy / m_jumpDuration;

                // Jump
                m_body.velocity = new Vector3(velX, velY);
                
                Debug.DrawLine(position, position + new Vector3(velX, velY), Color.red, 1f);
                Debug.LogFormat("Jumping from {0} to {1} with velocity {2}", position, m_jumpDestination, new Vector3(velX, velY));

                Managers.instance.fxManager.SpawnFX(EFXType.Dust, transform.position);
            }

            // Exit condition - Jump done
            if (m_stateDuration >= m_jumpDuration)
            {
                m_nextState = (int)EPlayerStates.Running;
                // Debug.Break();
            }
        }

        /// Called before each update
        protected override void OnPreStateAction()
        {
        }

        /// Called after each update
        protected override void OnPostStateAction()
        {
        }

        /// Callback - State changed
        protected override void OnStateChanged()
        {
            if (m_previousState == (int)EPlayerStates.Sprinting)
                m_speed /= m_sprintCoefficient;
        }
    }
}