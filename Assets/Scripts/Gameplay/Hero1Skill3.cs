using Data;
using Parameter;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero1Skill3 : HeroSkillCommon
	{
		private int heroID = 1;

		private int skillID = 3;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int changeToCast;

		private int numberOfProjectile;

		private int damage;

		private string buffkey = "Slow";

		private int slowPecent;

		private float slowTime;

		[SerializeField]
		private Transform gunPos;

		private EffectAttack effectAttackSender;

		private List<EnemyModel> inRangeEnemies = new List<EnemyModel>();

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_1_3 heroSkillParameter_1_ = new HeroSkillParameter_1_3();
			heroSkillParameter_1_ = (HeroSkillParameter_1_3)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			changeToCast = heroSkillParameter_1_.getParam(currentSkillLevel - 1).chance_to_cast;
			numberOfProjectile = heroSkillParameter_1_.getParam(currentSkillLevel - 1).number_of_projectile;
			damage = heroSkillParameter_1_.getParam(currentSkillLevel - 1).damage;
			slowPecent = heroSkillParameter_1_.getParam(currentSkillLevel - 1).slow_percent;
			slowTime = (float)heroSkillParameter_1_.getParam(currentSkillLevel - 1).slow_time / 1000f;
			effectAttackSender.buffKey = buffkey;
			effectAttackSender.debuffChance = 100;
			effectAttackSender.debuffEffectValue = slowPecent;
			effectAttackSender.debuffEffectDuration = slowTime;
			effectAttackSender.damageFXType = DamageFXType.Slow;
			SpawnBullet instance = SingletonMonoBehaviour<SpawnBullet>.Instance;
			Hero originalParameter = heroModel.OriginalParameter;
			instance.InitBulletsFromHeroes(originalParameter.id, 1);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_SLOW);
			heroModel.OnAttackEvent += HeroModel_OnAttackEvent;
		}

		private void HeroModel_OnAttackEvent()
		{
			if (unLock)
			{
				TryCastMultiProjectile();
			}
		}

		private void TryCastMultiProjectile()
		{
			if (Random.Range(0, 100) < changeToCast)
			{
				GetEnemies();
				int num = Mathf.Min(numberOfProjectile, inRangeEnemies.Count);
				UnityEngine.Debug.Log("Cast multi arrow on " + num + " enemy ");
				for (int i = 0; i < num; i++)
				{
					BulletModel bulletModel = null;
					SpawnBullet instance = SingletonMonoBehaviour<SpawnBullet>.Instance;
					Hero originalParameter = heroModel.OriginalParameter;
					bulletModel = instance.GetForHero(originalParameter.id, 1);
					bulletModel.transform.eulerAngles = Vector3.zero;
					int num2 = damage;
					bulletModel.transform.position = gunPos.position;
					bulletModel.gameObject.SetActive(value: true);
					bulletModel.InitFromHero(heroModel, new CommonAttackDamage(damage, 0), inRangeEnemies[i], effectAttackSender);
				}
			}
		}

		private void GetEnemies()
		{
			GameData instance = SingletonMonoBehaviour<GameData>.Instance;
			Vector2 centerPoint = heroModel.transform.position;
			Hero originalParameter = heroModel.OriginalParameter;
			instance.GetInRangeEnemies(centerPoint, (float)originalParameter.attack_range_max / GameData.PIXEL_PER_UNIT, inRangeEnemies);
		}
	}
}
