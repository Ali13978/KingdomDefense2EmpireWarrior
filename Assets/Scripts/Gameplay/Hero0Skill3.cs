using Data;
using Parameter;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero0Skill3 : HeroSkillCommon
	{
		private int heroID;

		private int skillID = 3;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private float skillRange;

		private float armorPerUnit;

		private float armorMax;

		[SerializeField]
		private float timeToCheck;

		private float timeTracking;

		private float armorAmount;

		private List<EnemyModel> inRangeEnemies = new List<EnemyModel>();

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_0_3 heroSkillParameter_0_ = new HeroSkillParameter_0_3();
			heroSkillParameter_0_ = (HeroSkillParameter_0_3)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			skillRange = (float)heroSkillParameter_0_.getParam(currentSkillLevel - 1).skill_range / GameData.PIXEL_PER_UNIT;
			armorPerUnit = (float)heroSkillParameter_0_.getParam(currentSkillLevel - 1).armor_per_unit / 100f;
			armorMax = (float)heroSkillParameter_0_.getParam(currentSkillLevel - 1).armor_max / 100f;
		}

		public void Init(bool unlock, HeroModel heroModel, int _skillRange, int _armorPerUnit, int _armorMax)
		{
			this.heroModel = heroModel;
			skillRange = (float)_skillRange / GameData.PIXEL_PER_UNIT;
			armorPerUnit = (float)_armorPerUnit / 100f;
			armorMax = (float)_armorMax / 100f;
		}

		public override void Update()
		{
			base.Update();
			if (unLock && (!heroModel || heroModel.IsAlive))
			{
				if (timeTracking == 0f)
				{
					GetEnemies();
				}
				timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
			}
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		private void GetEnemies()
		{
			SingletonMonoBehaviour<GameData>.Instance.GetInRangeEnemies(heroModel.transform.position, skillRange, inRangeEnemies);
			timeTracking = timeToCheck;
			AddPassiveArmor();
		}

		private void AddPassiveArmor()
		{
			armorAmount = (float)inRangeEnemies.Count * armorPerUnit;
			armorAmount = Mathf.Clamp(armorAmount, 0f, armorMax);
			heroModel.HeroHealthController.CurrentPhysicsArmor = heroModel.HeroHealthController.OriginPhysicsArmor + armorAmount;
			heroModel.HeroHealthController.CurrentMagicArmor = heroModel.HeroHealthController.OriginMagicArmor + armorAmount;
		}
	}
}
