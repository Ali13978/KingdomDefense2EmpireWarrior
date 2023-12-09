using Gameplay;
using Parameter;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyMoveState : NewEnemyState
{
	private bool haveRestOnTheWay;

	private float delayRest;

	private float countdownToRest;

	private bool isRanger;

	private float delayFindTarget = 0.12f;

	private float countdownFindTarget;

	private float rangerRange;

	private float sqRangerRange;

	private Vector3 invertXVector = new Vector3(-1f, 1f, 1f);

	public NewEnemyMoveState(EnemyModel enemyModel, INewFSMController fSMController)
		: base(enemyModel, fSMController)
	{
		haveRestOnTheWay = enemyModel.EnemyMovementController.HaveRestOnTheWay;
		delayRest = enemyModel.EnemyMovementController.DelayToRest;
		countdownToRest = delayRest;
		if ((bool)enemyModel.enemyAttackController)
		{
			isRanger = enemyModel.enemyAttackController.rangeAttack;
		}
		if (isRanger)
		{
			countdownFindTarget = delayFindTarget;
			rangerRange = enemyModel.enemyAttackController.GetRangerAtkRange();
			sqRangerRange = rangerRange * rangerRange;
		}
		LineData line = LineManager.Current.GetLine(enemyModel.Gate, 0);
		enemyModel.transform.position = line.Position;
	}

	public override void OnStartState()
	{
		base.OnStartState();
		if (enemyModel.monsterPathData == null)
		{
			int moveLine = enemyModel.moveLine;
			Enemy originalParameter = enemyModel.OriginalParameter;
			int lineIndex;
			if (originalParameter.isBoss)
			{
				lineIndex = LineManager.Current.GetLineIndex(enemyModel.Gate, 0);
				enemyModel.EnemyMovementController.currentLine = 0;
			}
			else
			{
				lineIndex = LineManager.Current.GetLineIndex(enemyModel.Gate, moveLine);
				enemyModel.EnemyMovementController.currentLine = moveLine;
			}
			enemyModel.monsterPathData = new MonsterPathData(lineIndex, OnMoveToEndPoint);
		}
		if (GameTools.IsUnderTargetOfAnyHero(enemyModel))
		{
			SetTransition(EntityStateEnum.EnemyWaitForAtk);
			return;
		}
		countdownFindTarget = 0f;
		enemyModel.EnemyAnimationController.ToRunState(EnemyAnimationController.animRunRight);
	}

	public override void OnInput(StateInputType inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		switch (inputType)
		{
		case StateInputType.SetEnemyIdleWaitForMeleeAtk:
		{
			CharacterModel target = (CharacterModel)args[0];
			if (enemyModel.EnemyFindTargetController != null)
			{
				enemyModel.EnemyFindTargetController.Target = target;
			}
			SetTransition(EntityStateEnum.EnemyWaitForAtk);
			break;
		}
		case StateInputType.Die:
			enemyModel.monsterPathData = null;
			break;
		}
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		if (haveRestOnTheWay)
		{
			countdownToRest -= dt;
			if (countdownToRest <= 0f)
			{
				countdownToRest += delayRest;
				SetTransition(EntityStateEnum.EnemyRest);
			}
		}
		if (isRanger && !enemyModel.IsInTunnel)
		{
			countdownFindTarget -= dt;
			if (countdownFindTarget <= 0f)
			{
				countdownFindTarget = delayFindTarget;
				CharacterModel characterModel = FindTarget();
				if (characterModel != null)
				{
					enemyModel.EnemyFindTargetController.Target = characterModel;
					SetTransition(EntityStateEnum.EnemyRangerAtk);
				}
			}
		}
		LineManager.Current.RequestMove(enemyModel, enemyModel.monsterPathData, enemyModel.EnemyMovementController.Speed * dt);
		ChangeAnimationRun();
	}

	public void OnMoveToEndPoint()
	{
		GameEventCenter.Instance.Trigger(GameEventType.OnEnemyMoveToEndPoint, enemyModel);
	}

	public CharacterModel FindTarget()
	{
		List<CharacterModel> listActiveAlly = SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly;
		for (int num = listActiveAlly.Count - 1; num >= 0; num--)
		{
			if (GameTools.IsValidCharacter(listActiveAlly[num]) && (listActiveAlly[num].transform.position - enemyModel.transform.position).sqrMagnitude <= sqRangerRange && GameTools.IsCharacterVisible(listActiveAlly[num]))
			{
				return listActiveAlly[num];
			}
		}
		return null;
	}

	private void ChangeAnimationRun()
	{
		Vector3 from = enemyModel.transform.position - enemyModel.CachedPosition;
		if (enemyModel.IsUnderground)
		{
			enemyModel.EnemyAnimationController.ToRunState(EnemyAnimationController.animRunUnderground);
			return;
		}
		float num = Vector3.Angle(from, Vector3.right);
		if ((num > 0f && num < 60f) || (num > 300f && num < 360f))
		{
			enemyModel.EnemyAnimationController.ToRunState(EnemyAnimationController.animRunRight);
			enemyModel.transform.localScale = Vector3.one;
		}
		else if (num > 120f && num < 240f)
		{
			enemyModel.EnemyAnimationController.ToRunState(EnemyAnimationController.animRunRight);
			enemyModel.transform.localScale = invertXVector;
		}
		else if (num > 60f && num < 120f && from.y > 0f)
		{
			enemyModel.EnemyAnimationController.ToRunState(EnemyAnimationController.animRunUp);
		}
		else if (num > 60f && num < 120f && from.y < 0f)
		{
			enemyModel.EnemyAnimationController.ToRunState(EnemyAnimationController.animRunDown);
		}
	}
}
