using SSR.Core.Architecture.WritableData;
using UnityEngine;

namespace Middle
{
	[CreateAssetMenu(fileName = "GlobalUpgradeProgress", menuName = "SSR/PlayerData/GlobalUpgradeProgress")]
	public class GlobalUpgradeProgress : WritableScriptableObject<GlobalUpgradeProgressData>
	{
		public void ResetData()
		{
			CopyDefaultDataToCurrentData();
			currentData.StarsSpent = 0;
		}
	}
}
