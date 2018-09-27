using Scripting.GameManagers;
using UnityEngine;

namespace Scripting.Utility
{
    public class PlaySoundEvent : MonoBehaviour
    {
        /// Plays a sound
        public void PlaySound(string p_id)
        {
            Managers.instance.soundManager.PlaySound(p_id, gameObject);
        }
    }
}