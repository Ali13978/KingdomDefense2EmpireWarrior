using MyCustom;
using UnityEngine;

namespace Gameplay
{
	public class GemController : CustomMonoBehaviour
	{
		[SerializeField]
		private float lifeTime;

		[SerializeField]
		private TextMesh textMesh;

		public void Init(int gemAmount)
		{
			textMesh.text = "+ " + gemAmount.ToString();
			CustomInvoke(LateAnimationOpen, lifeTime);
		}

		private void LateAnimationOpen()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.Push(base.gameObject);
		}
	}
}
