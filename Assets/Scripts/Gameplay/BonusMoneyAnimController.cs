using MyCustom;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class BonusMoneyAnimController : CustomMonoBehaviour
	{
		[SerializeField]
		private Text bonusMoneyText;

		[SerializeField]
		private float lifeTime;

		public void Init(int bonusMoney)
		{
			bonusMoneyText.text = $"+ {bonusMoney}";
			CustomInvoke(LateAnimationOpen, lifeTime);
		}

		private void LateAnimationOpen()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.Push(base.gameObject);
		}
	}
}
