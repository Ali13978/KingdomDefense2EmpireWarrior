using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class TowerSkillScaleDamageByRange : TowerController
	{
		[SerializeField]
		private int towerID;

		[SerializeField]
		private int towerLevel;

		private TowerDefaultSkill param;

		private float damageScaleMin;

		private float damageScaleMax;

		public float DamageScaleMin
		{
			get
			{
				return damageScaleMin;
			}
			private set
			{
				damageScaleMin = value;
			}
		}

		public float DamageScaleMax
		{
			get
			{
				return damageScaleMax;
			}
			private set
			{
				damageScaleMax = value;
			}
		}

		public override void OnAppear()
		{
			base.OnAppear();
			SetParameter();
		}

		private void SetParameter()
		{
			param = TowerDefaultSkillParameter.Instance.GetTowerParameter(towerID, towerLevel);
			DamageScaleMin = (float)param.skillParam0 / 100f;
			DamageScaleMax = (float)param.skillParam1 / 100f;
		}

		public int GetScaledDamage(int originDamage, float towerMaxRange, EnemyModel target)
		{
			int num = -1;
			float num2 = Vector3.Distance(base.gameObject.transform.position, target.transform.position);
			float num3 = num2 / towerMaxRange;
			float num4 = DamageScaleMin + (DamageScaleMax - DamageScaleMin) * num3;
			return (int)((float)originDamage * num4);
		}
	}
}
