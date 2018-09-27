using Scripting.Actors;
using Scripting.GameManagers;
using UnityEngine;


namespace Scripting.QTE
{
    internal class QTEDefend : QuickTimeEvent
    {
        /// Jump destination
        [SerializeField]
        GameObject m_target = null;

        /// Callback - Player exited
        protected override void OnPlayerExited()
        {
            m_target.SetActive(true);
            Managers.instance.playerManager.player.Defend(m_target);
        }
        
        /// Player succeeded inputs
        protected override void OnQTESucceeded()
        {
            Managers.instance.playerManager.player.isRunning = true;
        }
    }
}