using Data;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero11Skill3 : HeroSkillCommon
	{
		public GameObject healFxPrefab;

		public float delayBtwHeal = 0.3f;

		private int heroID = 11;

		private int skillID = 3;

		private int currentLevel;

		private int currentSkillLevel;

		private HeroModel heroModel;

		private Hero11Skill3Param skillParams;

		private float cooldownDuration;

		private float cooldownCountdown;

		private float sqDisHeal;

		private GameObject healObj;

		private int healPerTime;

		private int numOfHealTimes;

		private bool unlocked;

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			unlocked = true;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			skillParams = (HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID) as HeroSkillParameter_11_3).listParam[currentSkillLevel - 1];
			cooldownDuration = (float)skillParams.cooldown_time * 0.001f;
			cooldownCountdown = cooldownDuration * 0.7f;
			float num = (float)skillParams.heal_range / GameData.PIXEL_PER_UNIT;
			sqDisHeal = num * num;
			float num2 = (float)skillParams.heal_duration * 0.001f;
			numOfHealTimes = Mathf.RoundToInt(num2 / delayBtwHeal);
			healPerTime = skillParams.total_heal / numOfHealTimes;
		}

		public override void Update()
		{
			base.Update();
			if (unlocked)
			{
				if (cooldownCountdown > 0f)
				{
					cooldownCountdown -= Time.deltaTime;
					return;
				}
				cooldownCountdown = cooldownDuration;
				StartCoroutine(CastSkill());
			}
		}

		private IEnumerator CastSkill()
		{
			healObj = ObjectPool.Spawn(healFxPrefab, heroModel.transform, Vector3.zero);
			UnityEngine.Debug.LogFormat("heal {0} times in {1}s, each time heals {2}hp", numOfHealTimes, skillParams.heal_duration, healPerTime);
			for (int i = 0; i < numOfHealTimes; i++)
			{
				yield return new WaitForSeconds(delayBtwHeal);
				List<CharacterModel> listAlly = SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly;
				for (int num = listAlly.Count - 1; num >= 0; num--)
				{
					if (SingletonMonoBehaviour<GameData>.Instance.SqrDistance(heroModel.transform.position, listAlly[num].transform.position) < sqDisHeal)
					{
						listAlly[num].IncreaseHealth(healPerTime);
					}
				}
			}
			healObj.Recycle();
		}
	}
}
