using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripting
{

    [System.Serializable]
    public class PlayerData
    {
        [SerializeField]
        private QTEType m_playerController;
        [SerializeField]
        private bool    m_enable              = false;
        [SerializeField]
        private bool    m_isDisabledPlayer    = false;
        [SerializeField]
        private float   m_skill               = 0.5f;

        public QTEType playerController
        {
            get { return m_playerController;  }
            set { m_playerController = value;  }
        }

        public bool enable
        {
            get { return m_enable; }
            set { enable = value; }
        }

        public bool isDisablePlayer
        {
            get { return m_isDisabledPlayer; }
            set { m_isDisabledPlayer = value; }
        }

        public float skill
        {
            get { return m_skill; }
            set { m_skill = value; }
        }

        public float GetInitialSlowMotionFactor()
        {
            return 0.05f;
        }
    }

    public class PlayerDatas : MonoBehaviour
    {
        [SerializeField]
        private PlayerData[] m_datas = new PlayerData[4];


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public PlayerData GetWeakestPlayer(QTEType[] types)
        {
            uint weakestIndex = 0;

            for (uint i = 0; i < m_datas.Length; ++i)
            {
                foreach (QTEType type in types)
                {
                    if (m_datas[i].playerController == type
                        && m_datas[i].skill < m_datas[weakestIndex].skill)
                    {
                        weakestIndex = i;
                        break;
                    }
                }
            }

            return m_datas[weakestIndex];
        }


    }
}