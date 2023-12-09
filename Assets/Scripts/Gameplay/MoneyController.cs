using DG.Tweening;
using MyCustom;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class MoneyController : CustomMonoBehaviour
	{
		[SerializeField]
		private Text moneyMessage;

		private int lastMoney = -1;

		private Vector3 scaleVector = new Vector3(1.2f, 1.2f, 1.2f);

		private Tween tween;

		public void SetMoneyMessage()
		{
			int money = SingletonMonoBehaviour<GameData>.Instance.Money;
			if (money != lastMoney)
			{
				moneyMessage.text = money.ToString();
				AnimationAddMoney();
				lastMoney = money;
			}
		}

		private void AnimationAddMoney()
		{
			tween.Restart();
			tween = moneyMessage.transform.DOScale(scaleVector, 0.2f).OnComplete(LateAnimation);
		}

		private void LateAnimation()
		{
			tween = moneyMessage.transform.DOScale(Vector3.one, 0.1f);
		}
	}
}
