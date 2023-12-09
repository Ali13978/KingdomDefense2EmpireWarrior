using Parameter;
using RewardPopup;
using TMPro;
using UnityEngine;

public class RewardPriorityPopupController : GameplayPriorityPopupController
{
	[Header("Controllers")]
	public GeneralItemGroupController generalItemGroupController;

	public TextMeshProUGUI title;

	public int defaultLocalId = 32;

	public void SetRewardData(RewardItem[] listData, string customTitle = null)
	{
		base.transform.SetAsLastSibling();
		generalItemGroupController.InitListItems(listData);
		if (!string.IsNullOrEmpty(customTitle))
		{
			title.text = customTitle;
		}
		else
		{
			title.text = Singleton<NotificationDescription>.Instance.GetNotiContent(defaultLocalId).Replace('@', '\n').Replace('#', '-');
		}
	}
}
