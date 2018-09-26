﻿using System.Collections;
using System.Collections.Generic;
using Scripting.Actors;
using Scripting.GameManagers;
using UnityEngine;

namespace Scripting.QTE
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
    public abstract class QuickTimeEvent : MonoBehaviour
    {
        /// All the QTE required
        [SerializeField]
        private QTEDone[] m_QTENeeded = new QTEDone[1];

        /// Input shortcut
        internal static ArduInput s_inputs { get {return Managers.instance.playerManager.inputs;}}


        /// The box collider
        private BoxCollider2D m_collider;

        /// Success bonus added to next same QTE
        [SerializeField]
        [Range(0, 1)]
        private float m_succesPercentageToAdd = 0.05f;

        /// Malus bonus added to next same QTE
        [SerializeField]
        [Range(0, 1)]
        private float m_failurePercentageToRemove = 0.1f;

        /// Start
        void Start()
        {
            // Get collider
            m_collider = GetComponent<BoxCollider2D>();

            enabled = false;
        }
        
        /// Update
        void FixedUpdate()
        {
            // Update inputs
            UpdateQTERequierement();

            /// Check if succeedded
            bool allDone = true;

            foreach (QTEDone qte in m_QTENeeded)
            {
                if (!qte.p_done)
                {
                    allDone = false;
                    break;
                }
            }
            ///
            
            // If QTE is triggered
            if(allDone == true)
            {
                // Disable update
                enabled = false; 

                // Disable slow motion
                Utility.TimeManager.StopSlowMotion();


                /// Add bonus|malus relative to current position of player in box
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

                OnQTESucceeded();
            }
            // If QTE is not triggered
            else
            {
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

            OnPlayerEntered();

            enabled = true;
        }

        /// Callback - Triger enter
        void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            //PlayerData data = Managers.instance.playerManager.GetWeakestPlayer(m_QTENeeded);

            Utility.TimeManager.StopSlowMotion();

            OnPlayerExited();

            enabled = false;
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

        /// Return current X progress of player in collider
        protected float GetPositionInCollider()
        {
            float playerXPos = Managers.instance.playerManager.player.transform.position.x;

            float qteSize = m_collider.size.x * transform.localScale.x;

            float qteLeftPosition   = m_collider.bounds.min.x;

            float progress = (playerXPos - qteLeftPosition) / qteSize;
            return Mathf.Clamp01(progress);
        }

        /// Player entered area
        protected abstract void OnPlayerEntered();

        /// Player exited area
        protected abstract void OnPlayerExited();

        /// Player succeeded inputs
        protected virtual void OnQTESucceeded()
        {
            Managers.instance.playerManager.player.ForceState((int)EPlayerStates.Sprinting);
        }
    }

}