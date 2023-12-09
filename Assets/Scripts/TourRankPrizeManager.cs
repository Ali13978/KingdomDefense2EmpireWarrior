using RewardPopup;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TourRankPrizeManager : MonoBehaviour
{
	public Text rangeOfRankText;

	public List<GeneralItem> rewardEntries;

	public void Init(int upperRank, int lowerRank, List<RewardItem> rewards)
	{
		rangeOfRankText.text = string.Format("{0} {1}-{2}", GameTools.GetLocalization("RANK"), upperRank, lowerRank);
		int num = Mathf.Min(rewards.Count, rewardEntries.Count);
		for (int i = 0; i < num; i++)
		{
			rewardEntries[i].gameObject.SetActive(value: true);
			rewardEntries[i].Init(rewards[i]);
		}
		for (int j = num; j < rewardEntries.Count; j++)
		{
			rewardEntries[j].gameObject.SetActive(value: false);
		}
	}
}
