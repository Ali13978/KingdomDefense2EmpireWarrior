using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Tower3Ultimate1Skill1 : TowerUltimateCommon
	{
		private int towerID = 3;

		private int ultimateBranch = 1;

		private int skillID = 1;

		private TowerModel towerModel;

		private int magicArmorDecrease;

		private float skillRange;

		[SerializeField]
		private float duration;

		[SerializeField]
		private float cooldownTime;

		private float timeTracking;

		private string buffKey = "ReduceMagicArmor";

		[SerializeField]
		private DamageToAOERange damageToAOERange;

		private EffectAttack effectAttack;

		public override void InitTowerModel(TowerModel towerModel)
		{
			this.towerModel = towerModel;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			ReadParameter(ultiLevel);
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void Update()
		{
			if (SingletonMonoBehaviour<GameData>.Instance.IsGameStart && unlock)
			{
				if (IsCooldownDone())
				{
					CastSkill();
				}
				timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
			}
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		private void ReadParameter(int currentSkillLevel)
		{
			magicArmorDecrease = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			skillRange = (float)TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1) / GameData.PIXEL_PER_UNIT;
			unlock = true;
			effectAttack.buffKey = buffKey;
			effectAttack.debuffChance = 100;
			effectAttack.debuffEffectValue = magicArmorDecrease;
			effectAttack.debuffEffectDuration = duration;
			InitFXs();
		}

		private void InitFXs()
		{
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void CastSkill()
		{
			damageToAOERange.CastDamage(DamageType.Range, new CommonAttackDamage(0, 0, skillRange), effectAttack);
			timeTracking = cooldownTime;
			towerModel.towerSoundController.PlayCastSkillSound(skillID);
		}
	}
}
