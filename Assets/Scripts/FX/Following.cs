using UnityEngine;

namespace Scripting.FX
{

	public class Following : MonoBehaviour 
	{
		/// the object to follow
		internal GameObject objectToFollow = null;

		/// Late update
		void LateUpdate()
		{
			gameObject.transform.position = objectToFollow.transform.position;
		}
	}
}