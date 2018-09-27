using System.Collections;
using System.Collections.Generic;
using Scripting.Actors;
using Scripting.GameManagers;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


namespace Scripting.QTE
{
    public enum QTEType
    {
        NONE = -1,
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
        protected QTEDone m_QTENeeded = null;

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

        /// Color of the QTE
        public Color color { get; protected set; }

        /// Area renderer of the QTE
        public SpriteRenderer areaSprite;

        [Header("Tutorial")]
        public bool isTutorial = false;

        public PostProcessVolume processVolume;
        private Vignette vignette; 


        /// Start
        protected virtual void Start()
        {
            // Get collider
            m_collider = GetComponent<BoxCollider2D>();

            enabled = false;

            areaSprite.color = color;

            FogAndFirefliesColorChanger[] colorChangers = areaSprite.GetComponentsInChildren<FogAndFirefliesColorChanger>();
            foreach (FogAndFirefliesColorChanger colorChanger in colorChangers)
                colorChanger.ChangeColor(color);

            if (!processVolume.profile.TryGetSettings<Vignette>(out vignette))
                Debug.LogError("Vignette post process not found");
        }
        
        /// Update
        void FixedUpdate()
        {
            // Update inputs
            UpdateQTERequierement();

            /// Check if succeedded
            // If QTE is triggered
            if(m_QTENeeded.p_done == true)
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
                    Managers.instance.playerManager.GetPlayer(m_QTENeeded.type).skill += Mathf.Lerp(m_succesPercentageToAdd, 0, position * 2);
                }
                // Late Trigger
                else
                {
                    Managers.instance.playerManager.GetPlayer(m_QTENeeded.type).skill -= Mathf.Lerp(0, m_failurePercentageToRemove, position * 2 - 1);
                }
                Mathf.Clamp(Managers.instance.playerManager.GetPlayer(m_QTENeeded.type).skill, 0.1f, 1);

                /// Activate and setup trail
                StartCoroutine(TrailControl(1 - position));

                OnQTESucceeded();
            }
            // If QTE is not triggered
            else
            {
                // Update slowmo
                Utility.TimeManager.StartSlowMotion(Mathf.Lerp(1, Managers.instance.playerManager.GetPlayer(m_QTENeeded.type).GetSlowMotionMin(), GetPositionInCollider() * 10));

                if (isTutorial && GetPositionInCollider() >= 0.5f)
                {
                    Utility.TimeManager.StartSlowMotion(0.001f);

                    vignette.enabled.Override(true);
                }
            }
        }

        private void Update()
        {
            if (isTutorial && GetPositionInCollider() >= 0.5f)
            {
                float value = Mathf.Lerp(0.5f, 0.7f, Mathf.PingPong(Time.realtimeSinceStartup, 1));
                print(value);

                vignette.intensity.Override(value);
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

        virtual protected void UpdateQTERequierement()
        {
            QTEDone qte = m_QTENeeded;

            if (qte.p_done)
                return;

            switch (qte.type)
            {
                case QTEType.ACCELERO:
                    if (s_inputs.acceleroOn)
                        qte.p_done = true;

                    if (s_inputs.buttonJustOn)
                        StartCoroutine(Managers.instance.playerManager.player.DeactivateJumpArtifact());
                    if (s_inputs.microJustOn)
                        StartCoroutine(Managers.instance.playerManager.player.DeactivateShieldArtifact());
                    if (s_inputs.lightJustOn)
                        StartCoroutine(Managers.instance.playerManager.player.DeactivateGhostArtifact());

                    break;

                case QTEType.BUTTON:
                    if (s_inputs.buttonOn)
                        qte.p_done = true;

                    if (s_inputs.acceleroJustOn)
                        StartCoroutine(Managers.instance.playerManager.player.DeactivateAttackArtifact());
                    if (s_inputs.microJustOn)
                        StartCoroutine(Managers.instance.playerManager.player.DeactivateShieldArtifact());
                    if (s_inputs.lightJustOn)
                        StartCoroutine(Managers.instance.playerManager.player.DeactivateGhostArtifact());
                    break;

                case QTEType.LIGHT:
                    if (s_inputs.lightOn)
                        qte.p_done = true;

                    if (s_inputs.buttonJustOn)
                        StartCoroutine(Managers.instance.playerManager.player.DeactivateJumpArtifact());
                    if (s_inputs.microJustOn)
                        StartCoroutine(Managers.instance.playerManager.player.DeactivateShieldArtifact());
                    if (s_inputs.acceleroJustOn)
                        StartCoroutine(Managers.instance.playerManager.player.DeactivateAttackArtifact());
                    break;

                case QTEType.MICRO:
                    if (s_inputs.microOn)
                        qte.p_done = true;

                    if (s_inputs.buttonJustOn)
                        StartCoroutine(Managers.instance.playerManager.player.DeactivateJumpArtifact());
                    if (s_inputs.acceleroJustOn)
                        StartCoroutine(Managers.instance.playerManager.player.DeactivateAttackArtifact());
                    if (s_inputs.lightJustOn)
                        StartCoroutine(Managers.instance.playerManager.player.DeactivateGhostArtifact());
                    break;
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
        protected virtual void OnPlayerEntered()
        {
            Managers.instance.playerManager.player.enableInputs = false;
            // Managers.instance.playerManager.player.QTESucceeded = false;
        }

        /// Player exited area
        protected abstract void OnPlayerExited();

        /// Player succeeded inputs
        IEnumerator TrailControl(float time)
        {
            GameObject trail = Managers.instance.playerManager.player.trail;
            TrailRenderer[] trails = trail.GetComponentsInChildren<TrailRenderer>();
            trail.SetActive(true);
            /// Setup
            foreach (TrailRenderer t in trails)
            {
                t.material.color = color;
                t.material.SetColor("_EmissionColor", color * Mathf.LinearToGammaSpace(10));
                t.emitting = true;
                t.time = time;
            }

            // Make disapear trail
            for (float timeElapsed = 0; timeElapsed < time; timeElapsed += Time.deltaTime)
            {
                yield return new WaitForEndOfFrame();
            }

            for (float timeElapsed = 0; timeElapsed < time; timeElapsed += Time.deltaTime)
            {
                // Not sure if useful 
                foreach (TrailRenderer t in trails)
                {
                    t.time -= Time.deltaTime;
                }
                yield return new WaitForEndOfFrame();
            }

            foreach (TrailRenderer t in trails)
            {
                t.emitting = false;
            }
            trail.SetActive(false);

        }
        
        /// QTE Succeeded
        protected virtual void OnQTESucceeded()
        {
            // Which QTE succeeded
            Managers.instance.playerManager.player.QTESucceeded = m_QTENeeded.type;

            // Progress
            Managers.instance.playerManager.player.QTEProgress = GetPositionInCollider();

            // FX
            Managers.instance.fxManager.SpawnFX(EFXType.Reward, Managers.instance.playerManager.player.transform.position);

            // Sound FX
            Managers.instance.soundManager.PlaySound("Play_Feedback_Success", Managers.instance.playerManager.player.gameObject);

            // Make it run
            Managers.instance.playerManager.player.isRunning = true;

            if (isTutorial)
                vignette.enabled.Override(false);
        }
    }

}