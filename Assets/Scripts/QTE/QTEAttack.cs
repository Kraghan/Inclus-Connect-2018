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

        protected override void Start()
        {
            base.Start();
            color = new Color(196 / 255f, 0, 74 / 255f);
        }

        /// Callback - Player exited
        protected override void OnPlayerExited()
        {
            Managers.instance.playerManager.player.Attack(m_target);
            Managers.instance.playerManager.player.QTEExit();
        }
    }
}