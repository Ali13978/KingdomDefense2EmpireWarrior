using MyCustom;
using Parameter;
using System.Collections.Generic;
using UnityEngine;

namespace HeroCamp
{
	public class PetActionAvatarGroupController : CustomMonoBehaviour
	{
		[SerializeField]
		private Transform petActionAvatarHolder;

		private List<HeroActionAvatarController> listPetActionAvatar = new List<HeroActionAvatarController>();

		private void InitListPetActionAvatars()
		{
			if (listPetActionAvatar.Count < 1)
			{
				List<int> listPetID = HeroParameter.Instance.GetListPetID();
				for (int i = 0; i < listPetID.Count; i++)
				{
					HeroActionAvatarController heroActionAvatarController = Object.Instantiate(Resources.Load<HeroActionAvatarController>($"HeroCamp/MiniActionAvatars/action_avatar_hero_{listPetID[i]}"));
					heroActionAvatarController.transform.SetParent(petActionAvatarHolder);
					heroActionAvatarController.transform.localScale = Vector3.one;
					heroActionAvatarController.transform.localPosition = Vector3.zero;
					heroActionAvatarController.transform.localRotation = Quaternion.Euler(Vector3.zero);
					heroActionAvatarController.Init(listPetID[i]);
					listPetActionAvatar.Add(heroActionAvatarController);
					heroActionAvatarController.Hide();
				}
			}
		}

		public void ShowSelectedPetActionAvatar(int petID)
		{
			InitListPetActionAvatars();
			HideAll();
			DisplayPetActionAvatar(petID);
		}

		public void HideAll()
		{
			foreach (HeroActionAvatarController item in listPetActionAvatar)
			{
				item.Hide();
			}
		}

		private void DisplayPetActionAvatar(int petID)
		{
			for (int i = 0; i < listPetActionAvatar.Count; i++)
			{
				if (listPetActionAvatar[i].HeroID == petID)
				{
					listPetActionAvatar[i].Show();
				}
			}
		}
	}
}
