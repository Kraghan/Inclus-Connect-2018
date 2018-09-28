using Scripting.Actors;
using Scripting.GameManagers;
using UnityEngine;


namespace Scripting.QTE
{
    internal class QTEJump : QuickTimeEvent
    {
        /// Jump destination
        public GameObject m_destination = null;

        /// Jump duration
        public float m_duration = 1f;

        protected override void Start()
        {
            color = new Color(66 / 255f, 199 / 255f, 75 / 255f);
            base.Start();
        }
        
        /// Callback - Player exited
        protected override void OnPlayerExited()
        {
            Managers.instance.playerManager.player.isRunning = false;

            // Orders to jump
            Debug.LogFormat("Destination is {0}", m_destination);
            if (m_destination != null)
                Managers.instance.playerManager.player.JumpTo(m_destination.transform.position, m_duration);
            else
                Managers.instance.playerManager.player.SetNextState((int)EPlayerStates.Running);
            Debug.Log("Player exited !");

        
        }

        protected override void OnQTESucceeded()
        {
            base.OnQTESucceeded();
            StartCoroutine(Managers.instance.playerManager.player.ActivateJumpArtifact());
        }
    }
}