using System.Collections.Generic;
using UnityEngine;

namespace DailyTrial
{
	public class MissionGroupController : MonoBehaviour
	{
		[SerializeField]
		private List<MissionController> listMission = new List<MissionController>();

		public void InitAllMissionsState()
		{
			foreach (MissionController item in listMission)
			{
				item.InitState();
			}
		}
	}
}
