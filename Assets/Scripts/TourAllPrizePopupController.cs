using Gameplay;
using System.Collections.Generic;
using UnityEngine;

public class TourAllPrizePopupController : GameplayPopupController
{
	private List<TourAllPrizeLeagueEntry> leagueEntries = new List<TourAllPrizeLeagueEntry>();

	public TourAllPrizeLeagueEntry sampleEntry;

	public float entryHeight;

	public RectTransform scrollContent;

	public RectTransform scrollHandle;

	private bool isInited;

	public void Init(int curLeagueIndex)
	{
		OpenWithScaleAnimation();
		if (!isInited)
		{
			isInited = true;
			RectTransform rectTransform = scrollHandle;
			Vector2 offsetMin = scrollHandle.offsetMin;
			rectTransform.offsetMin = new Vector2(offsetMin.x, 0f);
			RectTransform rectTransform2 = scrollHandle;
			Vector2 offsetMax = scrollHandle.offsetMax;
			rectTransform2.offsetMax = new Vector2(offsetMax.x, 0f);
			int numberOfLeagues = GameTools.GetNumberOfLeagues();
			leagueEntries.Add(sampleEntry);
			for (int i = 1; i < numberOfLeagues; i++)
			{
				TourAllPrizeLeagueEntry tourAllPrizeLeagueEntry = Object.Instantiate(sampleEntry, sampleEntry.transform.parent);
				tourAllPrizeLeagueEntry.transform.localPosition = sampleEntry.transform.localPosition + new Vector3(0f, (float)(-i) * entryHeight, 0f);
				leagueEntries.Add(tourAllPrizeLeagueEntry);
			}
			for (int j = 0; j < numberOfLeagues; j++)
			{
				leagueEntries[j].Init(j);
			}
			RectTransform rectTransform3 = scrollContent;
			Vector2 sizeDelta = scrollContent.sizeDelta;
			rectTransform3.sizeDelta = new Vector2(sizeDelta.x, (float)numberOfLeagues * entryHeight + 50f);
		}
		Vector3 localPosition = scrollContent.localPosition;
		localPosition.y = entryHeight * (float)curLeagueIndex;
		scrollContent.localPosition = localPosition;
	}
}
