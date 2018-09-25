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

    [RequireComponent(typeof(Collider2D))]
    public class QuickTimeEvent : MonoBehaviour
    {
        [SerializeField]
        private QTEType[] m_QTENeeded = new QTEType[1];
        protected static PlayerDatas s_player = null;

        // Use this for initialization
        void Start()
        {
            s_player = GameObject.FindObjectOfType<PlayerDatas>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            PlayerData data = s_player.GetWeakestPlayer(m_QTENeeded);

            Utility.TimeManager.StartSlowMotion(data.GetInitialSlowMotionFactor());
        }
    }

}