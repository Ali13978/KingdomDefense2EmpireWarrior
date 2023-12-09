using DG.Tweening;
using Parameter;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class AllyMovementController : AllyController
	{
		private List<string> increaseMovementSpeedBuffKeys = new List<string>
		{
			"IncreaseMovementSpeed"
		};

		public Vector3 assignedPosition;

		private Vector3 PoolPos = new Vector3(1000f, 1000f, 0f);

		private float moveSpeed;

		private TowerSpawnAllyController towerSpawnAllyController;

		private Tower originalParameter;

		public float MoveSpeed => moveSpeed;

		public override void OnAppear()
		{
			base.OnAppear();
			SetParameter();
			base.AllyModel.transform.DOKill();
			if (base.AllyModel.controlledAlly)
			{
				towerSpawnAllyController = base.AllyModel.TowerSpawnAllyController;
				originalParameter = base.AllyModel.TowerSpawnAllyController.TowerModel.OriginalParameter;
			}
			base.AllyModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		private void SetParameter()
		{
			moveSpeed = base.AllyModel.MoveSpeed;
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			base.AllyModel.transform.DOKill();
		}

		public void MoveToReadyPos(Vector3 end)
		{
			assignedPosition = end;
			base.AllyModel.GetFSMController().GetCurrentState().OnInput(StateInputType.UserAssignPosition, end);
		}

		public void MoveToReadyPosImmediately(Vector3 end, float time)
		{
			assignedPosition = end;
			base.AllyModel.GetFSMController().GetCurrentState().OnInput(StateInputType.UserAssignPosition, end);
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (increaseMovementSpeedBuffKeys.Contains(buffKey))
			{
				ApplyIncreaseMovementSpeed();
			}
		}

		private void ApplyIncreaseMovementSpeed()
		{
			float buffsValue = base.AllyModel.BuffsHolder.GetBuffsValue(increaseMovementSpeedBuffKeys);
			moveSpeed = base.AllyModel.MoveSpeed + (float)(int)(base.AllyModel.MoveSpeed * buffsValue / 100f);
		}
	}
}
