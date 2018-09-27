using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Scripting.GameManagers
{
    internal class Managers : MonoBehaviour
    {
        /// Singleton
        static Managers m_instance = null;
        static internal Managers instance 
        {
            get
            {
                return m_instance;
            }
        }

        // FX Manager
        internal FXManager fxManager {get; private set;}

        // Player manager
        internal PlayerManager playerManager {get; private set;}

        // Sound manager
        internal SoundManager soundManager {get; private set;}
        
        // Post processing stack 
        internal PostProcessVolume processVolume { get; private set; }

        void Awake()
        {
            if (m_instance == null)
                m_instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            // Get fx manager
            fxManager = GetComponent<FXManager>();

            // get player manager
            playerManager = GetComponent<PlayerManager>();

            // get sound manager
            soundManager = GetComponent<SoundManager>();

            // get post processing stack
            processVolume = FindObjectOfType<PostProcessVolume>();
        }
    }
}