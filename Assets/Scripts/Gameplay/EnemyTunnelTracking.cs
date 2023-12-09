using GeneralVariable;
using UnityEngine;

namespace Gameplay
{
	public class EnemyTunnelTracking : EnemyController
	{
		[SerializeField]
		private float offset = 0.1f;

		private GameObject tunnelInPos;

		private GameObject tunnelOutPos;

		private void Start()
		{
			tunnelInPos = GameObject.FindGameObjectWithTag(GeneralVariable.GeneralVariable.TUNNEL_IN_NAME);
			tunnelOutPos = GameObject.FindGameObjectWithTag(GeneralVariable.GeneralVariable.TUNNEL_OUT_NAME);
		}

		public override void Update()
		{
			base.Update();
			if (!(tunnelInPos == null) && !(tunnelOutPos == null))
			{
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (GameTools.IsEnemyAbleToGoTunnel(base.EnemyModel))
			{
				if (other.CompareTag(GeneralVariable.GeneralVariable.TUNNEL_IN_NAME) && !base.EnemyModel.IsInTunnel)
				{
					OnGoIntoTunnel();
				}
				if (other.CompareTag(GeneralVariable.GeneralVariable.TUNNEL_OUT_NAME) && base.EnemyModel.IsInTunnel)
				{
					OnGoOutOfTunnel();
				}
			}
		}

		private void OnGoIntoTunnel()
		{
			if (!base.EnemyModel.IsInTunnel && GameTools.IsEnemyAbleToGoTunnel(base.EnemyModel))
			{
				base.EnemyModel.IsInTunnel = true;
				SingletonMonoBehaviour<GameData>.Instance.RemoveEnemyFromListActiveEnemy(base.EnemyModel);
				base.EnemyModel.EnemyEffectController.Hide();
				base.EnemyModel.EnemyHealthController.HideHealthBar();
			}
		}

		private void OnGoOutOfTunnel()
		{
			if (base.EnemyModel.IsInTunnel)
			{
				base.EnemyModel.IsInTunnel = false;
				SingletonMonoBehaviour<GameData>.Instance.AddEnemyToListActiveEnemy(base.EnemyModel);
			}
			base.EnemyModel.EnemyEffectController.Show();
		}
	}
}
