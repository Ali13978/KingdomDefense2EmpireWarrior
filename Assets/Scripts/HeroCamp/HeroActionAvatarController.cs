using MyCustom;

namespace HeroCamp
{
	public class HeroActionAvatarController : CustomMonoBehaviour
	{
		public int HeroID
		{
			get;
			set;
		}

		public void Init(int _heroID)
		{
			HeroID = _heroID;
		}

		public void Show()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
