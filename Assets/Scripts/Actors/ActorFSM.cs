using UnityEngine;


namespace Scripting.Actors
{
    internal abstract class ActorFSM : MonoBehaviour
    {
        /// FSM
        protected int m_previousState = -1;
        protected int m_currentState = -1;
        protected int m_nextState = -1;

        /// True if first frame in current state
        protected bool m_firstFrameInState = false;

        /// The duration in current state
        protected float m_stateDuration = 0f;

        /// State => method
        protected System.Action[] m_actions = null; // YOU MUST SETUP THIS ARRAY MANUALLY

        // onStateChanged(p_previousState, p_newState)
        protected System.Action<int, int> onStateChanged = null;

        /// Awake
        void Awake()
        {
            InitializeFSM();
        }

        /// Fixed Update
        void FixedUpdate()
        {
            // Before action
            OnPreStateAction();

            // Update
            m_actions[m_currentState]();

            // After action
            OnPostStateAction();

            // Count state duration
            m_stateDuration += Time.fixedDeltaTime;

            // Finalize transition
            FinalizeFSM();
        }

        // Setup transition
        void FinalizeFSM()
        {
            m_firstFrameInState = false;

            if (m_nextState == m_currentState)
                m_nextState = -1;

            if (m_nextState != -1)
            {
                m_previousState = m_currentState;
                m_currentState = m_nextState;
                m_nextState = -1;

                m_stateDuration = 0f;

                m_firstFrameInState = true;

                if (onStateChanged != null)
                    onStateChanged(m_previousState, m_currentState);

                // Inner callback
                OnStateChanged();
            }
        }

        /// Initialize the FSM
        void InitializeFSM()
        {
            m_currentState = 0;
        }

        /// Force a state
        internal void ForceState(int p_state)
        {
            m_nextState = p_state;
            FinalizeFSM();
            FixedUpdate();
        }

        /// Called before each update
        abstract protected void OnPreStateAction();

        /// Called after each update
        abstract protected void OnPostStateAction();


        /// Called switching state each update
        abstract protected void OnStateChanged();
    }
}