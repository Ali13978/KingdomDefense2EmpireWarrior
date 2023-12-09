using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class ObjectActivationSwitcher : MonoBehaviour
	{
		[SerializeField]
		private GameObject targetObject;

		public void Switch()
		{
			targetObject.SetActive(!targetObject.activeSelf);
		}
	}
}
