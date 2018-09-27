using System.Collections;
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

        protected override void Start()
        {
            color = new Color(112 / 255f, 0 / 255f, 193 / 255f);
            base.Start();
        }

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
            StartCoroutine(Managers.instance.playerManager.player.ActivateGhostArtifact());
            StartCoroutine("FadeOut");
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

        IEnumerator FadeOut()
        {
            float count = 0f;
            const float kFadeDuration = 0.5f;

            while(count < 0.5f)
            {
                count += Time.deltaTime;
                m_door.magma.color = new Color(1,1,1, Mathf.Lerp(1, 0, count / kFadeDuration));
                yield return new WaitForEndOfFrame();
            }
        }
    }
}