using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TourRankEntryManager : MonoBehaviour
{
	public Text rankText;

	public Text userNameText;

	public Text timeText;

	public List<Image> heroIcons;

	public GameObject highlightBg;

	public Image flagImage;

	public void Init(TourPlayerInfo playerInfo)
	{
		highlightBg.SetActive(playerInfo.isYou);
		rankText.text = (playerInfo.rank + 1).ToString();
		userNameText.text = playerInfo.name;
		for (int i = playerInfo.heroIds.Count; i < heroIcons.Count; i++)
		{
			heroIcons[i].gameObject.SetActive(value: false);
		}
		int num = Mathf.Min(heroIcons.Count, playerInfo.heroIds.Count);
		for (int j = 0; j < num; j++)
		{
			heroIcons[j].gameObject.SetActive(value: true);
			GameTools.SetRewardSprite(new RewardItem(RewardType.SingleHero, playerInfo.heroIds[j], 1, isDisplayQuantity: false), heroIcons[j]);
		}
		if (flagImage != null)
		{
			flagImage.sprite = Resources.Load<Sprite>($"CountryFlags2/{playerInfo.countryCode}");
		}
		timeText.text = $"{(int)playerInfo.time.TotalMinutes}:{playerInfo.time.Seconds:00}.{playerInfo.time.Milliseconds:000}";
	}
}
