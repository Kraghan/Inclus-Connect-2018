using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripting.FX
{
	class DisableEvent : MonoBehaviour 
	{
		/// Disable object
		public void Disable()
		{
			gameObject.SetActive(false);
		}
		
	}
}