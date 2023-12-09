using Data;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace MapLevel
{
	public class HeroSelectButtonController : ButtonController
	{
		private Button button;

		private Image image;

		[SerializeField]
		private Text heroLevelText;

		[SerializeField]
		private GameObject lockImage;

		private int heroID;

		private HeroesInputGroupController heroesInputGroupController;

		public bool Selected;

		public int HeroID
		{
			get
			{
				return heroID;
			}
			set
			{
				heroID = value;
			}
		}

		private void Awake()
		{
			GetAllComponents();
		}

		private void GetAllComponents()
		{
			button = GetComponent<Button>();
			image = GetComponent<Image>();
		}

		private void OnEnable()
		{
			SetHeroLevel();
		}

		public void UpdateStatus()
		{
			if (ReadWriteDataHero.Instance.IsHeroOwned(HeroID))
			{
				image.color = Color.white;
				lockImage.SetActive(value: false);
				base.transform.SetAsFirstSibling();
			}
			else
			{
				image.color = Color.gray;
				lockImage.SetActive(value: true);
				base.transform.SetAsLastSibling();
			}
		}

		public void Init(int heroID, HeroesInputGroupController heroesInputGroupController)
		{
			HeroID = heroID;
			this.heroesInputGroupController = heroesInputGroupController;
			SetHeroAvatar();
			SetHeroLevel();
		}

		private void SetHeroAvatar()
		{
			image.sprite = Resources.Load<Sprite>($"HeroesMiniAvatar/mini_avatar_hero_{HeroID}");
		}

		private void SetHeroLevel()
		{
			heroLevelText.text = (ReadWriteDataHero.Instance.GetCurrentHeroLevel(HeroID) + 1).ToString();
		}

		public void SetDefaultValue()
		{
			SetView_NonSelect();
		}

		public override void OnClick()
		{
			base.OnClick();
			if (ReadWriteDataHero.Instance.IsHeroOwned(HeroID))
			{
				if (!heroesInputGroupController.HeroesSelectedController.IsFullSlot() && !Selected)
				{
					heroesInputGroupController.HeroesSelectedController.ChooseHero(HeroID);
					SetView_Selected();
					SendEvent_ChooseHero();
				}
			}
			else
			{
				UnityEngine.Debug.Log("Ban co muon unlock hero khong ???");
				heroesInputGroupController.AskToBuyHeroPopupController.Init(HeroID);
			}
		}

		private void SendEvent_ChooseHero()
		{
			string heroName = HeroParameter.Instance.GetHeroName(HeroID);
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_ChooseHeroAtMapLevelSelect(heroName);
		}

		public void SetView_NonSelect()
		{
			Selected = false;
			base.gameObject.SetActive(value: true);
		}

		public void SetView_Selected()
		{
			Selected = true;
			base.gameObject.SetActive(value: false);
		}
	}
}
