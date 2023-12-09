using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class BossW3_Spawn_P0 : BossW3BaseState
	{
		public BossW3_Spawn_P0(LogicEnemyBossW3 logicEnemy)
			: base(logicEnemy)
		{
			enemyModel.SetSpecialStateDuration(999999f);
			enemyModel.GetFSMController().GetCurrentState().SetTransition(EntityStateEnum.EnemySpecialState);
			logicEnemy.StartCoroutine(SpawnSequence());
		}

		private IEnumerator SpawnSequence()
		{
			BossW3StatueManager.instance.BreakIce();
			yield return new WaitForSeconds(1.5f);
			BossW3StatueManager.instance.iceAnimator.gameObject.SetActive(value: false);
			BossW3StatueManager.instance.bossStatue.SetActive(value: false);
			enemyModel.transform.position = BossW3StatueManager.instance.bossStatue.transform.position;
			yield return new WaitForSeconds(2f);
			SingletonMonoBehaviour<CameraController>.Instance.PinchZoomFov.MoveAndZoomToPosition(enemyModel.transform.position, 2f, 3.9f);
			logicEnemy.curState = new BossW3_Teleport_P3(logicEnemy, 0.55f, 0.7f);
		}
	}
}
