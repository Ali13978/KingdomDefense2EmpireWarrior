using MyCustom;
using UnityEngine;

namespace Notify
{
	public class GeneralNotify : CustomMonoBehaviour
	{
		[SerializeField]
		private GameObject notifyUnit;

		public void TryShowNotify(bool isShow)
		{
			notifyUnit.SetActive(isShow);
		}
	}
}
