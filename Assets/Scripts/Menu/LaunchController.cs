using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripting.Menu
{
	class LaunchController : MonoBehaviour 
	{
		/// The game scene
		[SerializeField]
		string m_next_scn = "scn_game";

		/// Callback - First controller assistance setting changed
		public void OnController1AssistanceChanged(bool p_value)
		{

		}

		/// Callback - Second controller assistance setting changed
		public void OnController2AssistanceChanged(bool p_value)
		{

		}

		/// Callback - Thrid controller assistance setting changed
		public void OnController3AssistanceChanged(bool p_value)
		{

		}

		/// Callback - Fourth controller assistance setting changed
		public void OnController4AssistanceChanged(bool p_value)
		{

		}

		/// Callback - Play pressed
		public void OnPlaypressed()
		{
			SceneManager.LoadScene(m_next_scn);
		}
	}
}