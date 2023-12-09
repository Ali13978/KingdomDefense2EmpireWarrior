namespace Gameplay
{
	public class Pet1000Skill : HeroSkillCommon
	{
		private float atkbuffPercentage;

		public override void Init(HeroModel heroModel)
		{
			PetConfigData petConfigData = heroModel.PetConfigData;
			atkbuffPercentage = petConfigData.Skillvalues[0];
			HeroModel petOwner = heroModel.PetOwner;
			petOwner.BuffsHolder.AddBuff("BuffAttackByPercentage", new Buff(isPositive: true, atkbuffPercentage, 999999f), BuffStackLogic.StackUp, BuffStackLogic.ChooseMax);
		}
	}
}
