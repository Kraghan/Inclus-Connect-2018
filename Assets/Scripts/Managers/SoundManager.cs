using UnityEngine;

namespace Scripting.GameManagers
{
	public class SoundManager : MonoBehaviour
	{
		/// <summary>
		/// Sound Node
		/// </summary>
		[SerializeField]
		GameObject _SoundNode = null;
		internal GameObject soundNode {get {return _SoundNode;}}

        /// <summary>
        /// The Bank to load for the whole game 
        /// </summary>
        [SerializeField]
        string _BankName = "Main";

        /// <summary>
        /// The ambient sound ID
        /// </summary>
        internal uint musicID{ get; private set;}
        string m_currentMusicName = null;

		/// <summary>
		/// Start
		/// </summary>
		void Start()
		{
			// Load a bank synchronously, using its ID.
			// This bank may contain only the definition of the events you wish to use.
			uint returnedBankID;
			AKRESULT eResult = AkSoundEngine.LoadBank(
				_BankName,         				// Identifier of the bank to load.
				AkSoundEngine.AK_DEFAULT_POOL_ID,         	// Memory pool ID (data is written in default sound engine memory pool when AK_DEFAULT_POOL_ID is passed).
				out returnedBankID              				// Returned bank ID.
				);

            Debug.Assert(eResult == AKRESULT.AK_Success);

            // Play ambiance
            PlaySound("Play_AMB", _SoundNode);

            PlayMusic("Play_MUSICGLOBAL");
        }

		/// <summary>
		/// Destroy
		/// </summary>
		void Destroy()
		{
			if (musicID != 0)
				StopSound(musicID);

			AkSoundEngine.ClearBanks();
        }

        /// <summary>
        /// Plays a music
        /// </summary>
        internal void PlayMusic(string p_id)
		{
            Debug.LogFormat("SoundManager.playMusic(): Playing the music {0}.", p_id);

            if (m_currentMusicName != p_id)
            {
                m_currentMusicName = p_id;
            }
            else
            {
                return;
            }

            if (musicID != 0)
            {
                StopSound(musicID);
            }


			musicID = PlaySound(p_id, _SoundNode);
		}


		/// <summary>
		/// Plays a sound an log it
		/// </summary>
		internal uint PlaySound(string p_id, GameObject p_origin)
		{
			Debug.LogFormat("SoundManager.playSound(): Playing the sound {0} form object {1}.", p_id, p_origin.name);

			return AkSoundEngine.PostEvent(p_id, p_origin);
		}

		/// <summary>
		/// Stop playing a sound
		/// </summary>
		internal void StopSound(uint p_id, int p_duration_ms = 500)
		{
			AkSoundEngine.StopPlayingID(p_id, p_duration_ms, AkCurveInterpolation.AkCurveInterpolation_Linear);
		}

		/// <summary>
		/// Sets RTPC value
		/// </summary>
		internal void SetRTPCvalue(string p_id, float p_value, GameObject p_origin)
		{
			AkSoundEngine.SetRTPCValue(p_id, p_value, p_origin, 0, AkCurveInterpolation.AkCurveInterpolation_Exp1, false);
		}

        /// <summary>
        /// Sets state
        /// </summary>
        internal void SetState(string p_id, string p_value)
        {
            AkSoundEngine.SetState(p_id, p_value);
        }

        /// <summary>
        /// Callback - application focus lost
        /// </summary>
        void OnApplicationFocus(bool p_value)
        {
            if (p_value == false)
            {
                AkSoundEngine.WakeupFromSuspend();
                AkSoundEngine.RenderAudio();
            }
        }
    }
}