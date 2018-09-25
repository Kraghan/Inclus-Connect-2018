using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripting.Menu
{
	 class OptionController : MonoBehaviour 
	 {
		 /// The option root
		 [SerializeField]
		 GameObject m_root = null;
		 
		/// Awake
		void Awake()
		{
			// Ensure root is set
			Debug.Assert(m_root != null, "OptionController.Awake(): Root cannot be null.");
		}

		 /// Callback - Play button pressed
		 public void OnBackPressed()
		 {
			 SetEnabled(false);
		 }

		/// Sets pause or disables it
		 internal void SetEnabled(bool p_value)
		 {
			 m_root.SetActive(p_value);
		 }
	}

}