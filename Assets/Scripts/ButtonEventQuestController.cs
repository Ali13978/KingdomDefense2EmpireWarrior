using UnityEngine;
using WorldMap;

public class ButtonEventQuestController : ButtonController
{
	public GameObject notifyObj;

	private void Start()
	{
		bool active = false;
		if (EventQuestSystem.Instance.unclaimedRewardList.Count > 0)
		{
			active = true;
		}
		notifyObj.SetActive(active);
	}

	public override void OnClick()
	{
		base.OnClick();
		OpenEventQuestPanel();
		notifyObj.SetActive(value: false);
	}

	private void OpenEventQuestPanel()
	{
		SingletonMonoBehaviour<UIRootController>.Instance.eventQuestPopup.Init();
	}
}
