using Parameter;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Tower3Ultimate0Skill1 : TowerUltimateCommon
	{
		private int towerID = 3;

		private int ultimateBranch;

		private int skillID = 1;

		private int hpGenerate;

		private int reloadDecrease;

		private float duration;

		private float skillRange;

		private TowerModel towerModel;

		private string buffKey = "IncreaseAttackSpeed";

		private List<CharacterModel> allyInRange = new List<CharacterModel>();

		private float timeTracking;

		[SerializeField]
		private GameObject magicCircle;

		private void Update()
		{
			if (unlock)
			{
				if (timeTracking == 0f)
				{
					GetAlliesInRange();
					HealingAlly();
					AddBuffIncreaseAttackSpeed();
				}
				timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
			}
		}

		public override void InitTowerModel(TowerModel towerModel)
		{
			this.towerModel = towerModel;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			unlock = true;
			ReadParameter(ultiLevel);
			TryToCreateMagicAura();
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_HEAL_1);
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			magicCircle.SetActive(value: false);
		}

		private void ReadParameter(int currentSkillLevel)
		{
			hpGenerate = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			reloadDecrease = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			skillRange = (float)TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 2) / GameData.PIXEL_PER_UNIT;
			duration = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 3);
			timeTracking = duration;
		}

		public void TryToCreateMagicAura()
		{
			if (unlock)
			{
				magicCircle.SetActive(value: true);
			}
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		private void GetAlliesInRange()
		{
			allyInRange.Clear();
			List<CharacterModel> listActiveAlly = SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly;
			for (int i = 0; i < listActiveAlly.Count; i++)
			{
				CharacterModel characterModel = listActiveAlly[i];
				float num = SingletonMonoBehaviour<GameData>.Instance.SqrDistance(base.gameObject, characterModel.gameObject);
				if (num <= skillRange * skillRange)
				{
					allyInRange.Add(characterModel);
				}
			}
			timeTracking = duration;
			UnityEngine.Debug.Log("ally in range " + allyInRange.Count);
		}

		private void HealingAlly()
		{
			foreach (CharacterModel item in allyInRange)
			{
				if (item.IsAlive)
				{
					item.IncreaseHealth(hpGenerate);
					EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_HEAL_1);
					effect.transform.position = item.transform.position;
					if ((bool)item.GetComponent<SpriteRenderer>())
					{
						effect.Init(timeTracking + 0.5f, item.BuffsHolder.transform, item.GetComponent<SpriteRenderer>().sprite.rect.width);
					}
					else
					{
						effect.Init(timeTracking + 0.5f, item.BuffsHolder.transform, item.GetComponentInChildren<SpriteRenderer>().sprite.rect.width);
					}
				}
			}
		}

		private void AddBuffIncreaseAttackSpeed()
		{
			foreach (CharacterModel item in allyInRange)
			{
				item.BuffsHolder.AddBuff(buffKey, new Buff(isPositive: true, reloadDecrease, duration), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
			}
		}
	}
}
