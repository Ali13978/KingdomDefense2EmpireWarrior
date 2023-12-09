using MyCustom;
using Parameter;
using System.Collections.Generic;
using UnityEngine;

namespace HeroCamp
{
	public class HeroActionAvatarGroupController : CustomMonoBehaviour
	{
		[SerializeField]
		private Transform heroActionAvatarHolder;

		private List<HeroActionAvatarController> listHeroActionAvatar = new List<HeroActionAvatarController>();

		private void Awake()
		{
			InitListHeroActionAvatars();
		}

		private void InitListHeroActionAvatars()
		{
			if (listHeroActionAvatar.Count < 1)
			{
				List<int> listHeroID = HeroParameter.Instance.GetListHeroID();
				for (int i = 0; i < listHeroID.Count; i++)
				{
					HeroActionAvatarController heroActionAvatarController = Object.Instantiate(Resources.Load<HeroActionAvatarController>($"HeroCamp/MiniActionAvatars/action_avatar_hero_{listHeroID[i]}"));
					heroActionAvatarController.transform.SetParent(heroActionAvatarHolder);
					heroActionAvatarController.transform.localScale = Vector3.one;
					heroActionAvatarController.transform.localPosition = Vector3.zero;
					heroActionAvatarController.Init(listHeroID[i]);
					listHeroActionAvatar.Add(heroActionAvatarController);
					heroActionAvatarController.Hide();
				}
			}
		}

		public void ShowSelectedHeroActionAvatar(int currentHeroID)
		{
			HideAll();
			DisplayHeroActionAvatar(currentHeroID);
		}

		private void HideAll()
		{
			foreach (HeroActionAvatarController item in listHeroActionAvatar)
			{
				item.Hide();
			}
		}

		private void DisplayHeroActionAvatar(int currentHeroID)
		{
			for (int i = 0; i < listHeroActionAvatar.Count; i++)
			{
				if (listHeroActionAvatar[i].HeroID == currentHeroID)
				{
					listHeroActionAvatar[i].Show();
				}
			}
		}
	}
}
