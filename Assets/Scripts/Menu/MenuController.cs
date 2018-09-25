using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripting.Menu
{
	 class MenuController : MonoBehaviour 
	 {
		 /// The game scene
		 [SerializeField]
		 string m_next_scene = "scn_game";

		/// The option panel
		[SerializeField]
		 OptionController m_option_controller = null;

		/// Awake
		void Awake()
		{
			// Ensure linking
			Debug.Assert(m_option_controller != null, "MenuController.Awake(): Option controller cannot be null.");
		}

		 /// Callback - Play button pressed
		 public void OnPlayPressed()
		 {
			 // Load next scene
			 SceneManager.LoadScene(m_next_scene);
		 }

		/// Callback - Play button pressed
		 public void OnScorePressed()
		 {
			// TBD
		 }

		 /// Callback - Play button pressed
		 public void OnOptionsPressed()
		 {
			 m_option_controller.SetEnabled(true);
		 }

		 /// Callback - Play button pressed
		 public void OnExitPressed()
		 {
			 Application.Quit();
		 }
	}

}