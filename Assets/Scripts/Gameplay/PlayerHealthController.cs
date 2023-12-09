using DG.Tweening;
using MyCustom;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class PlayerHealthController : CustomMonoBehaviour
	{
		[SerializeField]
		private Text heathMessage;

		private int lastHealth = -1;

		private Vector3 scaleVector = new Vector3(1.2f, 1.2f, 1.2f);

		private Tween tween;

		public void SetHealthMessage()
		{
			int currentHealth = SingletonMonoBehaviour<GameData>.Instance.CurrentHealth;
			if (currentHealth != lastHealth)
			{
				heathMessage.text = currentHealth.ToString();
				AnimationChangeHealth();
				lastHealth = currentHealth;
			}
		}

		private void AnimationChangeHealth()
		{
			tween.Restart();
			tween = heathMessage.transform.DOScale(scaleVector, 0.2f).OnComplete(LateAnimation);
		}

		private void LateAnimation()
		{
			tween = heathMessage.transform.DOScale(Vector3.one, 0.1f);
		}
	}
}
