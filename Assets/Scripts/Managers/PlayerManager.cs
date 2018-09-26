using System.Collections;
using System.Collections.Generic;
using Scripting.Actors;
using Scripting.QTE;
using UnityEngine;

namespace Scripting.GameManagers
{

    /// The data container
    internal class PlayerData
    {
        /// Phe player controller
        [SerializeField]
        internal QTEType playerController       = QTEType.MICRO;
        
        /// Is the player enabled
        [SerializeField]
        internal bool    enabled                = false;

        /// Is the player assisted
        [SerializeField]
        internal bool    isAssisted             = false;

        /// Player skill 
        [SerializeField]
        [Range(0,1)]
        internal float   skill                  = 0.5f;

        /// Constructor
        internal PlayerData(QTEType p_type, bool p_enabled, bool p_assisted)
        {
            playerController = p_type;
            enabled = p_enabled;
            isAssisted = p_assisted;

            if (isAssisted == false)
                skill = 0.75f;
            else
                skill = 0.25f;
        }

        internal float GetSlowMotionMin()
        {
            return Mathf.Lerp(0.1f, 1, skill);
        }
    }

    /// The container class
    internal class PlayerManager : MonoBehaviour
    {
        /// Player controller
        [SerializeField]
        private PlayerController m_player;
        internal PlayerController player {get {return m_player;} set {m_player = value; inputs = m_player.GetComponent<ArduInput>();}}

        internal ArduInput inputs {get; private set;}

        /// The datas for each players
        internal PlayerData[] datas = new PlayerData[4];

        /// Awake
        void Awake()
        {
            // Initialize datas
            for(int i = 0 ; i < 4 ; ++i)
                datas[i] =  new PlayerData((QTEType)i, true, true);
        }


       /// Get datas from the controller
        public PlayerData GetPlayer(QTEType type)
        {

            for (uint i = 0; i < datas.Length; ++i)
            {
                if (datas[i].playerController == type)
                {
                    return datas[i];
                }
            }
            return null;
        }
    }
}