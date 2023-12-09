using Data;
using DG.Tweening;
using Middle;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class SpawnAlly : SingletonMonoBehaviour<SpawnAlly>
	{
		private Vector3 PoolPosition = new Vector3(1000f, 100f, 0f);

		private Dictionary<int, HeroModel> listActiveHero = new Dictionary<int, HeroModel>();

		public Dictionary<int, HeroModel> ListActiveHero
		{
			get
			{
				return listActiveHero;
			}
			set
			{
				listActiveHero = value;
			}
		}

		public void InitHeroesStartPosition()
		{
			List<int> list = new List<int>();
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				list = SingletonMonoBehaviour<GameData>.Instance.ListHeroesIdsSelected;
				break;
			case GameMode.DailyTrialMode:
			{
				int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
				list = DailyTrialParameter.Instance.getListInputHeroesID(currentDayIndex);
				break;
			}
			case GameMode.TournamentMode:
				list = SingletonMonoBehaviour<GameData>.Instance.ListHeroesIdsSelected;
				break;
			}
			for (int i = 0; i < list.Count; i++)
			{
				HeroModel heroModel = Get(list[i]);
				heroModel.transform.position = SingletonMonoBehaviour<HeroStartPositionManager>.Instance.listStartPosition[i].position;
				heroModel.SetAssignedPosition(heroModel.transform.position);
				if (!ListActiveHero.ContainsKey(list[i]))
				{
					ListActiveHero.Add(list[i], heroModel);
				}
			}
		}

		public float GetHeroSkillCooldownTime(int heroID)
		{
			HeroModel value = null;
			listActiveHero.TryGetValue(heroID, out value);
			return value.HeroSkillController.GetActiveSkillCooldownTime();
		}

		public string GetHeroSkillUseType(int heroID)
		{
			HeroModel value = null;
			listActiveHero.TryGetValue(heroID, out value);
			return value.HeroSkillController.GetActiveSkillUseType();
		}

		public void PushAlliesToPool(int towerID, int towerLevel, int allocateNumber)
		{
			string arg = $"ally_{towerID}_{towerLevel}";
			AllyModel allyModel = null;
			allyModel = Object.Instantiate(Resources.Load<AllyModel>($"Allies/{arg}"));
			allyModel.gameObject.SetActive(value: false);
			TrashManRecycleBin trashManRecycleBin = new TrashManRecycleBin();
			trashManRecycleBin.prefab = allyModel.gameObject;
			trashManRecycleBin.instancesToPreallocate = allocateNumber;
			TrashManRecycleBin recycleBin = trashManRecycleBin;
			TrashMan.manageRecycleBin(recycleBin);
			TrashMan.despawn(allyModel.gameObject);
		}

		public AllyModel Get(int id, int level)
		{
			AllyModel allyModel = null;
			string gameObjectName = $"ally_{id}_{level}(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			allyModel = gameObject.GetComponent<AllyModel>();
			allyModel.transform.parent = base.transform;
			allyModel.gameObject.SetActive(value: false);
			return allyModel;
		}

		public void Push(AllyModel ally, float delayTime)
		{
			StartCoroutine(IPushAlly(ally, delayTime));
		}

		private IEnumerator IPushAlly(AllyModel ally, float delayTime)
		{
			ally.transform.DOKill();
			yield return new WaitForSeconds(delayTime);
			ally.transform.position = PoolPosition;
			ally.gameObject.SetActive(value: false);
			TrashMan.despawn(ally.gameObject);
		}

		public void InitPoolHeroes()
		{
			List<int> list = new List<int>();
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				list = SingletonMonoBehaviour<GameData>.Instance.ListHeroesIdsSelected;
				break;
			case GameMode.DailyTrialMode:
			{
				int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
				list = DailyTrialParameter.Instance.getListInputHeroesID(currentDayIndex);
				break;
			}
			case GameMode.TournamentMode:
				list = SingletonMonoBehaviour<GameData>.Instance.ListHeroesIdsSelected;
				break;
			}
			for (int i = 0; i < list.Count; i++)
			{
				HeroModel heroModel = null;
				heroModel = Object.Instantiate(Resources.Load<HeroModel>($"Heroes/hero_{list[i]}"));
				heroModel.gameObject.SetActive(value: false);
				heroModel.transform.position = Vector3.zero;
				TrashManRecycleBin trashManRecycleBin = new TrashManRecycleBin();
				trashManRecycleBin.prefab = heroModel.gameObject;
				trashManRecycleBin.instancesToPreallocate = 0;
				TrashManRecycleBin recycleBin = trashManRecycleBin;
				TrashMan.manageRecycleBin(recycleBin);
				TrashMan.despawn(heroModel.gameObject);
			}
		}

		private HeroModel Get(int id)
		{
			if (id < 0)
			{
				UnityEngine.Debug.LogError("Input Id <0");
				return null;
			}
			HeroModel heroModel = null;
			string gameObjectName = $"hero_{id}(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			heroModel = gameObject.GetComponent<HeroModel>();
			heroModel.transform.parent = base.transform;
			return heroModel;
		}

		public void Push(HeroModel hero)
		{
			hero.transform.DOKill();
			hero.transform.position = PoolPosition;
			hero.gameObject.SetActive(value: false);
			TrashMan.despawn(hero.gameObject);
		}

		public void RestoreHealthForAllAllies()
		{
			foreach (CharacterModel item in SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly)
			{
				item.RestoreHealth();
			}
		}

		public void Reload()
		{
		}
	}
}
