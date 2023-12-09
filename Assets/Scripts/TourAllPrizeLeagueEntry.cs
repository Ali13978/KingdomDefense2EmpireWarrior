using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TourAllPrizeLeagueEntry : MonoBehaviour
{
	public Text leagueTitleText;

	public List<GameObject> leagueIcons;

	public List<TourRankPrizeManager> prizeBlocks;

	public void Init(int leagueIndex)
	{
		leagueTitleText.text = GameTools.GetLocalization("LEAGUE_" + leagueIndex);
		for (int num = leagueIcons.Count - 1; num >= 0; num--)
		{
			leagueIcons[num].SetActive(value: false);
		}
		leagueIcons[leagueIndex].SetActive(value: true);
		List<TournamentPrizeConfigData> leagueAllPrize = GameTools.GetLeagueAllPrize(leagueIndex);
		int count = leagueAllPrize.Count;
		for (int i = 0; i < count; i++)
		{
			prizeBlocks[count - i - 1].Init(leagueAllPrize[i].Rankrangeupper, leagueAllPrize[i].Rankrangelower, GameTools.GetTournamentRewardList(leagueAllPrize[i]));
		}
	}
}
