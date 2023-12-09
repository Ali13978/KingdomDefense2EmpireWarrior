using System;
using UnityEngine;

namespace Gameplay
{
	public class MapController : SingletonMonoBehaviour<MapController>
	{
		[SerializeField]
		private MapDestinationController[] listMapDestination;

		public event Action<Vector2> OnEnemyReachGate;

		public void DispatchEventEnemyReachGate(int gateID)
		{
			if (this.OnEnemyReachGate != null)
			{
				this.OnEnemyReachGate(listMapDestination[gateID].transform.position);
			}
		}
	}
}
