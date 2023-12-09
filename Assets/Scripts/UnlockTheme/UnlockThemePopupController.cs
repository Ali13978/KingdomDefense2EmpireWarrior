using Data;
using Gameplay;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WorldMap;

namespace UnlockTheme
{
	public class UnlockThemePopupController : GameplayPopupController
	{
		[Space]
		[Header("Image material")]
		[SerializeField]
		private Button unlockButton;

		[SerializeField]
		private Material material;

		[Space]
		[SerializeField]
		private UnlockConditionGroupController unlockConditionGroupController;

		private int[] listConditionsValue;

		private int currentThemeIDToUnlock;

		public void Init(int themeID)
		{
			currentThemeIDToUnlock = themeID;
			OpenWithScaleAnimation();
			unlockConditionGroupController.HideAll();
			InitAllUnlockCondition(themeID);
			InitUnlockButton();
		}

		private void InitUnlockButton()
		{
			if (IsAllConditionPass())
			{
				unlockButton.enabled = true;
				material.SetFloat("_EffectAmount", 0f);
			}
			else
			{
				unlockButton.enabled = false;
				material.SetFloat("_EffectAmount", 1f);
			}
		}

		private void InitAllUnlockCondition(int themeID)
		{
			listConditionsValue = new int[6];
			Dictionary<int, string> listCondition = ReadWriteDataTheme.Instance.GetListCondition(themeID);
			foreach (KeyValuePair<int, string> item in listCondition)
			{
				switch (item.Key)
				{
				case 0:
				{
					UnityEngine.Debug.Log("dieu kien: vuot qua map " + item.Value);
					int num5 = int.Parse(item.Value.Replace("m_", string.Empty));
					if (ReadWriteDataMap.Instance.GetMapIDUnlocked() > num5)
					{
						listConditionsValue[0] = 2;
						unlockConditionGroupController.InitConditionContent(0, 0, themeID, isPassedCondition: true);
					}
					else
					{
						listConditionsValue[0] = 1;
						unlockConditionGroupController.InitConditionContent(0, 0, themeID, isPassedCondition: false);
					}
					break;
				}
				case 1:
				{
					UnityEngine.Debug.Log("dieu kien: dat so sao " + item.Value);
					int num = int.Parse(item.Value.Replace("s_", string.Empty));
					if (ReadWriteDataPlayerCurrency.Instance.GetCurrentStar() >= num)
					{
						listConditionsValue[1] = 2;
						unlockConditionGroupController.InitConditionContent(1, 1, themeID, isPassedCondition: true);
					}
					else
					{
						listConditionsValue[1] = 1;
						unlockConditionGroupController.InitConditionContent(1, 1, themeID, isPassedCondition: false);
					}
					break;
				}
				case 2:
				{
					UnityEngine.Debug.Log("dieu kien: dat du so luong hero " + item.Value);
					int num3 = int.Parse(item.Value.Replace("ha_", string.Empty));
					if (ReadWriteDataHero.Instance.GetHeroOwnedAmount() >= num3)
					{
						listConditionsValue[2] = 2;
						unlockConditionGroupController.InitConditionContent(2, 2, themeID, isPassedCondition: true);
					}
					else
					{
						listConditionsValue[2] = 1;
						unlockConditionGroupController.InitConditionContent(2, 2, themeID, isPassedCondition: false);
					}
					break;
				}
				case 3:
				{
					UnityEngine.Debug.Log("dieu kien: so huu hero thu nhat dat level " + item.Value);
					string[] array = item.Value.Split('_');
					int heroID2 = int.Parse(array[0]);
					int num2 = int.Parse(array[1]);
					if (ReadWriteDataHero.Instance.IsHeroOwned(heroID2) && ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID2) >= num2)
					{
						listConditionsValue[3] = 2;
						unlockConditionGroupController.InitConditionContent(2, 3, themeID, isPassedCondition: true);
					}
					else
					{
						listConditionsValue[3] = 1;
						unlockConditionGroupController.InitConditionContent(2, 3, themeID, isPassedCondition: false);
					}
					break;
				}
				case 4:
				{
					UnityEngine.Debug.Log("dieu kien: so huu hero thu hai dat level " + item.Value);
					string[] array2 = item.Value.Split('_');
					int heroID3 = int.Parse(array2[0]);
					int num4 = int.Parse(array2[1]);
					if (ReadWriteDataHero.Instance.IsHeroOwned(heroID3) && ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID3) >= num4)
					{
						listConditionsValue[4] = 2;
						unlockConditionGroupController.InitConditionContent(4, 4, themeID, isPassedCondition: true);
					}
					else
					{
						listConditionsValue[4] = 1;
						unlockConditionGroupController.InitConditionContent(4, 4, themeID, isPassedCondition: false);
					}
					break;
				}
				case 6:
				{
					UnityEngine.Debug.Log("dieu kien: hero thu nhat so huu pet " + item.Value);
					int heroID = int.Parse(item.Value.Replace("p_", string.Empty));
					if (ReadWriteDataHero.Instance.IsHeroOwned(heroID) && ReadWriteDataHero.Instance.IsPetUnlocked(heroID))
					{
						listConditionsValue[5] = 2;
						unlockConditionGroupController.InitConditionContent(5, 6, themeID, isPassedCondition: true);
					}
					else
					{
						listConditionsValue[5] = 1;
						unlockConditionGroupController.InitConditionContent(5, 6, themeID, isPassedCondition: false);
					}
					break;
				}
				}
			}
		}

		private bool IsAllConditionPass()
		{
			bool result = false;
			int num = 0;
			int num2 = 0;
			int[] array = listConditionsValue;
			foreach (int num3 in array)
			{
				if (num3 > 0)
				{
					num++;
					num2 += num3;
				}
			}
			if (num2 == num * 2)
			{
				result = true;
			}
			return result;
		}

		public void OnClick_UnlockTheme()
		{
			ReadWriteDataTheme.Instance.SaveThemeUnlockData(currentThemeIDToUnlock);
			int themeIDUnlocked = ReadWriteDataTheme.Instance.GetThemeIDUnlocked();
			SingletonMonoBehaviour<WorldMap.UIRootController>.Instance.MapThemesController.SpawnTheme(themeIDUnlocked);
			ReadWriteDataTheme.Instance.SaveLastThemePlayed(themeIDUnlocked);
			SingletonMonoBehaviour<WorldMap.UIRootController>.Instance.MapThemesController.RefreshUnlockThemesStatus();
			SingletonMonoBehaviour<WorldMap.UIRootController>.Instance.MapThemesController.RefreshSelectThemesButtonStatus();
			CloseWithScaleAnimation();
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
		}
	}
}
