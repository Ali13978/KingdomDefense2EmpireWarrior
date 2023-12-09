using Gameplay;
using UnityEngine;

public class PetStayAroundState : NewHeroState
{
	private INewEntityState curSubState;

	private INewEntityState followState;

	private INewEntityState idleState;

	private HeroModel petModel;

	private float minDelayChangeSpot = 3.5f;

	private float maxDelayChangeSpot = 7f;

	private float disToOwner = 0.8f;

	private float countdown;

	private float countdownFindTarget;

	public Vector3 ownerToIdleSpotOffset;

	public PetStayAroundState(CharacterModel heroModel, INewFSMController fSMController)
		: base(heroModel, fSMController)
	{
		followState = new PetFollowState(heroModel, fSMController, this);
		idleState = new PetIdleState(heroModel, fSMController, this);
		petModel = (heroModel as HeroModel);
		ownerToIdleSpotOffset = GetRandomPosAroundOwner();
	}

	public override void OnStartState()
	{
		base.OnStartState();
		countdown = Random.Range(minDelayChangeSpot, maxDelayChangeSpot);
		curSubState = null;
		SetSubState(idleState);
		heroModel.AddTarget(null);
		if (petModel.PetConfigData.Atk_magic_min > 0 || petModel.PetConfigData.Atk_physics_min > 0)
		{
			countdownFindTarget = 0f;
		}
		else
		{
			countdownFindTarget = 1E+09f;
		}
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		countdown -= dt;
		if (countdown <= 0f)
		{
			countdown = Random.Range(minDelayChangeSpot, maxDelayChangeSpot);
			ownerToIdleSpotOffset = GetRandomPosAroundOwner();
		}
		curSubState.Update(dt);
		countdownFindTarget -= dt;
		if (!(countdownFindTarget <= 0f))
		{
			return;
		}
		countdownFindTarget = 1f;
		if (!IsOwnerMoving())
		{
			EnemyModel enemyModel = FindTargetInRange();
			if (enemyModel != null)
			{
				OnInput(StateInputType.MonsterInAtkRange, enemyModel);
			}
		}
	}

	public void SetSubState(INewEntityState state)
	{
		if (curSubState != state)
		{
			if (curSubState != null)
			{
				curSubState.OnExitState();
			}
			curSubState = state;
			curSubState.OnStartState();
		}
	}

	public override void OnInput(StateInputType inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		switch (inputType)
		{
		case StateInputType.PetReachFollowSpot:
			SetSubState(idleState);
			break;
		case StateInputType.PetFarFromOwner:
			SetSubState(followState);
			break;
		case StateInputType.MonsterInAtkRange:
			if (!IsOwnerMoving())
			{
				EnemyModel enemy = (EnemyModel)args[0];
				if (heroModel.IsInMeleeRange(enemy))
				{
					heroModel.AddTarget(enemy);
					SetTransition(EntityStateEnum.HeroMovetoTarget);
				}
				else if (heroModel.IsInRangerRange(enemy))
				{
					heroModel.AddTarget(enemy);
					SetTransition(EntityStateEnum.HeroRangeAtk);
				}
			}
			break;
		}
	}

	public Vector3 GetRandomPosAroundOwner()
	{
		Vector2 insideUnitCircle = Random.insideUnitCircle;
		insideUnitCircle = insideUnitCircle / insideUnitCircle.magnitude * disToOwner;
		return insideUnitCircle;
	}

	public bool IsOwnerMoving()
	{
		return petModel.PetOwner.IsAlive && petModel.PetOwner.GetFSMController().GetCurrentState() is NewHeroMoveState;
	}
}
