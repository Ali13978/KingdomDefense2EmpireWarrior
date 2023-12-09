using System;
using UnityEngine;

namespace Gameplay
{
	public class AlliesManager : SingletonMonoBehaviour<AlliesManager>
	{
		public event Action<int> onChooseAllies;

		public event Action<TowerModel, Vector2> onAlliesMoveToAssignedPosition;

		public void UnChooseTower(TowerModel towerModel)
		{
			if ((bool)towerModel)
			{
				towerModel.UnChooseTower();
			}
		}

		public void MoveAlliesToAssignedPosition(TowerModel towerModel, Vector3 targetPosition)
		{
			if (this.onAlliesMoveToAssignedPosition != null)
			{
				this.onAlliesMoveToAssignedPosition(towerModel, targetPosition);
			}
		}
	}
}
