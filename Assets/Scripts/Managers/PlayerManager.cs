﻿using System.Collections;
using System.Collections.Generic;
using Scripting.Actors;
using UnityEngine;

namespace Scripting.GameManagers
{

    /// The data container
    internal class PlayerData
    {
        /// Phe player controller
        [SerializeField]
        internal QTEType playerController = QTEType.MICRO;
        
        /// Is the player enabled
        [SerializeField]
        internal bool    enabled                = false;

        /// Is the player assisted
        [SerializeField]
        internal bool    isAssisted            = false;

        /// Constructor
        internal PlayerData(QTEType p_type, bool p_enabled, bool p_assisted)
        {
            playerController = p_type;
            enabled = p_enabled;
            isAssisted = p_assisted;
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


       /// Unused (DDA removed) 
        // public PlayerData GetWeakestPlayer(QTEType[] types)
        // {
        //     uint weakestIndex = 0;

        //     for (uint i = 0; i < m_datas.Length; ++i)
        //     {
        //         foreach (QTEType type in types)
        //         {
        //             if (m_datas[i].playerController == type
        //                 && m_datas[i].skill < m_datas[weakestIndex].skill)
        //             {
        //                 weakestIndex = i;
        //                 break;
        //             }
        //         }
        //     }

        //     return m_datas[weakestIndex];
        // }
    }
}