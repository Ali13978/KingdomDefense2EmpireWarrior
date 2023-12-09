using Gameplay;

public class PetFSMController : EntityFSMController
{
	private bool haveAttackStates;

	public PetFSMController(CharacterModel heroModel)
	{
		AddState(EntityStateEnum.HeroIdle, new PetStayAroundState(heroModel, this));
		AddState(EntityStateEnum.HeroSpecialState, new NewHeroSpecialState(heroModel, this));
		HeroModel heroModel2 = heroModel as HeroModel;
		haveAttackStates = GameTools.IsPetHavingAtkState(heroModel2);
		if (haveAttackStates)
		{
			AddState(EntityStateEnum.HeroRangeAtk, new NewHeroRangeAtkState(heroModel, this));
			AddState(EntityStateEnum.HeroMovetoTarget, new NewHeroMoveToTargetState(heroModel, this));
			AddState(EntityStateEnum.HeroMeleeAtk, new NewHeroMeleeAtkState(heroModel, this));
			AddState(EntityStateEnum.HeroIdleForAWhile, new NewHeroIdleForAwhileState(heroModel, this));
			base.secondaryState = new NewEntityDetectEnemyState(heroModel, this);
			base.secondaryState = new PetWatchStateOfOwnerBgState(heroModel, this);
			CreateTransition(EntityStateEnum.HeroIdle, EntityStateEnum.HeroRangeAtk);
			CreateTransition(EntityStateEnum.HeroIdle, EntityStateEnum.HeroMovetoTarget);
			CreateTransition(EntityStateEnum.HeroRangeAtk, EntityStateEnum.HeroMovetoTarget);
			CreateTransition(EntityStateEnum.HeroRangeAtk, EntityStateEnum.HeroIdleForAWhile);
			CreateTransition(EntityStateEnum.HeroMovetoTarget, EntityStateEnum.HeroMeleeAtk);
			CreateTransition(EntityStateEnum.HeroMovetoTarget, EntityStateEnum.HeroIdleForAWhile);
			CreateTransition(EntityStateEnum.HeroMeleeAtk, EntityStateEnum.HeroMovetoTarget);
			CreateTransition(EntityStateEnum.HeroMeleeAtk, EntityStateEnum.HeroIdleForAWhile);
			CreateTransition(EntityStateEnum.HeroMeleeAtk, EntityStateEnum.HeroRangeAtk);
			CreateTransition(EntityStateEnum.HeroIdleForAWhile, EntityStateEnum.HeroMovetoTarget);
			CreateTransition(EntityStateEnum.HeroIdleForAWhile, EntityStateEnum.HeroRangeAtk);
			CreateTransition(EntityStateEnum.HeroIdleForAWhile, EntityStateEnum.HeroIdle);
			AddState(EntityStateEnum.HeroMove, new PetMoveState(heroModel, this));
			CreateTransition(EntityStateEnum.HeroMove, EntityStateEnum.HeroIdle);
			CreateTransitionFromAllState(EntityStateEnum.HeroMove);
		}
		if (heroModel2.PetConfigData.Health > 0)
		{
			AddState(EntityStateEnum.HeroDie, new NewHeroDieState(heroModel, this, EntityStateEnum.HeroIdle));
			CreateTransitionFromAllState(EntityStateEnum.HeroDie);
			CreateTransition(EntityStateEnum.HeroDie, EntityStateEnum.HeroIdle);
		}
		CreateTransition(EntityStateEnum.HeroSpecialState, EntityStateEnum.HeroIdle);
		CreateTransitionFromAllState(EntityStateEnum.HeroSpecialState, EntityStateEnum.HeroDie);
		CreateTransition(EntityStateEnum.HeroSpecialState, EntityStateEnum.HeroSpecialState);
		SetCurrentState(stateDictionary[EntityStateEnum.HeroIdle]);
	}
}
