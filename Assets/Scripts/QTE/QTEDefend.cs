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

        protected override void Start()
        {
            color = new Color(87 / 255f, 180 / 255f, 169 / 255f);
            base.Start();
        }

        /// Callback - Player exited
        protected override void OnPlayerExited()
        {
            m_target.SetActive(true);
            Managers.instance.playerManager.player.Defend(m_target);
        }
    }
}