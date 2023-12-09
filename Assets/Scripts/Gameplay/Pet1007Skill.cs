namespace Gameplay
{
	public class Pet1007Skill : HeroSkillCommon
	{
		private HeroModel heroModel;

		private bool unLock;

		private HeroModel heroMaster;

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			unLock = true;
			heroMaster = heroModel.PetOwner;
			heroMaster.OnBeHitEvent += HeroModel_OnBeHitEvent;
		}

		private void HeroModel_OnBeHitEvent()
		{
			if ((bool)heroMaster.currentTarget)
			{
				heroModel.AddTarget(heroMaster.currentTarget);
				if ((bool)heroMaster.currentTarget.EnemyFindTargetController)
				{
					heroMaster.currentTarget.EnemyFindTargetController.AddTarget(heroModel);
				}
			}
		}
	}
}
