using CodeStage.AntiCheat.ObscuredTypes;
using Data;
using DG.Tweening;
using Gameplay;
using Middle;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTools
{
	public delegate float CustomScoreModifier(CharacterModel characterModel, EnemyModel enemy);

	public delegate bool CustomIsValidAlly(CharacterModel characterModel);

	public static float petDisToOwnerThreshold = 0.4f;

	public static float sqPetDisToOwnerThreshold = petDisToOwnerThreshold * petDisToOwnerThreshold;

	public static string pet1003BulletPath = "Bullets/hero_1003_bullet_0";

	public static string pet1003BulletName = "hero_1003_bullet_0(Clone)";

	private static string PLAY_TIME_COUNT = "playCountKe";

	public static int deltaValue = 1046527;

	private static Dictionary<string, TdLocalizationData> keyToLocalization;

	public static SystemLanguage curLanguage;

	private static int _uniqueId = 0;

	public static List<TourPlayerInfo> tourplayers;

	public static Dictionary<string, Tour_GroupInfo> allGroupInfos = new Dictionary<string, Tour_GroupInfo>();

	public static TourUserSelfInfo tourUserSelfInfo;

	public static TourSeasonInfo tourSeasonInfo;

	public static int blessedHeroId = -1;

	public static int maxUserPerTourGroup = 50;

	public static int requiredNumOfTourFriend = 5;

	public static bool isTestingSeasonReward = false;

	public static bool isTestingFriendReward = false;

	public static bool isTestingFriendRewardNoFakeUser = false;

	public static bool cachedHavingBooster = false;

	public static float cachedBoosterMultiplier = 1f;

	public static Dictionary<string, SubscriptionTypeEnum> productIdToSubscriptionEnum = new Dictionary<string, SubscriptionTypeEnum>
	{
		{
			"kd.sale.bundle.dailybooster",
			SubscriptionTypeEnum.dailyBooster
		},
		{
			"kd.sale.bundle.boosterone",
			SubscriptionTypeEnum.fiftyPercentAtkBoost
		},
		{
			"kd.sale.bundle.boostertwo",
			SubscriptionTypeEnum.doubleAttack
		}
	};

	private static string EVENT_PREFIX = "evDt_";

	public static string EVENT_ENDTIME_PREFIX = "ev_end_";

	private static string LISTEVENT = "evList_";

	private static string UNCLAIM_EV_LIST = "unclaimEvs";

	public static string LAST_DATE_INIT_EVENT = "lastDateInitEv";

	public static int GetGemQuantityTostartTour(bool increasePlayCount)
	{
		int numOfPlayTourCount = GetNumOfPlayTourCount();
		if (numOfPlayTourCount == 0)
		{
			return 0;
		}
		float jFactor = MapRuleParameter.Instance.leagueIndexToPriceConstants[tourUserSelfInfo.curtier].jFactor;
		int initGemQuantity = MapRuleParameter.Instance.leagueIndexToPriceConstants[tourUserSelfInfo.curtier].initGemQuantity;
		if (increasePlayCount)
		{
			SaveNumOfPlayTourCount(numOfPlayTourCount + 1);
		}
		return Mathf.CeilToInt((float)initGemQuantity * Mathf.Pow(jFactor, numOfPlayTourCount - 1));
	}

	public static int GetNumOfPlayTourCount()
	{
		return PlayerPrefs.GetInt(PLAY_TIME_COUNT, 0);
	}

	public static void SaveNumOfPlayTourCount(int count)
	{
		PlayerPrefs.SetInt(PLAY_TIME_COUNT, count);
	}

	public static bool IsValidCharacter(CharacterModel characterModel)
	{
		if (characterModel == null)
		{
			return false;
		}
		if (!characterModel.IsAlive)
		{
			return false;
		}
		return true;
	}

	public static bool IsCharacterVisible(CharacterModel characterModel)
	{
		return !characterModel.IsInvisible;
	}

	public static bool IsValidEnemy(EnemyModel enemyModel)
	{
		if (enemyModel == null)
		{
			return false;
		}
		if (!enemyModel.IsAlive)
		{
			return false;
		}
		if (!SingletonMonoBehaviour<GameData>.Instance.IsListActiveEnemyContainThis(enemyModel))
		{
			return false;
		}
		return true;
	}

	public static bool IsEnemyAbleToGoTunnel(EnemyModel enemyModel)
	{
		if (enemyModel.IsAir)
		{
			return false;
		}
		if (enemyModel.IsUnderground)
		{
			return false;
		}
		return true;
	}

	public static EnemyModel GetNearstEnemy(GameObject sources)
	{
		EnemyModel result = null;
		List<EnemyModel> listActiveEnemy = SingletonMonoBehaviour<GameData>.Instance.ListActiveEnemy;
		if (listActiveEnemy.Count > 0)
		{
			float num = float.PositiveInfinity;
			for (int num2 = listActiveEnemy.Count - 1; num2 >= 0; num2--)
			{
				if (listActiveEnemy[num2].curState != EntityStateEnum.EnemySpecialState)
				{
					Enemy originalParameter = listActiveEnemy[num2].OriginalParameter;
					if (!originalParameter.isBoss)
					{
						float sqrMagnitude = (sources.transform.position - listActiveEnemy[num2].transform.position).sqrMagnitude;
						if (sqrMagnitude < num)
						{
							num = sqrMagnitude;
							result = listActiveEnemy[num2];
						}
					}
				}
			}
		}
		return result;
	}

	public static List<EnemyModel> GetListEnemiesInRange(GameObject sources, CommonAttackDamage commonAttackDamage)
	{
		return GetListEnemiesInRange(sources.transform.position, commonAttackDamage);
	}

	public static List<EnemyModel> GetListEnemiesInRange(Vector3 sourcePos, CommonAttackDamage commonAttackDamage)
	{
		List<EnemyModel> list = new List<EnemyModel>();
		List<EnemyModel> listActiveEnemy = SingletonMonoBehaviour<GameData>.Instance.ListActiveEnemy;
		for (int i = 0; i < listActiveEnemy.Count; i++)
		{
			EnemyModel enemyModel = listActiveEnemy[i];
			if ((!enemyModel.IsAir || commonAttackDamage.targetType.isAir) && (!enemyModel.IsUnderground || commonAttackDamage.targetType.isUnderGround) && (!enemyModel.IsInTunnel || commonAttackDamage.targetType.isTunnel))
			{
				float num = SingletonMonoBehaviour<GameData>.Instance.SqrDistance(sourcePos, enemyModel.transform.position);
				if (num <= commonAttackDamage.aoeRange * commonAttackDamage.aoeRange)
				{
					list.Add(enemyModel);
				}
			}
		}
		return list;
	}

	public static EnemyModel GetRandomEnemyInRange(GameObject sources, CommonAttackDamage commonAttackDamage)
	{
		EnemyModel enemyModel = new EnemyModel();
		List<EnemyModel> listEnemiesInRange = GetListEnemiesInRange(sources, commonAttackDamage);
		if (listEnemiesInRange.Count > 0)
		{
			return listEnemiesInRange[UnityEngine.Random.Range(0, listEnemiesInRange.Count)];
		}
		return null;
	}

	public static List<EnemyModel> GetListFlyingEnemiesInRange(GameObject sources, CommonAttackDamage commonAttackDamage)
	{
		List<EnemyModel> list = new List<EnemyModel>();
		List<EnemyModel> listActiveEnemy = SingletonMonoBehaviour<GameData>.Instance.ListActiveEnemy;
		for (int i = 0; i < listActiveEnemy.Count; i++)
		{
			EnemyModel enemyModel = listActiveEnemy[i];
			if (enemyModel.IsAir && commonAttackDamage.targetType.isAir)
			{
				float num = SingletonMonoBehaviour<GameData>.Instance.SqrDistance(sources, enemyModel.gameObject);
				if (num <= commonAttackDamage.aoeRange * commonAttackDamage.aoeRange)
				{
					list.Add(enemyModel);
				}
			}
		}
		return list;
	}

	public static bool IsUnderTargetOfASpecificHero(EnemyModel self, CharacterModel characterModel)
	{
		if (!IsValidCharacter(characterModel))
		{
			return false;
		}
		if (characterModel.GetCurrentTarget() == null)
		{
			return false;
		}
		if (characterModel.GetCurrentTarget().GetInstanceID() != self.GetInstanceID())
		{
			return false;
		}
		return true;
	}

	public static bool IsUnderTargetOfAnyHero(EnemyModel self, bool hasExceptionHero = false, int exceptionId = -1)
	{
		if (!IsValidEnemy(self))
		{
			return true;
		}
		List<CharacterModel> listActiveAlly = SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly;
		for (int num = listActiveAlly.Count - 1; num >= 0; num--)
		{
			if (listActiveAlly[num].GetCurrentTarget() != null && (!hasExceptionHero || listActiveAlly[num].GetInstanceID() != exceptionId) && listActiveAlly[num].GetCurrentTarget().GetInstanceID() == self.GetInstanceID() && listActiveAlly[num].GetFSMController().GetCurrentState() is NewHeroMeleeAtkState)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsPetHavingAtkState(HeroModel petModel)
	{
		if (!petModel.IsPet)
		{
			return false;
		}
		return petModel.PetConfigData.Atk_magic_min > 0 || petModel.PetConfigData.Atk_physics_min > 0;
	}

	public static void CalculateAttackPosition(CharacterModel heroModel, EnemyModel enemy, float speed, out Vector3 attackPosition, out float timeMovingToAtkPos)
	{
		float x = 0f;
		float attackRangeMin = heroModel.GetAttackRangeMin();
		Enemy originalParameter = enemy.OriginalParameter;
		float num = attackRangeMin + (float)originalParameter.body_size / GameData.PIXEL_PER_UNIT;
		float num2 = 0f;
		Vector3 position = enemy.transform.position;
		float x2 = position.x;
		Vector3 position2 = heroModel.transform.position;
		if (x2 > position2.x)
		{
			Vector3 position3 = enemy.transform.position;
			x = position3.x - num;
		}
		Vector3 position4 = enemy.transform.position;
		float x3 = position4.x;
		Vector3 position5 = heroModel.transform.position;
		if (x3 < position5.x)
		{
			Vector3 position6 = enemy.transform.position;
			x = position6.x + num;
		}
		Vector3 position7 = enemy.transform.position;
		float y = position7.y + num2;
		attackPosition = new Vector3(x, y, 0f);
		float num3 = Vector2.Distance(heroModel.transform.position, attackPosition);
		timeMovingToAtkPos = num3 / (speed / GameData.PIXEL_PER_UNIT);
	}

	public static void CalculatePosition(CharacterModel heroModel, float speed, Vector3 targetPosition, out float timeMovingToAtkPos)
	{
		float num = Vector2.Distance(heroModel.transform.position, targetPosition);
		timeMovingToAtkPos = num / (speed / GameData.PIXEL_PER_UNIT);
	}

	public static float MoveToAttackPosition(CharacterModel heroModel, EnemyModel enemy, float speed, Action CallbackWhencompleteMove)
	{
		CalculateAttackPosition(heroModel, enemy, speed, out Vector3 attackPosition, out float timeMovingToAtkPos);
		heroModel.transform.DOMove(attackPosition, timeMovingToAtkPos).SetEase(Ease.Linear).OnComplete(delegate
		{
			if (CallbackWhencompleteMove != null)
			{
				CallbackWhencompleteMove();
			}
		});
		heroModel.GetAnimationController().ToRunState();
		float x = attackPosition.x;
		Vector3 position = heroModel.transform.position;
		if (x > position.x)
		{
			heroModel.transform.localScale = Vector3.one;
		}
		else
		{
			heroModel.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		return timeMovingToAtkPos;
	}

	public static float TimeMoveToPosition(CharacterModel heroModel, float speed, Vector3 targetPosition, Action CallbackWhencompleteMove)
	{
		CalculatePosition(heroModel, speed, targetPosition, out float timeMovingToAtkPos);
		heroModel.transform.DOMove(targetPosition, timeMovingToAtkPos).SetEase(Ease.Linear).OnComplete(delegate
		{
			if (CallbackWhencompleteMove != null)
			{
				CallbackWhencompleteMove();
			}
		});
		heroModel.GetAnimationController().ToRunState();
		return timeMovingToAtkPos;
	}

	public static int GetUniqueId()
	{
		_uniqueId++;
		return _uniqueId;
	}

	public static int GetTowerSourceId(int level, int id)
	{
		return level * 10 + id;
	}

	public static void SetRewardSprite(RewardItem rewardItem, Image itemAvatar)
	{
		switch (rewardItem.rewardType)
		{
		case RewardType.Life:
			itemAvatar.sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_life");
			break;
		case RewardType.Money:
			itemAvatar.sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_money");
			break;
		case RewardType.Gem:
			itemAvatar.sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_gem");
			break;
		case RewardType.Item:
			itemAvatar.sprite = Resources.Load<Sprite>($"LuckyChest/Items/lucky_item_pw_{rewardItem.itemID}");
			break;
		case RewardType.SingleHero:
			itemAvatar.sprite = Resources.Load<Sprite>($"LuckyChest/Items/lucky_item_hero_{rewardItem.itemID}");
			break;
		case RewardType.League:
			itemAvatar.sprite = Resources.Load<Sprite>($"LuckyChest/Items/h{rewardItem.itemID + 1}");
			break;
		}
	}

	public static List<TournamentPrizeConfigData> GetLeagueAllPrize(int leagueIndex)
	{
		List<TournamentPrizeConfigData> list = new List<TournamentPrizeConfigData>();
		TournamentPrizeConfig tournamentPrizeConfig = CommonData.Instance.tournamentPrizeConfig;
		int num = tournamentPrizeConfig.dataArray.Length;
		for (int i = 0; i < num; i++)
		{
			if (tournamentPrizeConfig.dataArray[i].Leagueindex == leagueIndex)
			{
				list.Add(tournamentPrizeConfig.dataArray[i]);
			}
		}
		if (list.Count <= 0)
		{
			leagueIndex = 0;
			for (int j = 0; j < num; j++)
			{
				if (tournamentPrizeConfig.dataArray[j].Leagueindex == leagueIndex)
				{
					list.Add(tournamentPrizeConfig.dataArray[j]);
				}
			}
		}
		return list;
	}

	public static List<RewardItem> GetTournamentRewardList(TournamentPrizeConfigData prize)
	{
		List<RewardItem> list = new List<RewardItem>();
		string[] array = prize.Itemtypes.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			switch (array[i])
			{
			case "Gem":
				list.Add(new RewardItem(RewardType.Gem, prize.Itemquantities[i], isDisplayQuantity: true));
				break;
			case "Item0":
				list.Add(new RewardItem(RewardType.Item, 0, prize.Itemquantities[i], isDisplayQuantity: true));
				break;
			case "Item1":
				list.Add(new RewardItem(RewardType.Item, 1, prize.Itemquantities[i], isDisplayQuantity: true));
				break;
			case "Item2":
				list.Add(new RewardItem(RewardType.Item, 2, prize.Itemquantities[i], isDisplayQuantity: true));
				break;
			case "Item3":
				list.Add(new RewardItem(RewardType.Item, 3, prize.Itemquantities[i], isDisplayQuantity: true));
				break;
			case "League":
				list.Add(new RewardItem(RewardType.League, prize.Leagueindex + 1, prize.Itemquantities[i], isDisplayQuantity: true));
				break;
			}
		}
		return list;
	}

	public static int[] DecodeStringToIntArray(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			UnityEngine.Debug.LogError("DecodeStringToIntArray null or empty string");
			return new int[0];
		}
		string[] array = s.Split(',');
		int num = array.Length;
		if (num > 0 && string.IsNullOrEmpty(array[num - 1]))
		{
			num--;
		}
		int[] array2 = new int[num];
		for (int i = 0; i < num; i++)
		{
			array2[i] = int.Parse(array[i]);
		}
		return array2;
	}

	public static int GetNumberOfLeagues()
	{
		return 7;
	}

	public static string ConvertIconToText(TdSpriteId spriteId)
	{
		return $"<sprite={(int)spriteId}>";
	}

	public static string GetPetName(int petId)
	{
		int num = petId % 1000;
		PetConfigData petConfigData = CommonData.Instance.petConfig.dataArray[num];
		return petConfigData.Petname;
	}

	public static string GetPetDescription(int petId)
	{
		int num = petId % 1000;
		PetConfigData petConfigData = CommonData.Instance.petConfig.dataArray[num];
		string[] array = new string[petConfigData.Desc_values.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = petConfigData.Desc_values[i].ToString();
		}
		return string.Format(GetLocalization(petConfigData.Desc), array);
	}

	public static string GetLocalization(string key)
	{
		if (keyToLocalization == null)
		{
			keyToLocalization = new Dictionary<string, TdLocalizationData>();
			SetCurrentLanguage(Config.Instance.LanguageID);
			TdLocalizationData[] dataArray = CommonData.Instance.tdLocalizationConfig.dataArray;
			for (int num = dataArray.Length - 1; num >= 0; num--)
			{
				if (!string.IsNullOrEmpty(dataArray[num].Key) && !keyToLocalization.ContainsKey(dataArray[num].Key))
				{
					keyToLocalization.Add(dataArray[num].Key, dataArray[num]);
				}
			}
		}
		if (!keyToLocalization.ContainsKey(key))
		{
			return string.Empty;
		}
		switch (curLanguage)
		{
		case SystemLanguage.Portuguese:
			return keyToLocalization[key].Brazil;
		case SystemLanguage.French:
			return keyToLocalization[key].French;
		case SystemLanguage.German:
			return keyToLocalization[key].German;
		case SystemLanguage.Korean:
			return keyToLocalization[key].Korean;
		case SystemLanguage.Polish:
			return keyToLocalization[key].Polish;
		case SystemLanguage.Russian:
			return keyToLocalization[key].Russian;
		case SystemLanguage.Spanish:
			return keyToLocalization[key].Spanish;
		case SystemLanguage.Vietnamese:
			return keyToLocalization[key].Vi;
		case SystemLanguage.Chinese:
			return keyToLocalization[key].Chinese;
		case SystemLanguage.Japanese:
			return keyToLocalization[key].Japanese;
		default:
			return keyToLocalization[key].En;
		}
	}

	public static void SetCurrentLanguage(string lang)
	{
		curLanguage = SystemLanguage.English;
		if (lang == "lg_brazil")
		{
			curLanguage = SystemLanguage.Portuguese;
		}
		else if (lang == "lg_en")
		{
			curLanguage = SystemLanguage.English;
		}
		else if (lang == "lg_french")
		{
			curLanguage = SystemLanguage.French;
		}
		else if (lang == "lg_german")
		{
			curLanguage = SystemLanguage.German;
		}
		else if (lang == "lg_korean")
		{
			curLanguage = SystemLanguage.Korean;
		}
		else if (lang == "lg_polish")
		{
			curLanguage = SystemLanguage.Polish;
		}
		else if (lang == "lg_russian")
		{
			curLanguage = SystemLanguage.Russian;
		}
		else if (lang == "lg_spanish")
		{
			curLanguage = SystemLanguage.Spanish;
		}
		else if (lang == "lg_chinese")
		{
			curLanguage = SystemLanguage.Chinese;
		}
		else if (lang == "lg_vi")
		{
			curLanguage = SystemLanguage.Vietnamese;
		}
		else if (lang == "lg_japanese")
		{
			curLanguage = SystemLanguage.Japanese;
		}
	}

	public static int GetEncodedHeroList(List<int> heroList)
	{
		int num = 0;
		int num2 = Mathf.Min(heroList.Count, 3);
		for (int num3 = num2 - 1; num3 >= 0; num3--)
		{
			num = num * 1000 + (heroList[num3] + 1);
		}
		return num;
	}

	public static List<int> DecodeHeroList(int encodeHeroes)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < 3; i++)
		{
			int item = encodeHeroes % 1000 - 1;
			list.Add(item);
			encodeHeroes /= 1000;
			if (encodeHeroes <= 0)
			{
				break;
			}
		}
		return list;
	}

	public static bool IsUltimateHero(int heroId)
	{
		return IsPetAvailable(heroId + 1000) && ReadWriteDataHero.Instance.IsPetUnlocked(heroId);
	}

	public static bool IsPetAvailable(int petId)
	{
		int num = petId % 1000;
		return CommonData.Instance.petConfig.dataArray[num].Is_available > 0;
	}

	public static bool IsBitOn(int number, int bitIndex)
	{
		return ((number >> bitIndex) & 1) > 0;
	}

	public static void SetBit(ref int number, int bitIndex, bool toOnState)
	{
		if (toOnState)
		{
			number |= 1 << bitIndex;
		}
		else
		{
			number &= ~(1 << bitIndex);
		}
	}

	public static bool IsCharacterHavingCondition(int charOverallCondition, CharacterCondition conditionType)
	{
		return IsBitOn(charOverallCondition, (int)conditionType);
	}

	public static void SetCharacterCondition(ref int charOverallCondition, CharacterCondition conditionType, bool toOnState)
	{
		SetBit(ref charOverallCondition, (int)conditionType, toOnState);
	}

	public static DateTime FromUnixTimeToDateTime(long unixTime)
	{
		return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime().AddMilliseconds(unixTime);
	}

	public static void WriteTimeStamp(string key, DateTime localdate)
	{
		DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
		PlayerPrefs.SetString(key, ((long)(localdate - d).TotalMilliseconds).ToString());
	}

	public static DateTime ReadTimeStamp(string key)
	{
		string @string = PlayerPrefs.GetString(key, "0");
		long.TryParse(@string, out long result);
		return FromUnixTimeToDateTime(result);
	}

	public static DateTime GetNow()
	{
		return UnbiasedTime.Instance.Now();
	}

	public static DateTime GetMoment0(DateTime day)
	{
		day = day.AddHours(-day.Hour);
		day = day.AddMinutes(-day.Minute);
		day = day.AddSeconds(-day.Second);
		return day;
	}

	public static EnemyModel SummonEnemy(int enemyId, int gate)
	{
		if (enemyId < 0)
		{
			UnityEngine.Debug.LogError("Input ID < 0");
			return null;
		}
		EnemyModel enemyModel = null;
		string gameObjectName = $"enemy_{enemyId}(Clone)";
		GameObject gameObject = TrashMan.spawn(gameObjectName);
		enemyModel = gameObject.GetComponent<EnemyModel>();
		enemyModel.transform.parent = SingletonMonoBehaviour<SpawnEnemy>.Instance.transform;
		SingletonMonoBehaviour<GameData>.Instance.AddEnemyToListActiveEnemy(enemyModel);
		enemyModel.gameObject.SetActive(value: true);
		enemyModel.gameObject.transform.localScale = Vector3.one;
		enemyModel.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
		enemyModel.SetDataStartRun(enemyId, (int)MiddleDelivery.Instance.BattleLevel, gate, UnityEngine.Random.Range(0, Config.Instance.LineCount));
		enemyModel.GetFSMController();
		return enemyModel;
	}

	public static List<CharacterModel> GetAllyInRange(Vector3 centerPos, CustomIsValidAlly customIsValidAlly, float detectRange)
	{
		List<CharacterModel> list = new List<CharacterModel>();
		float num = detectRange * detectRange;
		List<CharacterModel> listActiveAlly = SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly;
		for (int num2 = listActiveAlly.Count - 1; num2 >= 0; num2--)
		{
			if (IsValidCharacter(listActiveAlly[num2]) && (centerPos - listActiveAlly[num2].transform.position).sqrMagnitude <= num && customIsValidAlly(listActiveAlly[num2]))
			{
				list.Add(listActiveAlly[num2]);
			}
		}
		return list;
	}

	public static CharacterModel GetAllyWithHighestScore(EnemyModel enemy, CustomIsValidAlly customIsValidAlly, float detectRange, CustomScoreModifier customScoreModifier = null)
	{
		float num = -1000000f;
		CharacterModel result = null;
		float num2 = detectRange * detectRange;
		List<CharacterModel> listActiveAlly = SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly;
		for (int num3 = listActiveAlly.Count - 1; num3 >= 0; num3--)
		{
			if (IsValidCharacter(listActiveAlly[num3]) && (enemy.transform.position - listActiveAlly[num3].transform.position).sqrMagnitude <= num2 && customIsValidAlly(listActiveAlly[num3]) && IsCharacterVisible(listActiveAlly[num3]))
			{
				float characterScore = GetCharacterScore(listActiveAlly[num3], enemy, customScoreModifier);
				if (characterScore > num)
				{
					num = characterScore;
					result = listActiveAlly[num3];
				}
			}
		}
		return result;
	}

	public static float GetCharacterScore(CharacterModel hero, EnemyModel enemy, CustomScoreModifier customScoreModifier = null)
	{
		float num = GetEnemyHeroDistanceScore(enemy, hero);
		if (customScoreModifier != null)
		{
			num += customScoreModifier(hero, enemy);
		}
		return num;
	}

	public static EnemyModel GetEnemyWithHighestScore(CharacterModel hero)
	{
		float num = -1000000f;
		EnemyModel result = null;
		List<EnemyModel> listActiveEnemy = SingletonMonoBehaviour<GameData>.Instance.ListActiveEnemy;
		for (int num2 = listActiveEnemy.Count - 1; num2 >= 0; num2--)
		{
			if (!listActiveEnemy[num2].IsInTunnel && (hero.IsInRangerRange(listActiveEnemy[num2]) || hero.IsInMeleeRange(listActiveEnemy[num2])))
			{
				float enemyScore = GetEnemyScore(hero, listActiveEnemy[num2]);
				if (enemyScore > num)
				{
					num = enemyScore;
					result = listActiveEnemy[num2];
				}
			}
		}
		return result;
	}

	public static float GetEnemyScore(CharacterModel hero, EnemyModel enemy)
	{
		float num = 0f;
		if (IsEnemyUnderAttackOfSpecificAlly(enemy, hero))
		{
			num += 20f;
		}
		num += GetEnemyHeroDistanceScore(enemy, hero);
		if (IsEnemyUnderAttackAndBlockingOfAnotherAlly(enemy, hero))
		{
			num -= 30f;
		}
		return num + GetEnemyTypeBonusScore(enemy, hero);
	}

	public static bool IsEnemyUnderAttackOfSpecificAlly(EnemyModel enemy, CharacterModel hero)
	{
		if (!IsValidEnemy(enemy))
		{
			return true;
		}
		if (IsValidEnemy(hero.GetCurrentTarget()) && hero.GetCurrentTarget().GetInstanceID() == enemy.GetInstanceID())
		{
			return true;
		}
		return false;
	}

	public static float GetEnemyHeroDistanceScore(EnemyModel enemy, CharacterModel hero)
	{
		float num = 10f;
		float sqrMagnitude = (enemy.transform.position - hero.transform.position).sqrMagnitude;
		float num2 = hero.GetRangerRange() * hero.GetRangerRange();
		return (1f - sqrMagnitude / num2) * num;
	}

	public static float GetEnemyTypeBonusScore(EnemyModel enemy, CharacterModel hero)
	{
		float num = 0f;
		if (hero.CanAttackAirEnemy() && enemy.IsAir)
		{
			num += 5f;
		}
		return num;
	}

	public static bool IsEnemyUnderAttackAndBlockingOfAnotherAlly(EnemyModel enemy, CharacterModel hero)
	{
		if (!IsValidEnemy(enemy))
		{
			return true;
		}
		if (enemy.EnemyFindTargetController == null)
		{
			return true;
		}
		if (enemy.EnemyFindTargetController.Target != null && enemy.EnemyFindTargetController.Target.GetInstanceID() != hero.GetInstanceID())
		{
			return true;
		}
		List<CharacterModel> listActiveAlly = SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly;
		for (int num = listActiveAlly.Count - 1; num >= 0; num--)
		{
			if (listActiveAlly[num].GetInstanceID() != hero.GetInstanceID() && IsValidEnemy(listActiveAlly[num].GetCurrentTarget()) && listActiveAlly[num].GetCurrentTarget().GetInstanceID() == enemy.GetInstanceID() && listActiveAlly[num].IsMeleeAttacking())
			{
				return true;
			}
		}
		return false;
	}

	public static void SaveListUnclaimedReward(List<EventConfigData> unclaimedList)
	{
		byte[] array = new byte[unclaimedList.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (byte)unclaimedList[i].Eventid;
		}
		ObscuredPrefs.SetByteArray(UNCLAIM_EV_LIST, array);
	}

	public static List<int> GetListUnclaimedReward()
	{
		List<int> list = new List<int>();
		byte[] byteArray = ObscuredPrefs.GetByteArray(UNCLAIM_EV_LIST);
		for (int i = 0; i < byteArray.Length; i++)
		{
			list.Add(byteArray[i]);
		}
		return list;
	}

	public static void SaveListRunningEvent(List<RunningEventData> listEvent)
	{
		byte[] array = new byte[listEvent.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (byte)listEvent[i].configData.Eventid;
			SaveRunningEventProgress(listEvent[i]);
			WriteTimeStamp(EVENT_ENDTIME_PREFIX + listEvent[i].configData.Eventid, listEvent[i].endTime);
		}
		ObscuredPrefs.SetByteArray(LISTEVENT, array);
	}

	public static byte[] GetListRunningEvent()
	{
		return ObscuredPrefs.GetByteArray(LISTEVENT);
	}

	public static void SaveRunningEventProgress(RunningEventData eventData)
	{
		ObscuredPrefs.SetInt(EVENT_PREFIX + eventData.configData.Eventid, eventData.curProgress.Value);
	}

	public static void GetRunningEventProgress(RunningEventData eventData)
	{
		int @int = ObscuredPrefs.GetInt(EVENT_PREFIX + eventData.configData.Eventid, 0);
		DateTime endTime = ReadTimeStamp(EVENT_ENDTIME_PREFIX + eventData.configData.Eventid);
		eventData.curProgress.Value = @int;
		eventData.endTime = endTime;
	}

	public static int GetDayOfYearUpdateEvent()
	{
		return PlayerPrefs.GetInt("dayOYupdateEv", -1);
	}

	public static void SetDayOfYearUpdateEvent(int day)
	{
		PlayerPrefs.SetInt("dayOYupdateEv", day);
	}

	public static bool IsSubscriptionActive(SubscriptionTypeEnum subId)
	{
		DateTime endSubscriptionTime = GetEndSubscriptionTime(subId);
		if ((endSubscriptionTime - GetNow()).Seconds > 0)
		{
			return true;
		}
		return false;
	}

	public static void SetEndSubscriptionTime(SubscriptionTypeEnum subId, DateTime localTime)
	{
		WriteTimeStamp("subEndTime" + (int)subId, localTime);
	}

	public static DateTime GetEndSubscriptionTime(SubscriptionTypeEnum subId)
	{
		return ReadTimeStamp("subEndTime" + (int)subId);
	}

	public static void SetLastTimeCheckInSubscription(SubscriptionTypeEnum subId, DateTime localTime)
	{
		WriteTimeStamp("checksubTime" + (int)subId, localTime);
	}

	public static DateTime GetLastTimeCheckInSubscription(SubscriptionTypeEnum subId)
	{
		return ReadTimeStamp("checksubTime" + (int)subId);
	}

	public static float SquareDistance(float x1, float y1, float x2, float y2)
	{
		return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
	}

	public static float SquareDistance(Vector3 a, Vector3 b)
	{
		return SquareDistance(a.x, a.y, b.x, b.y);
	}

	public static float SquareDistancePointSegment(float x, float z, float px, float pz, float qx, float qz)
	{
		float num = qx - px;
		float num2 = qz - pz;
		float num3 = x - px;
		float num4 = z - pz;
		float num5 = num * num + num2 * num2;
		float num6 = num * num3 + num2 * num4;
		if (num5 > 0f)
		{
			num6 /= num5;
		}
		if (num6 < 0f)
		{
			num6 = 0f;
		}
		else if (num6 > 1f)
		{
			num6 = 1f;
		}
		num3 = px + num6 * num - x;
		num4 = pz + num6 * num2 - z;
		return num3 * num3 + num4 * num4;
	}

	public static float SquareDistancePointSegment(Vector3 point, Vector3 segmentPoint1, Vector3 segmentPoint2)
	{
		return SquareDistancePointSegment(point.x, point.y, segmentPoint1.x, segmentPoint1.y, segmentPoint2.x, segmentPoint2.y);
	}

	public static bool Left(Vector3 a, Vector3 b, Vector3 p)
	{
		return (b.x - a.x) * (p.y - a.y) - (p.x - a.x) * (b.y - a.y) <= 0f;
	}

	public static bool Left(Vector3 a, Vector3 p)
	{
		return (0f - a.x) * (p.y - a.y) - (p.x - a.x) * (0f - a.y) <= 0f;
	}

	public static bool HaveProjectOnSegment(Vector3 a, Vector3 b, Vector3 p)
	{
		Vector3 b2 = new Vector3(a.y - b.y, b.x - a.x, 0f);
		return Left(a, a + b2, p) != Left(b, b + b2, p);
	}

	public static Vector3 GetProjectOnLine(Vector3 a, Vector3 b, Vector3 p)
	{
		if (a.x == b.x && a.y == b.y)
		{
			a.x -= 1E-05f;
		}
		float num = (p.x - a.x) * (b.x - a.x) + (p.y - a.y) * (b.y - a.y);
		float num2 = (b.x - a.x) * (b.x - a.x) + (b.y - a.y) * (b.y - a.y);
		num /= num2;
		return new Vector3(a.x + num * (b.x - a.x), a.y + num * (b.y - a.y), 0f);
	}
}
