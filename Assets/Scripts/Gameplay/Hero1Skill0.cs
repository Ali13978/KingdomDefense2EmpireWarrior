using Data;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero1Skill0 : HeroSkillCommon
	{
		private int heroID = 1;

		private int skillID;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private Hero heroParameter;

		private int numberOfProjectile;

		private float range;

		private int offsetHigh;

		private float duration;

		private float delayTime;

		private int damage;

		private float cooldownTime;

		private string description;

		private string useType;

		private RaycastHit2D hit;

		[SerializeField]
		private LayerMask avaiableCastSkillLayerMask;

		[SerializeField]
		private Hero1Skill0Projectile projectilePrefab;

		private void Start()
		{
			HeroesManager.Instance.onCastHeroSkillToAssignedPosition += Instance_onCastHeroSkillToAssignedPosition;
		}

		private void OnDestroy()
		{
			HeroesManager.Instance.onCastHeroSkillToAssignedPosition -= Instance_onCastHeroSkillToAssignedPosition;
		}

		private void Instance_onCastHeroSkillToAssignedPosition(int heroID, Vector2 targetPosition)
		{
			if (this.heroID == heroID)
			{
				StartCoroutine(CastSkill(targetPosition));
			}
		}

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			heroParameter = HeroParameter.Instance.GetHeroParameter(heroID, currentLevel);
			HeroSkillParameter_1_0 heroSkillParameter_1_ = new HeroSkillParameter_1_0();
			heroSkillParameter_1_ = (HeroSkillParameter_1_0)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			numberOfProjectile = heroSkillParameter_1_.getParam(currentSkillLevel - 1).number_of_projectile;
			range = (float)heroSkillParameter_1_.getParam(currentSkillLevel - 1).range / GameData.PIXEL_PER_UNIT;
			offsetHigh = heroSkillParameter_1_.getParam(currentSkillLevel - 1).offsetHigh;
			duration = (float)heroSkillParameter_1_.getParam(currentSkillLevel - 1).duration / 1000f;
			delayTime = (float)heroSkillParameter_1_.getParam(currentSkillLevel - 1).delayTime / 1000f;
			damage = heroSkillParameter_1_.getParam(currentSkillLevel - 1).damage;
			cooldownTime = (float)heroSkillParameter_1_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_1_.getParam(currentSkillLevel - 1).description;
			useType = heroSkillParameter_1_.getParam(currentSkillLevel - 1).use_type;
			InitFxs();
		}

		public override float GetCooldownTime()
		{
			return cooldownTime;
		}

		public override string GetUseType()
		{
			return useType;
		}

		public void Init(Hero _heroParameter, int _numberOfArrow, int _range, int _offsetHigh, int _duration, int _delayTime, int _damage, int _cooldownTime, string _descsription)
		{
			heroParameter = _heroParameter;
			numberOfProjectile = _numberOfArrow;
			range = (float)_range / GameData.PIXEL_PER_UNIT;
			offsetHigh = _offsetHigh;
			duration = (float)_duration / 1000f;
			delayTime = (float)_delayTime / 1000f;
			damage = _damage;
			cooldownTime = _cooldownTime;
			description = _descsription;
		}

		private void InitFxs()
		{
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(projectilePrefab.gameObject);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.LIGHTNING_PROJECTILE_SHADOW);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.LIGHTNING_PROJECTILE_RANGE);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.LIGHTNING_EXPLOSION);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_FADE_SCREEN);
		}

		private IEnumerator CastSkill(Vector2 targetPosition)
		{
			CastEffectSkillRange(targetPosition);
			SingletonMonoBehaviour<GameplayUIHeroManager>.Instance.listSelectHeroSkillButton[1].DoCooldown();
			EffectController fxCanvas = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_FADE_SCREEN);
			fxCanvas.transform.position = Vector3.zero;
			float fadeTime = duration * ((float)offsetHigh - targetPosition.y) + (float)numberOfProjectile * delayTime;
			fxCanvas.Init(1.5f * fadeTime);
			fxCanvas.DoFadeIn(fadeTime / 2f, 20f / 51f);
			for (int i = 0; i < numberOfProjectile; i++)
			{
				yield return new WaitForSeconds(delayTime);
				Hero1Skill0Projectile bullet = SingletonMonoBehaviour<SpawnBullet>.Instance.GetLightningBullet();
				bullet.transform.position = new Vector2(targetPosition.x, offsetHigh) + Random.insideUnitCircle * range;
				bullet.Init(damage, range, duration * ((float)offsetHigh - targetPosition.y), (float)offsetHigh - targetPosition.y);
			}
			yield return new WaitForSeconds(fadeTime / 4f);
			fxCanvas.DoFadeOut(fadeTime / 4f, 20f / 51f);
		}

		private void CastEffectSkillRange(Vector2 targetPosition)
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.LIGHTNING_PROJECTILE_RANGE);
			effect.transform.position = targetPosition;
			effect.Init(0.75f);
		}
	}
}
