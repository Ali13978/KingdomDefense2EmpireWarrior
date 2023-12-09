using DailyReward;
using LifetimePopup;
using System.Collections.Generic;
using UnityEngine;

public class PriorityPopupManager : MonoBehaviour
{
	[Space]
	[Header("UI parent")]
	public Transform popupParent;

	[Header("Priority popup prefab")]
	public RewardPriorityPopupController rewardPopupPrefab;

	public SpecialOfferPriorityPopup offerStarterPopup;

	public SpecialOfferPriorityPopup offerTrioPopup;

	public SpecialOfferPriorityPopup offerLandskyPopup;

	public SpecialOfferPriorityPopup offerTwoGodPopup;

	public SpecialOfferPriorityPopup dailyBoosterPopup;

	public DailyRewardPopupController dailyRewardPopupPrefab;

	public AskToRatePopupController ratePopupPrefab;

	public static PriorityPopupManager Instance;

	private List<GameplayPriorityPopupController> popupQueue = new List<GameplayPriorityPopupController>();

	private GameplayPriorityPopupController currentPopup;

	private void Awake()
	{
		if (Instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		Initialization();
	}

	private void Initialization()
	{
	}

	public GameplayPriorityPopupController CreatePopup(GameplayPriorityPopupController popupPrefab, PopupPriorityEnum priority)
	{
		GameplayPriorityPopupController gameplayPriorityPopupController = ObjectPool.Spawn(popupPrefab, popupParent, Vector3.zero);
		gameplayPriorityPopupController.InitPriority(priority);
		return gameplayPriorityPopupController;
	}

	public void AddPopup(GameplayPriorityPopupController popup)
	{
		switch (popup.priority)
		{
		case PopupPriorityEnum.Normal:
			popupQueue.Add(popup);
			break;
		case PopupPriorityEnum.Highest:
			popupQueue.Insert(0, popup);
			break;
		}
		popup.gameObject.SetActive(value: false);
		TryShowNextPopupInQueue();
	}

	public void RemoveCurrentPopup(GameplayPriorityPopupController popup)
	{
		if (!(popup != currentPopup))
		{
			currentPopup = null;
			TryShowNextPopupInQueue();
		}
	}

	private void TryShowNextPopupInQueue()
	{
		if (!(currentPopup != null) && popupQueue.Count > 0)
		{
			currentPopup = popupQueue[0];
			popupQueue.RemoveAt(0);
			currentPopup.OpenWithScaleAnimation();
		}
	}
}
