using System.Collections;
using System.Collections.Generic;
using Scripting.Actors;
using Scripting.GameManagers;
using UnityEngine;

namespace Scripting
{
    public enum QTEType
    {
        MICRO,
        LIGHT,
        ACCELERO,
        BUTTON
    }

    [System.Serializable]
    public class QTEDone
    {
        [SerializeField]
        private QTEType m_type;

        public QTEType type
        {
            get { return m_type; }
        }

        public bool p_done = false;
    }

    [RequireComponent(typeof(BoxCollider2D))]
    public class QuickTimeEvent : MonoBehaviour
    {
        /// TEMP
        [SerializeField]
        GameObject m_destination = null;

        [SerializeField]
        float m_duration = 1f;
        ///

        [SerializeField]
        private QTEDone[] m_QTENeeded = new QTEDone[1];
        internal static ArduInput s_inputs { get {return Managers.instance.playerManager.inputs;}}

        private bool m_activated = false;
        private BoxCollider2D m_collider;

        [SerializeField]
        [Range(0, 1)]
        private float m_succesPercentageToAdd = 0.05f;

        [SerializeField]
        [Range(0, 1)]
        private float m_failurePercentageToRemove = 0.1f;

        // Use this for initialization
        void Start()
        {
            m_collider = GetComponent<BoxCollider2D>();
        }
        
        void FixedUpdate()
        {
            if (!m_activated)
                return;

            UpdateQTERequierement();

            bool allDone = true;

            foreach (QTEDone qte in m_QTENeeded)
            {
                if (!qte.p_done)
                {
                    allDone = false;
                    break;
                }
            }
            
            // If QTE is triggered
            if(allDone)
            {
                m_activated = false;
                Utility.TimeManager.StopSlowMotion();

                float position = GetPositionInCollider();

                // Early trigger
                if(position < 0.5)
                {
                    Managers.instance.playerManager.GetPlayer(m_QTENeeded[0].type).skill += Mathf.Lerp(m_succesPercentageToAdd, 0, position * 2);
                }
                // Late Trigger
                else
                {
                    Managers.instance.playerManager.GetPlayer(m_QTENeeded[0].type).skill -= Mathf.Lerp(0, m_failurePercentageToRemove, position * 2 - 1);
                }

                Mathf.Clamp(Managers.instance.playerManager.GetPlayer(m_QTENeeded[0].type).skill, 0.1f, 1);
            }
            // If QTE is not triggered
            else
            {
                // Debug.LogFormat("{0} - {1}", Managers.instance.playerManager.GetPlayer(m_QTENeeded[0].type).GetSlowMotionMin(), GetPositionInCollider() );
                // Update slowmo
                Utility.TimeManager.StartSlowMotion(Mathf.Lerp(1, Managers.instance.playerManager.GetPlayer(m_QTENeeded[0].type).GetSlowMotionMin(), GetPositionInCollider()));
            }
        }

        /// Callback - Triger enter
        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            //PlayerData data = s_player.GetWeakestPlayer(m_QTENeeded);

            Utility.TimeManager.StartSlowMotion(1);

            if (Managers.instance.playerManager.inputs.buttonJustOn == true)
                Managers.instance.playerManager.player.ForceState((int)PlayerController.EPlayerStates.Sprinting);

            m_activated = true;
        }

        /// Callback - Triger enter
        void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            //PlayerData data = Managers.instance.playerManager.GetWeakestPlayer(m_QTENeeded);

            Utility.TimeManager.StopSlowMotion();

            Managers.instance.playerManager.player.JumpTo(m_destination.transform.position, m_duration);

            m_activated = false;
        }

        private void UpdateQTERequierement()
        {
            foreach (QTEDone qte in m_QTENeeded)
            {
                if (qte.p_done)
                    continue;

                switch (qte.type)
                {
                    case QTEType.ACCELERO:
                        if (s_inputs.acceleroOn)
                            qte.p_done = true;
                        break;

                    case QTEType.BUTTON:
                        if (s_inputs.buttonOn)
                            qte.p_done = true;
                        break;

                    case QTEType.LIGHT:
                        if (s_inputs.lightOn)
                            qte.p_done = true;
                        break;

                    case QTEType.MICRO:
                        if (s_inputs.microOn)
                            qte.p_done = true;
                        break;
                }
            }
        }

        private float GetPositionInCollider()
        {
            float playerXPos = Managers.instance.playerManager.player.transform.position.x;

            float qteSize = m_collider.size.x * transform.localScale.x;

            float qteLeftPosition   = m_collider.bounds.min.x;
            float qteRightPosition  = m_collider.bounds.max.x;

            float progress = (playerXPos - qteLeftPosition) / qteSize;
            Debug.Log(progress);
            return Mathf.Clamp01(progress);
        }
    }

}