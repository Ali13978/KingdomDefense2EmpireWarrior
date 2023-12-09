using UnityEngine;

public class ReadDataFacebookServicesReward : MonoBehaviour
{
	[SerializeField]
	private FacebookServicesReward facebookServicesReward;

	public int GetRewardAmount_Gem(string productID)
	{
		int result = -1;
		foreach (FBSReward item in facebookServicesReward.listReward)
		{
			if (item.rewardID.Equals(productID))
			{
				result = item.rewardAmount_Gem;
			}
		}
		return result;
	}
}
