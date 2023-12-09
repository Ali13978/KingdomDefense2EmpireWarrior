using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
	public class FlyingCoinController : MonoBehaviour
	{
		[SerializeField]
		private float timeToMove;

		public void Init(Vector3 target)
		{
			base.transform.DOMove(target, timeToMove).SetEase(Ease.Linear);
		}
	}
}
