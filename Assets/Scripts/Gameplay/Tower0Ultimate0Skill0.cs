using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Tower0Ultimate0Skill0 : TowerUltimateCommon
	{
		private int towerID;

		private int ultimateBranch;

		private int skillID;

		private int chanceToCastSkill;

		private int numberOfArrow;

		private int damagePerArrow;

		private float skillRange;

		[SerializeField]
		private float delayTime;

		[SerializeField]
		private float duration;

		[SerializeField]
		private GameObject arrowPrefab;

		[Space]
		[SerializeField]
		private float offsetX;

		[Space]
		[SerializeField]
		private float offsetY;

		private Vector2 offset;

		private TowerModel towerModel;

		private void Start()
		{
			offset.Set(offsetX, offsetY);
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
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(arrowPrefab.gameObject);
			CastSkill();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void ReadParameter(int currentSkillLevel)
		{
			chanceToCastSkill = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			numberOfArrow = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			damagePerArrow = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 2);
			skillRange = (float)TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 3) / GameData.PIXEL_PER_UNIT;
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.LIGHTNING_PROJECTILE_RANGE);
		}

		public void TryToCastRainOfArrow()
		{
			if (unlock && chanceToCastSkill > 0 && Random.Range(0, 100) < chanceToCastSkill)
			{
				CastSkill();
			}
		}

		private void CastSkill()
		{
			if (towerModel.towerFindEnemyController.Targets.Count > 0)
			{
				UnityEngine.Debug.Log("Cast skill rain of arrow!");
				EnemyModel enemyModel = towerModel.towerFindEnemyController.Targets[0];
				CastEffectSkillRange(enemyModel.transform.position);
				StartCoroutine(CastSkill(enemyModel.transform.position));
			}
		}

		private IEnumerator CastSkill(Vector2 targetPosition)
		{
			for (int i = 0; i < numberOfArrow; i++)
			{
				Tower0Ultimate0Bullet bullet = SingletonMonoBehaviour<SpawnBullet>.Instance.GetTower0Ultimate0Bullet();
				bullet.transform.position = targetPosition + offset + Random.insideUnitCircle * skillRange;
				bullet.Init(targerPosition: (Vector3)((Vector2)bullet.transform.position - offset), _damage: damagePerArrow, _lifeTime: duration);
				yield return new WaitForSeconds(delayTime);
			}
		}

		private void CastEffectSkillRange(Vector2 targetPosition)
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.LIGHTNING_PROJECTILE_RANGE);
			effect.transform.position = targetPosition;
			effect.Init(0.75f);
		}
	}
}
