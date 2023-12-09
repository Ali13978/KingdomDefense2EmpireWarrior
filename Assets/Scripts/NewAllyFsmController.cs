using Gameplay;

public class NewAllyFsmController : EntityFSMController
{
	public NewAllyFsmController(CharacterModel heroModel)
	{
		AddState(EntityStateEnum.HeroIdle, new NewHeroIdleState(heroModel, this));
		AddState(EntityStateEnum.HeroWakeUp, new NewHeroWakeUpState(heroModel, this));
		AddState(EntityStateEnum.HeroRangeAtk, new NewHeroRangeAtkState(heroModel, this));
		AddState(EntityStateEnum.HeroMovetoTarget, new NewHeroMoveToTargetState(heroModel, this));
		AddState(EntityStateEnum.HeroMeleeAtk, new NewHeroMeleeAtkState(heroModel, this));
		AddState(EntityStateEnum.HeroIdleForAWhile, new NewHeroIdleForAwhileState(heroModel, this));
		AddState(EntityStateEnum.HeroMove, new NewHeroMoveState(heroModel, this));
		AddState(EntityStateEnum.HeroDie, new NewHeroDieState(heroModel, this, EntityStateEnum.HeroDestroy));
		AddState(EntityStateEnum.HeroDisappear, new NewHeroDisappearState(heroModel, this));
		AddState(EntityStateEnum.HeroDestroy, new NewHeroDisappearState(heroModel, this, isDiappearedIntermediate: true));
		AddState(EntityStateEnum.HeroSpecialState, new NewHeroSpecialState(heroModel, this));
		base.secondaryState = new NewEntityDetectEnemyState(heroModel, this);
		CreateTransition(EntityStateEnum.HeroIdle, EntityStateEnum.HeroRangeAtk);
		CreateTransition(EntityStateEnum.HeroIdle, EntityStateEnum.HeroMovetoTarget);
		CreateTransition(EntityStateEnum.HeroWakeUp, EntityStateEnum.HeroIdle);
		CreateTransition(EntityStateEnum.HeroWakeUp, EntityStateEnum.HeroRangeAtk);
		CreateTransition(EntityStateEnum.HeroWakeUp, EntityStateEnum.HeroMovetoTarget);
		CreateTransition(EntityStateEnum.HeroRangeAtk, EntityStateEnum.HeroMovetoTarget);
		CreateTransition(EntityStateEnum.HeroRangeAtk, EntityStateEnum.HeroIdleForAWhile);
		CreateTransition(EntityStateEnum.HeroMovetoTarget, EntityStateEnum.HeroMeleeAtk);
		CreateTransition(EntityStateEnum.HeroMovetoTarget, EntityStateEnum.HeroIdleForAWhile);
		CreateTransition(EntityStateEnum.HeroMeleeAtk, EntityStateEnum.HeroMovetoTarget);
		CreateTransition(EntityStateEnum.HeroMeleeAtk, EntityStateEnum.HeroIdleForAWhile);
		CreateTransition(EntityStateEnum.HeroIdleForAWhile, EntityStateEnum.HeroMovetoTarget);
		CreateTransition(EntityStateEnum.HeroIdleForAWhile, EntityStateEnum.HeroRangeAtk);
		CreateTransition(EntityStateEnum.HeroIdleForAWhile, EntityStateEnum.HeroMove);
		CreateTransition(EntityStateEnum.HeroMove, EntityStateEnum.HeroIdle);
		CreateTransition(EntityStateEnum.HeroMove, EntityStateEnum.HeroMove);
		CreateTransition(EntityStateEnum.HeroDie, EntityStateEnum.HeroDestroy);
		CreateTransition(EntityStateEnum.HeroSpecialState, EntityStateEnum.HeroIdle);
		CreateTransitionFromAllState(EntityStateEnum.HeroMove, EntityStateEnum.HeroDie, EntityStateEnum.HeroDestroy, EntityStateEnum.HeroDisappear);
		CreateTransitionFromAllState(EntityStateEnum.HeroDie, EntityStateEnum.HeroDisappear, EntityStateEnum.HeroDestroy);
		CreateTransitionFromAllState(EntityStateEnum.HeroDisappear, EntityStateEnum.HeroDestroy, EntityStateEnum.HeroDie);
		CreateTransitionFromAllState(EntityStateEnum.HeroSpecialState, EntityStateEnum.HeroDie, EntityStateEnum.HeroDisappear, EntityStateEnum.HeroDestroy);
		SetCurrentState(stateDictionary[EntityStateEnum.HeroWakeUp]);
	}
}
