using System;
using System.Collections.Generic;
using UnityEngine;

namespace Middle
{
	[Serializable]
	public class GlobalUpgradeProgressData
	{
		[SerializeField]
		private List<TierUpgradeStatus> tiers;

		[SerializeField]
		private int starsSpent;

		public List<TierUpgradeStatus> Tiers => tiers;

		public int StarsSpent
		{
			get
			{
				return starsSpent;
			}
			set
			{
				starsSpent = value;
				if (GlobalUpgradeProgressData.OnStarsSpentChanged != null)
				{
					GlobalUpgradeProgressData.OnStarsSpentChanged();
				}
			}
		}

		public static event Action OnStarsSpentChanged;
	}
}
