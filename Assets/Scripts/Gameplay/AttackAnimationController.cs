using MyCustom;
using UnityEngine;

namespace Gameplay
{
	public class AttackAnimationController : CustomMonoBehaviour
	{
		public GameObject source;

		public GameObject target;

		public virtual void Init(GameObject target, float lifeTime)
		{
			this.target = target;
			CustomInvoke(OnReturnPool, lifeTime);
		}

		public virtual void Run()
		{
			base.gameObject.SetActive(value: true);
		}

		private void StopImmediately()
		{
			base.gameObject.SetActive(value: false);
		}

		public void OnReturnPool()
		{
			StopImmediately();
		}
	}
}
