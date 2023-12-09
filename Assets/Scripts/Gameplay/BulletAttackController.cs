using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
	public class BulletAttackController : MonoBehaviour
	{
		[SerializeField]
		private UnityEvent startAttack = new UnityEvent();

		[SerializeField]
		private UnityEvent animationStopAttack = new UnityEvent();

		[SerializeField]
		private UnityEvent stopAttack = new UnityEvent();

		public void StartAttack()
		{
			startAttack.Invoke();
		}

		public void StopAttack()
		{
			stopAttack.Invoke();
		}

		public void AnimationStopAttack()
		{
			animationStopAttack.Invoke();
		}
	}
}
