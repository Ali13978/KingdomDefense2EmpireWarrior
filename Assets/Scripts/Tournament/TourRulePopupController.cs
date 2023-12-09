using Gameplay;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tournament
{
	public class TourRulePopupController : GameplayPopupController
	{
		[Header("Rule content 2")]
		public Text friendRuleText;

		private void Start()
		{
			List<TournamentPrizeConfigData> leagueAllPrize = GameTools.GetLeagueAllPrize(1000);
			int num = leagueAllPrize[0].Itemquantities[0];
			friendRuleText.text = string.Format(GameTools.GetLocalization("RULE_CONTENT_2"), num);
		}
	}
}
