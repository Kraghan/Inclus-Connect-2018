using UnityEngine;

namespace Scripting.FX
{

	public class Following : MonoBehaviour 
	{
		/// the object to follow
		internal GameObject objectToFollow = null;

		/// Offset following
		internal Vector3 offset = Vector3.zero;

		/// Late update
		void LateUpdate()
		{
			gameObject.transform.position = objectToFollow.transform.position + offset;
		}
	}
}