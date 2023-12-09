using Data;
using LifetimePopup;
using Middle;
using UnityEngine;
using UnityEngine.UI;

namespace WorldMap
{
	public class MapButtonController : ButtonController
	{
		[SerializeField]
		private int mapID;

		[SerializeField]
		private int themeID;

		[Space]
		[SerializeField]
		private StarGroupController starGroupController;

		[Space]
		[SerializeField]
		private GameObject[] listFlagObject;

		[SerializeField]
		private GameObject lockObject;

		[Space]
		[Header("Components")]
		[SerializeField]
		private Button button;

		[Space]
		[Header("Specific setting")]
		[SerializeField]
		private bool lockMap;

		private void Awake()
		{
			button = GetComponent<Button>();
		}

		private void Start()
		{
			starGroupController.DisplayStarGroup(ReadWriteDataMap.Instance.GetStarEarnedByMap(mapID));
		}

		public void ViewLock()
		{
			button.enabled = false;
			HideAllFlags();
			ShowLockFlag();
		}

		public void ViewUnLock()
		{
			int mapModeResult = ReadWriteDataMap.Instance.GetMapModeResult(mapID);
			HideAllFlags();
			switch (mapModeResult)
			{
			case 0:
				ShowFlag(0);
				break;
			case 1:
				ShowFlag(0);
				break;
			case 2:
				ShowFlag(1);
				break;
			case 3:
				ShowFlag(2);
				break;
			}
			button.enabled = true;
		}

		public void ViewFocus()
		{
		}

		private void HideAllFlags()
		{
			GameObject[] array = listFlagObject;
			foreach (GameObject gameObject in array)
			{
				gameObject.SetActive(value: false);
			}
			lockObject.SetActive(value: false);
		}

		private void ShowFlag(int flagID)
		{
			listFlagObject[flagID].SetActive(value: true);
		}

		private void ShowLockFlag()
		{
			lockObject.SetActive(value: true);
		}

		public override void OnClick()
		{
			base.OnClick();
			if (lockMap)
			{
				string content = "Coming soon!";
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(content, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
			else
			{
				MiddleDelivery.Instance.MapIDSelected = mapID;
				ReadWriteDataTheme.Instance.SaveLastThemePlayed(themeID);
				SingletonMonoBehaviour<UIRootController>.Instance.mapLevelSelectPopupController.Init(mapID);
			}
		}
	}
}
