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
        internal static PlayerDatas s_player = null;

        // Use this for initialization
        void Start()
        {
            s_player = PlayerDatas.instance;
        }

        /// Callback - Triger enter
        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            Utility.TimeManager.StartSlowMotion();
        }
    }

}