using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MapLevel
{
	public class HeroSlotController : ButtonController
	{
		private Button button;

		private Image image;

		[SerializeField]
		private Sprite addHero;

		[SerializeField]
		private Text heroLevelText;

		public GameObject boosterObj;

		public TextMeshProUGUI boosterText;

		private int slot;

		private int heroID;

		public bool IsInitValue;

		private HeroesInputGroupController heroesInputGroupController;

		private void Awake()
		{
			GetAllComponents();
			button.enabled = false;
		}

		public void Init(HeroesInputGroupController heroesInputGroupController)
		{
			this.heroesInputGroupController = heroesInputGroupController;
		}

		private void GetAllComponents()
		{
			button = GetComponent<Button>();
			image = GetComponent<Image>();
		}

		public override void OnClick()
		{
			base.OnClick();
			if (IsInitValue)
			{
				UnityEngine.Debug.Log("bỏ chọn hero + " + heroID + " tại slot " + slot);
				heroesInputGroupController.HeroesSelectedController.UnChooseHero(slot);
				heroesInputGroupController.HeroesSelectController.UnChooseHero(heroID);
				IsInitValue = false;
				button.enabled = false;
				ViewUnChoose();
			}
		}

		public void InitHeroSlot(int slot, int heroID)
		{
			this.slot = slot;
			this.heroID = heroID;
			IsInitValue = true;
			button.enabled = true;
			SetHeroAvatar();
			SetHeroLevel();
			boosterObj.SetActive(value: false);
			if (GameTools.cachedHavingBooster)
			{
				boosterObj.SetActive(value: true);
				boosterText.text = $"x{GameTools.cachedBoosterMultiplier}";
			}
		}

		public void SetDefaultValue()
		{
			IsInitValue = false;
			ViewUnChoose();
		}

		private void SetHeroAvatar()
		{
			image.sprite = Resources.Load<Sprite>($"HeroesAvatar/avatar_hero_{heroID}");
		}

		private void SetHeroLevel()
		{
			heroLevelText.text = (ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID) + 1).ToString();
		}

		public void ViewUnChoose()
		{
			image.sprite = addHero;
			heroLevelText.text = string.Empty;
		}

		public void Show()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
