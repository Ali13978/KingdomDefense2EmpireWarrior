using Data;
using Middle;
using Parameter;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

namespace Gameplay
{
	public class GameplayUIHeroManager : SingletonMonoBehaviour<GameplayUIHeroManager>
	{
		[Space]
		[Header("Hero icon")]
		[SerializeField]
		private SelectHero heroIconOrigin;

		[SerializeField]
		private Transform heroIconParent;

		private Dictionary<int, SelectHero> listSelectHeroButton = new Dictionary<int, SelectHero>();

		[Space]
		[Header("Hero icon skill")]
		[SerializeField]
		private SelectHeroSkill heroSkillIconOrigin;

		[SerializeField]
		private Transform heroIconSkillParent;

		public Dictionary<int, SelectHeroSkill> listSelectHeroSkillButton = new Dictionary<int, SelectHeroSkill>();

		[Space]
		[Header("Hero Current Level Information")]
		[SerializeField]
		private HeroCurrentLevelInformationPopup heroCurrentLevelInformationPopup;

		[Space]
		[Header("Controllers")]
		[SerializeField]
		private GameObject heroMoveControllerGroup;

		[SerializeField]
		private GameObject heroSkillControllerGroup;

		private List<int> listHeroesID = new List<int>();

		public HeroCurrentLevelInformationPopup HeroCurrentLevelInformationPopup
		{
			get
			{
				return heroCurrentLevelInformationPopup;
			}
			set
			{
				heroCurrentLevelInformationPopup = value;
			}
		}

		private void Start()
		{
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				listHeroesID = SingletonMonoBehaviour<GameData>.Instance.ListHeroesIdsSelected;
				break;
			case GameMode.DailyTrialMode:
			{
				int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
				listHeroesID = DailyTrialParameter.Instance.getListInputHeroesID(currentDayIndex);
				break;
			}
			case GameMode.TournamentMode:
				listHeroesID = SingletonMonoBehaviour<GameData>.Instance.ListHeroesIdsSelected;
				break;
			}
			HeroesManager.Instance.onChooseHero += Instance_onChooseHero;
			HeroesManager.Instance.onUnChooseHero += Instance_onUnChooseHero;
			HeroesManager.Instance.onChooseHeroSkill += Instance_onChooseHeroSkill;
			HeroesManager.Instance.onUnChooseHeroSkill += Instance_onUnChooseHeroSkill;
			if (GameplayTutorialManager.Instance.IsTutorialDone() || !GameplayTutorialManager.Instance.IsTutorialMap())
			{
				EnableHeroUI();
			}
			else
			{
				DisableHeroUI();
			}
		}

		private void OnDestroy()
		{
			HeroesManager.Instance.onChooseHero -= Instance_onChooseHero;
			HeroesManager.Instance.onUnChooseHero -= Instance_onUnChooseHero;
		}

		private void Instance_onChooseHero(int inputHeroID)
		{
			DisableChooseOtherButtonHero(inputHeroID);
		}

		private void Instance_onUnChooseHero(int inputHeroID)
		{
			foreach (KeyValuePair<int, SelectHero> item in listSelectHeroButton)
			{
				if (item.Value.HeroID == inputHeroID)
				{
					item.Value.HideSelected();
				}
			}
		}

		private void Instance_onChooseHeroSkill(int inputHeroID)
		{
			DisableChooseOtherButtonHeroSkill(inputHeroID);
		}

		private void Instance_onUnChooseHeroSkill(int inputHeroID)
		{
			foreach (KeyValuePair<int, SelectHeroSkill> item in listSelectHeroSkillButton)
			{
				if (item.Value.HeroID == inputHeroID)
				{
					item.Value.HideSelected();
				}
			}
		}

		public void InitListHeroesSelected(int heroID)
		{
			if (!listSelectHeroButton.ContainsKey(heroID))
			{
				SelectHero selectHero = Object.Instantiate(heroIconOrigin);
				selectHero.transform.SetParent(heroIconParent);
				selectHero.transform.localScale = heroIconOrigin.transform.localScale;
				selectHero.Init(heroID);
				listSelectHeroButton.Add(heroID, selectHero);
			}
			else
			{
				listSelectHeroButton[heroID].ViewEnable();
			}
			if (!listSelectHeroSkillButton.ContainsKey(heroID))
			{
				SelectHeroSkill selectHeroSkill = Object.Instantiate(heroSkillIconOrigin);
				selectHeroSkill.transform.SetParent(heroIconSkillParent);
				selectHeroSkill.transform.localScale = heroSkillIconOrigin.transform.localScale;
				selectHeroSkill.Init(heroID);
				listSelectHeroSkillButton.Add(heroID, selectHeroSkill);
			}
			else
			{
				listSelectHeroSkillButton[heroID].ViewEnable();
			}
		}

		public void EnableHeroUI()
		{
			heroMoveControllerGroup.SetActive(value: true);
			heroSkillControllerGroup.SetActive(value: true);
		}

		private void DisableHeroUI()
		{
			heroMoveControllerGroup.SetActive(value: false);
			heroSkillControllerGroup.SetActive(value: false);
		}

		private void DisableChooseOtherButtonHero(int selectedHeroID)
		{
			foreach (KeyValuePair<int, SelectHero> item in listSelectHeroButton)
			{
				if (item.Value.HeroID != selectedHeroID)
				{
					item.Value.Refresh();
				}
			}
		}

		private void DisableChooseOtherButtonHeroSkill(int selectedHeroID)
		{
			foreach (KeyValuePair<int, SelectHeroSkill> item in listSelectHeroSkillButton)
			{
				if (item.Value.HeroID != selectedHeroID)
				{
					item.Value.Refresh();
				}
			}
		}

		public void UpdateHeroHealthBarStatus(int heroID, int currentHealth, int OriginHealth)
		{
			if (listSelectHeroButton.ContainsKey(heroID))
			{
				listSelectHeroButton[heroID].UpdateHealthBar(currentHealth, OriginHealth);
			}
		}

		public void DisableHeroesUI(int heroID)
		{
			if (listSelectHeroButton.ContainsKey(heroID))
			{
				listSelectHeroButton[heroID].ViewDisable();
				listSelectHeroButton[heroID].DoCooldown();
				listSelectHeroSkillButton[heroID].ViewDisableImmediately();
			}
		}

		public void RestoreAllCooldownSkill()
		{
			foreach (int item in listHeroesID)
			{
				RestoreCooldownSkill(item);
			}
		}

		private void RestoreCooldownSkill(int heroID)
		{
			listSelectHeroSkillButton[heroID].ViewEnableImmediately();
		}
	}
}
