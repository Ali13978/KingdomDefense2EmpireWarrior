using UnityEngine;

namespace Gameplay
{
	public class Pet1005Skill : HeroSkillCommon
	{
		private HeroModel heroModel;

		private bool unLock;

		private int chanceToStun;

		private float duration;

		private string buffKey = "Slow";

		private float armorBuffPercentage;

		private float armorBonus;

		public override void Init(HeroModel heroModel)
		{
			this.heroModel = heroModel;
			unLock = true;
			PetConfigData petConfigData = heroModel.PetConfigData;
			chanceToStun = petConfigData.Skillvalues[0];
			duration = (float)petConfigData.Skillvalues[1] / 1000f;
			armorBuffPercentage = petConfigData.Skillvalues[2];
			armorBonus = (float)petConfigData.Skillvalues[3] / 100f;
			heroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
			InitFXs();
			CastBuffToOwner();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_STUN);
		}

		private void CastBuffToOwner()
		{
			HeroModel petOwner = heroModel.PetOwner;
			petOwner.BuffsHolder.AddBuff("IncreaseArmorPhysics", new Buff(isPositive: true, armorBonus, 999999f), BuffStackLogic.StackUp, BuffStackLogic.ChooseMax);
			petOwner.BuffsHolder.AddBuff("IncreaseArmorMagic", new Buff(isPositive: true, armorBonus, 999999f), BuffStackLogic.StackUp, BuffStackLogic.ChooseMax);
		}

		private void HeroModel_OnHitEnemyEvent()
		{
			if (unLock && (bool)heroModel.currentTarget && Random.Range(0, 100) < chanceToStun)
			{
				heroModel.currentTarget.ProcessEffect(buffKey, 100, duration, DamageFXType.Stun);
			}
		}
	}
}
