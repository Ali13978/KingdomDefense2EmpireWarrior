using Middle;
using MyCustom;
using UnityEngine;

namespace Gameplay
{
	public class BuildRegionController : CustomMonoBehaviour
	{
		public int regionID;

		private BoxCollider2D boxCollider;

		[SerializeField]
		private GameObject buildableImage;

		public Transform spawnAllyPosition;

		private void Awake()
		{
			GetAllComponents();
		}

		private void GetAllComponents()
		{
			boxCollider = GetComponent<BoxCollider2D>();
		}

		public void DisplayBuildable()
		{
			TurnOnCollider();
			buildableImage.SetActive(value: true);
		}

		public void DisplayNotBuildable()
		{
			TurnOffCollider();
			buildableImage.SetActive(value: false);
		}

		public void TurnOnCollider()
		{
			boxCollider.enabled = true;
		}

		public void TurnOffCollider()
		{
			boxCollider.enabled = false;
		}

		public void TryToClick()
		{
			if (SingletonMonoBehaviour<UIRootController>.Instance.BuyTowerPopupController.isOpen)
			{
				GameplayManager.Instance.CurrentTowerRange.GetComponent<TowerRangeController>().HideRange();
			}
			SingletonMonoBehaviour<UIRootController>.Instance.BuyTowerPopupController.Init(base.transform);
			Config.Instance.currentTowerRegionIDSelected = regionID;
		}
	}
}
