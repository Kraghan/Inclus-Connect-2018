using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripting.Menu
{
	class LaunchController : MonoBehaviour 
	{
		/// The game scene
		[SerializeField]
		string m_next_scn = "scn_game";

		/// Callback - First controller assistance setting changed
		public void OnController1AssistanceChanged(Toggle p_toggle)
		{
			PlayerDatas.instance.datas[0].isAssisted = p_toggle.isOn;
			Debug.LogFormat("LaunchController.OnAssistanceChanged(): Player 1 assistance set to {0}.", p_toggle.isOn);
		}

		/// Callback - Second controller assistance setting changed
		public void OnController2AssistanceChanged(Toggle p_toggle)
		{
			PlayerDatas.instance.datas[1].isAssisted = p_toggle.isOn;
			Debug.LogFormat("LaunchController.OnAssistanceChanged(): Player 2 assistance set to {0}.", p_toggle.isOn);
		}

		/// Callback - Thrid controller assistance setting changed
		public void OnController3AssistanceChanged(Toggle p_toggle)
		{
			PlayerDatas.instance.datas[2].isAssisted = p_toggle.isOn;
			Debug.LogFormat("LaunchController.OnAssistanceChanged(): Player 3 assistance set to {0}.", p_toggle.isOn);
		}

		/// Callback - Fourth controller assistance setting changed
		public void OnController4AssistanceChanged(Toggle p_toggle)
		{
			PlayerDatas.instance.datas[3].isAssisted = p_toggle.isOn;
			Debug.LogFormat("LaunchController.OnAssistanceChanged(): Player 4 assistance set to {0}.", p_toggle.isOn);
		}

		/// Callback - Play pressed
		public void OnPlaypressed()
		{
			SceneManager.LoadScene(m_next_scn);
		}
	}
}