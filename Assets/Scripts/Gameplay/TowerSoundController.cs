using Middle;
using UnityEngine;

namespace Gameplay
{
	public class TowerSoundController : TowerController
	{
		[SerializeField]
		private TowerAttackSingleTargetCommonController towerAttackSingleTargetCommonController;

		private AudioSource audioSourceNormal;

		private AudioSource audioSourceSkill;

		[Space]
		[SerializeField]
		private AudioClip attack;

		[Space]
		[SerializeField]
		private AudioClip hitEnemyWithArmor;

		[Space]
		[SerializeField]
		private AudioClip hitEnemyWithoutArmor;

		[Space]
		[SerializeField]
		private AudioClip explosion;

		[SerializeField]
		private AudioClip[] castSkill;

		private void Awake()
		{
			audioSourceNormal = GetComponent<AudioSource>();
			audioSourceSkill = GetComponentInChildren<AudioSource>();
			if ((bool)towerAttackSingleTargetCommonController)
			{
				towerAttackSingleTargetCommonController.OnFireBullet += TowerAttackSingleTargetCommonController_OnFireBullet;
			}
		}

		public override void Update()
		{
			base.Update();
			UpdateVolume();
		}

		private void OnDestroy()
		{
			if ((bool)towerAttackSingleTargetCommonController)
			{
				towerAttackSingleTargetCommonController.OnFireBullet -= TowerAttackSingleTargetCommonController_OnFireBullet;
			}
		}

		private void TowerAttackSingleTargetCommonController_OnFireBullet(BulletModel arg1, EnemyModel arg2)
		{
			PlayAttack();
		}

		private void UpdateVolume()
		{
			audioSourceNormal.volume = SingletonMonoBehaviour<UnitSoundManager>.Instance.ReadDataVolumeAdjust.GetVolume_GameplayEffect();
		}

		public void PlayAttack()
		{
			if (Config.Instance.Sound && (bool)attack)
			{
				audioSourceNormal.clip = attack;
				audioSourceNormal.Play();
			}
		}

		public void PlayHitEnemyWithArmor()
		{
			if (Config.Instance.Sound && (bool)hitEnemyWithArmor)
			{
				audioSourceNormal.clip = hitEnemyWithArmor;
				audioSourceNormal.Play();
			}
		}

		public void PlayHitEnemyWithoutArmor()
		{
			if (Config.Instance.Sound && (bool)hitEnemyWithoutArmor)
			{
				audioSourceNormal.clip = hitEnemyWithoutArmor;
				audioSourceNormal.Play();
			}
		}

		public void PlayExplosion()
		{
			if (Config.Instance.Sound && (bool)explosion)
			{
				audioSourceNormal.clip = explosion;
				audioSourceNormal.Play();
			}
		}

		public void PlayCastSkillSound(int index)
		{
			if (Config.Instance.Sound && castSkill.Length > index && castSkill[index] != null)
			{
				audioSourceSkill.clip = castSkill[index];
				audioSourceSkill.Play();
			}
		}
	}
}
