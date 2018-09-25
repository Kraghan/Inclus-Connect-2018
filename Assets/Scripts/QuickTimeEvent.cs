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

    [RequireComponent(typeof(BoxCollider2D))]
    public class QuickTimeEvent : MonoBehaviour
    {
        [SerializeField]
        private QTEDone[] m_QTENeeded = new QTEDone[1];
        protected static PlayerDatas s_player = null;
        protected static ArduInput s_inputs = null;

        private bool m_activated = false;
        private BoxCollider2D m_collider;

        // Use this for initialization
        void Start()
        {
            if(!s_player)
                s_player = GameObject.FindObjectOfType<PlayerDatas>();
            if (!s_inputs)
                s_inputs = s_player.player.GetComponent<ArduInput>();

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
            
            if(allDone)
            {
                m_activated = false;
                Utility.TimeManager.StopSlowMotion();

                GetPositionInCollider();
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            //PlayerData data = s_player.GetWeakestPlayer(m_QTENeeded);

            Utility.TimeManager.StartSlowMotion(0.25f);

            m_activated = true;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            GetPositionInCollider();
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
            float playerXPos = s_player.player.transform.position.x;

            float qteSize = m_collider.size.x * transform.localScale.x;

            float qteLeftPosition   = transform.position.x - qteSize / 2;
            float qteRightPosition  = transform.position.x + qteSize / 2;

            //print(qteLeftPosition + " " + qteRightPosition + " " + playerXPos);

            print((qteLeftPosition - playerXPos) / (qteLeftPosition - qteRightPosition));

            return Mathf.Clamp01(qteLeftPosition - playerXPos) / (qteLeftPosition - qteRightPosition);
        }
    }

}