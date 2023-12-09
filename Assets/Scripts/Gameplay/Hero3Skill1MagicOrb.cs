using MyCustom;

namespace Gameplay
{
	public class Hero3Skill1MagicOrb : CustomMonoBehaviour
	{
		public void Init(float lifeTime)
		{
			Show();
			CustomInvoke(Hide, lifeTime);
		}

		private void Show()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
