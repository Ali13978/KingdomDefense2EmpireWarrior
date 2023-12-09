using Gameplay;

public class HeroFsmController : EntityFSMController
{
	public HeroFsmController(CharacterModel heroModel)
	{
		AddState(EntityStateEnum.HeroIdle, new NewHeroIdleState(heroModel, this));
		AddState(EntityStateEnum.HeroRangeAtk, new NewHeroRangeAtkState(heroModel, this));
		AddState(EntityStateEnum.HeroMovetoTarget, new NewHeroMoveToTargetState(heroModel, this));
		AddState(EntityStateEnum.HeroMeleeAtk, new NewHeroMeleeAtkState(heroModel, this));
		AddState(EntityStateEnum.HeroIdleForAWhile, new NewHeroIdleForAwhileState(heroModel, this));
		AddState(EntityStateEnum.HeroMove, new NewHeroMoveState(heroModel, this));
		AddState(EntityStateEnum.HeroDie, new NewHeroDieState(heroModel, this, EntityStateEnum.HeroIdle));
		AddState(EntityStateEnum.HeroSpecialState, new NewHeroSpecialState(heroModel, this));
		base.secondaryState = new NewEntityDetectEnemyState(heroModel, this);
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
		CreateTransition(EntityStateEnum.HeroIdleForAWhile, EntityStateEnum.HeroMove);
		CreateTransition(EntityStateEnum.HeroMove, EntityStateEnum.HeroIdle);
		CreateTransition(EntityStateEnum.HeroMove, EntityStateEnum.HeroMove);
		CreateTransition(EntityStateEnum.HeroDie, EntityStateEnum.HeroIdle);
		CreateTransition(EntityStateEnum.HeroSpecialState, EntityStateEnum.HeroIdle);
		CreateTransitionFromAllState(EntityStateEnum.HeroMove, EntityStateEnum.HeroDie, EntityStateEnum.HeroSpecialState);
		CreateTransitionFromAllState(EntityStateEnum.HeroDie);
		CreateTransitionFromAllState(EntityStateEnum.HeroSpecialState, EntityStateEnum.HeroDie);
		SetCurrentState(stateDictionary[EntityStateEnum.HeroIdle]);
	}
}
