using Scripting.Actors;
using Scripting.GameManagers;
using UnityEngine;


namespace Scripting.QTE
{
    internal class QTEDoor : QuickTimeEvent
    {
        /// Jump destination
        [SerializeField]
        GameObject m_door = null;

        /// Callback - Player entered
        protected override void OnPlayerEntered()
        {
        }

        /// Callback - Player exited
        protected override void OnPlayerExited()
        {
            Managers.instance.playerManager.player.EnterForm(EPlayerForm.Ghost);
        }
        
        /// Player succeeded inputs
        protected override void OnQTESucceeded()
        {
            Managers.instance.playerManager.player.isRunning = true;
            Managers.instance.playerManager.player.UpdateForm();
        }
    }
}