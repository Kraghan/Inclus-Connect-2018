using System.Collections;
using System.Collections.Generic;
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

    [RequireComponent(typeof(Collider2D))]
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
        internal static ArduInput s_inputs { get {return PlayerDatas.instance.player.GetComponent<ArduInput>();}}

        private bool m_activated = false;
        
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
            
            if(allDone)
            {
                m_activated = false;
                Utility.TimeManager.StopSlowMotion();
            }
        }

        /// Callback - Triger enter
        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            //PlayerData data = s_player.GetWeakestPlayer(m_QTENeeded);

            Utility.TimeManager.StartSlowMotion();

            m_activated = true;
        }

        /// Callback - Triger enter
        void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            //PlayerData data = PlayerDatas.instance.GetWeakestPlayer(m_QTENeeded);

            Utility.TimeManager.StopSlowMotion();

            PlayerDatas.instance.player.JumpTo(m_destination.transform.position, m_duration);

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
    }

}