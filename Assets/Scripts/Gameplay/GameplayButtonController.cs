using UnityEngine;

namespace Gameplay
{
	public class GameplayButtonController : MonoBehaviour
	{
		protected enum ButtonStatus
		{
			Available,
			Confirm
		}

		public virtual void OnClick()
		{
		}

		public virtual void OnMouseDown()
		{
		}
	}
}
