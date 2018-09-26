using Scripting.Actors;
using Scripting.GameManagers;
using UnityEngine;


namespace Scripting.QTE
{
    internal class QTEAttack : QuickTimeEvent
    {
        /// Jump destination
        [SerializeField]
        GameObject m_target = null;

        /// Callback - Player entered
        protected override void OnPlayerEntered()
        {
        }

        /// Callback - Player exited
        protected override void OnPlayerExited()
        {
            Managers.instance.playerManager.player.Attack(m_target);
        }

        /// Player succeeded inputs
        protected override void OnQTESucceeded()
        {
            Managers.instance.playerManager.player.isRunning = true;
        }
    }
}