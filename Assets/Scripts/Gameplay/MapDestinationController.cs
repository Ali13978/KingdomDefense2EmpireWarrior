using GeneralVariable;
using UnityEngine;

namespace Gameplay
{
	public class MapDestinationController : MonoBehaviour
	{
		[SerializeField]
		private int gateID;

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.tag == GeneralVariable.GeneralVariable.ENEMY_TAG)
			{
				UnityEngine.Debug.Log("Enemy Reach Gate!");
				SingletonMonoBehaviour<MapController>.Instance.DispatchEventEnemyReachGate(gateID);
			}
		}
	}
}
