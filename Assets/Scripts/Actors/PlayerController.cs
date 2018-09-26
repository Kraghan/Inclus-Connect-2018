using System.Collections;
using System.Collections.Generic;
using Scripting.GameManagers;
using UnityEngine;

namespace Scripting.Actors
{
    internal enum EPlayerStates
    {
        Idle,
        Running,
        Jumping,
        Attacking,
        Defending,
        Ghosting,
    }

    internal enum EPlayerForm
    {
        Default,
        Ghost
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

        // Renderer
        [SerializeField]
        Renderer m_render = null;



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

        internal bool isRunning = false;

        /// Attacking state datas
        GameObject m_attackTarget = null;

        // The attack animation duration
        float m_attackDuration = 0.3f;

        /// Defending state datas
        GameObject m_defendTarget = null;

        /// Ghost state data
        EPlayerForm m_form = EPlayerForm.Default;
        internal EPlayerForm form {get {return m_form;} set {m_form = value;}}

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
/* JUMPING  */  OnJumpingState,
/* ATTACKING*/  OnAttackingState,
/* DEFENDING*/  OnDefendingState,
/* GHOSTING */  OnGhostingState,
            };

            // Get trail
            trail = GetComponent<TrailRenderer>();
        }

        /// Physics update
        void OnRunningState()
        {
            m_body.velocity = new Vector2(m_speed * (isRunning ? m_sprintCoefficient : 1f), m_body.velocity.y);
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

        /// Orders to attack an object
        internal void Attack(GameObject p_target)
        {
            m_attackTarget = p_target;

            ForceState((int)EPlayerStates.Attacking);
        }

        /// Orders to defend
        internal void Defend(GameObject p_target)
        {
            m_defendTarget = p_target;

            ForceState((int)EPlayerStates.Defending);
        }

        /// Orders to change form
        internal void EnterForm(EPlayerForm p_form)
        {
            m_form = p_form;

            ForceState((int)EPlayerStates.Ghosting);
        }

        /// Update right form
        internal void UpdateForm()
        {
            m_form = m_inputs.lightOn == true ? EPlayerForm.Default : EPlayerForm.Ghost;
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

        /// Attacking state
        void OnAttackingState()
        {
            if (m_stateDuration > m_attackDuration)
            {
                m_nextState = (int)EPlayerStates.Running;
                isRunning = false;
                m_attackTarget.SetActive(false);
            }
        }

        /// Defending state
        void OnDefendingState()
        {
            if (m_defendTarget.activeSelf == false)
            {
                isRunning = false;
                m_nextState = (int)EPlayerStates.Running;
            }
        }

        /// Ghost state
        void OnGhostingState()
        {
            if (m_stateDuration > .3f)
            {
                isRunning = false;
                UpdateForm();
                m_nextState = (int)EPlayerStates.Running;
            }
        }

        /// Called before each update
        protected override void OnPreStateAction()
        {
            // Allow controls only if not sprinting
            if(isRunning == false)
            {
                // Check if button has been activated on this frame
                if(m_inputs.buttonJustOn == true && m_currentState != (int)EPlayerStates.Jumping)
                {
                    JumpTo(transform.position + Vector3.right * m_simpleJumpDistance, m_simpleJumpDuration);
                }

                if (m_currentState != (int)EPlayerStates.Ghosting)
                    UpdateForm();
            }
            
            m_render.material.SetFloat("_isGhost", m_form == EPlayerForm.Default ? 0 : 1);

        }

        /// Called after each update
        protected override void OnPostStateAction()
        {
        }

        /// Callback - State changed
        protected override void OnStateChanged()
        {
            Debug.LogFormat("Player switching to state {0}.", (EPlayerStates) m_currentState);
        }
    }
}