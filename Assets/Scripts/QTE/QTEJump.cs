using Scripting.Actors;
using Scripting.GameManagers;
using UnityEngine;


namespace Scripting.QTE
{
    internal class QTEJump : QuickTimeEvent
    {
        /// Jump destination
        [SerializeField]
        GameObject m_destination = null;

        /// Jump duration
        [SerializeField]
        float m_duration = 1f;

        /// Callback - Player entered
        protected override void OnPlayerEntered()
        {
        }

        /// Callback - Player exited
        protected override void OnPlayerExited()
        {
            Managers.instance.playerManager.player.JumpTo(m_destination.transform.position, m_duration);
        }
    }
}