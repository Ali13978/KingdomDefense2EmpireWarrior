using UnityEngine;

public class ReadDataAdsReward : MonoBehaviour
{
	[SerializeField]
	private AdsReward adsReward;

	public int GetRewardValue(string productID)
	{
		int result = -1;
		foreach (AdReward item in adsReward.listReward)
		{
			if (item.rewardID.Equals(productID))
			{
				result = item.rewardValue;
			}
		}
		return result;
	}
}
