using Scripting.Actors;
using Scripting.GameManagers;
using UnityEngine;


namespace Scripting.QTE
{
    internal class QTEDoor : QuickTimeEvent
    {
        /// Jump destination
        [SerializeField]
        Door m_door = null;
        
        /// Callback - Player exited
        protected override void OnPlayerExited()
        {
            Managers.instance.playerManager.player.EnterForm(m_door.form);
        }
        
        /// Player succeeded inputs
        protected override void OnQTESucceeded()
        {
            base.OnQTESucceeded();
            Managers.instance.playerManager.player.form = m_door.form;
            // Managers.instance.playerManager.player.EnterForm(m_door.form);
        }

        protected override void UpdateQTERequierement()
        {
            QTEDone qte = m_QTENeeded;
            if (qte.p_done)
                return;

            switch (qte.type)
            {
                case QTEType.ACCELERO:
                    if (s_inputs.acceleroOn)
                        qte.p_done = true;
                    break;

                case QTEType.BUTTON:
                    if (s_inputs.buttonOn)
                        qte.p_done = true;
                    break;

                case QTEType.LIGHT:
                    if ((s_inputs.lightOn == true && m_door.form == EPlayerForm.Default) || (s_inputs.lightOn == false && m_door.form == EPlayerForm.Ghost))
                        qte.p_done = true;
                    break;

                case QTEType.MICRO:
                    if (s_inputs.microOn)
                        qte.p_done = true;
                    break;
            }
        }
    }
}