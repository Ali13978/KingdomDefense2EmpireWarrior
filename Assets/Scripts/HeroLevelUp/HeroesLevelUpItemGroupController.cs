using Data;
using DG.Tweening;
using Gameplay;
using MyCustom;
using System.Collections.Generic;
using UnityEngine;

namespace HeroLevelUp
{
	public class HeroesLevelUpItemGroupController : CustomMonoBehaviour
	{
		[SerializeField]
		private Transform listHeroItemsParent;

		private List<HeroLevelUpItem> listHeroLevelUpItem = new List<HeroLevelUpItem>();

		[SerializeField]
		private HeroLevelUpItem heroLevelUpItemPrefabs;

		private Sequence sqShow;

		public void InitData()
		{
			CustomInvoke(OpenWithEffect, 0f);
			CustomInvoke(DisplayExpBarAfterCalculating, 2f);
		}

		private void DisplayExpBarAfterCalculating()
		{
			GameplayManager.Instance.heroesManager.CalculateExp();
			foreach (int item in SingletonMonoBehaviour<GameData>.Instance.ListHeroesIdsSelected)
			{
				int currentExp = ReadWriteDataHero.Instance.GetCurrentExp(item);
				UnityEngine.Debug.Log("after EXP hero " + item + " = " + currentExp);
			}
			foreach (HeroLevelUpItem item2 in listHeroLevelUpItem)
			{
				item2.InitExpBarAfterCalculating();
			}
		}

		private void OpenWithEffect()
		{
			Open();
			SingletonMonoBehaviour<CameraController>.Instance.PinchZoomFov.MoveToOriginPos();
			foreach (int item in SingletonMonoBehaviour<GameData>.Instance.ListHeroesIdsSelected)
			{
				HeroLevelUpItem heroLevelUpItem = Object.Instantiate(heroLevelUpItemPrefabs);
				heroLevelUpItem.transform.parent = listHeroItemsParent;
				heroLevelUpItem.transform.localScale = Vector3.one;
				heroLevelUpItem.Init(item);
				listHeroLevelUpItem.Add(heroLevelUpItem);
			}
		}

		private void Open()
		{
			base.gameObject.SetActive(value: true);
		}

		private void Close()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
