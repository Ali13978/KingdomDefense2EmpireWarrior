using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class TowerAnimationController : TowerController
	{
		[SerializeField]
		private List<Animator> listAnim = new List<Animator>();

		[SerializeField]
		private List<GameObject> listSprite = new List<GameObject>();

		private EnemyModel target;

		private TowerFindEnemyController towerFindEnemyController;

		private Vector3 localScale = Vector3.one;

		public override void Initialize()
		{
			base.Initialize();
			towerFindEnemyController = base.TowerModel.towerFindEnemyController;
		}

		public void TurnAround()
		{
			List<EnemyModel> targets = towerFindEnemyController.Targets;
			if (targets.Count > 0)
			{
				target = targets[0];
			}
			localScale.x = -GetDirection(target.gameObject);
			foreach (GameObject item in listSprite)
			{
				item.transform.localScale = localScale;
			}
		}

		private int GetDirection(GameObject target)
		{
			Vector3 position = target.transform.position;
			float x = position.x;
			Vector3 position2 = base.gameObject.transform.position;
			float num = x - position2.x;
			num = ((!(num > 0f)) ? (-1f) : 1f);
			return (int)num;
		}

		public void PlayAnimAttack(int index)
		{
			listAnim[index].Play("Attack");
		}
	}
}
