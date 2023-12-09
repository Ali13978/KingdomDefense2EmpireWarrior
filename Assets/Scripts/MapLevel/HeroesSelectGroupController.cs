using Data;
using Parameter;
using System.Collections.Generic;
using UnityEngine;

namespace MapLevel
{
	public class HeroesSelectGroupController : MonoBehaviour
	{
		[Space]
		[Header("Reference")]
		[SerializeField]
		private HeroesInputGroupController heroesInputGroupController;

		[Space]
		[Header("Init button")]
		[SerializeField]
		private HeroSelectButtonController heroSelectButtonPrefab;

		[SerializeField]
		private Transform listButtonParent;

		private List<HeroSelectButtonController> listHeroToSelect = new List<HeroSelectButtonController>();

		[Space]
		[Header("Set Content size by number of objects")]
		[SerializeField]
		private float cellSize;

		[SerializeField]
		private RectTransform content;

		public void InitListHeroToSelect()
		{
			List<int> listHeroID = HeroParameter.Instance.GetListHeroID();
			if (listHeroToSelect.Count == 0)
			{
				for (int i = 0; i < listHeroID.Count; i++)
				{
					HeroSelectButtonController heroSelectButtonController = UnityEngine.Object.Instantiate(heroSelectButtonPrefab);
					heroSelectButtonController.transform.SetParent(listButtonParent);
					heroSelectButtonController.transform.localScale = Vector3.one;
					heroSelectButtonController.Init(listHeroID[i], heroesInputGroupController);
					listHeroToSelect.Add(heroSelectButtonController);
				}
			}
			UpdateButtonsStatus();
			SetContentSize();
			SetDefaultValue();
			InitSavedValue();
		}

		public void UpdateButtonsStatus()
		{
			foreach (HeroSelectButtonController item in listHeroToSelect)
			{
				item.UpdateStatus();
			}
		}

		private void SetDefaultValue()
		{
			foreach (HeroSelectButtonController item in listHeroToSelect)
			{
				item.SetDefaultValue();
			}
		}

		private void InitSavedValue()
		{
			int num = 3;
			int[] listHeroIDSaved = ReadWriteDataHeroPrepare.Instance.GetListHeroIDSaved();
			for (int i = 0; i < num; i++)
			{
				foreach (HeroSelectButtonController item in listHeroToSelect)
				{
					if (item.HeroID == listHeroIDSaved[i])
					{
						item.OnClick();
					}
				}
			}
		}

		private void SetContentSize()
		{
			Vector2 sizeDelta = content.sizeDelta;
			sizeDelta.Set(sizeDelta.x, cellSize * (float)listHeroToSelect.Count);
			content.sizeDelta = sizeDelta;
		}

		public void UnChooseHero(int heroID)
		{
			foreach (HeroSelectButtonController item in listHeroToSelect)
			{
				if (item.HeroID == heroID)
				{
					item.SetView_NonSelect();
				}
			}
		}
	}
}
