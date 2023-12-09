using Data;
using DG.Tweening;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class SelectHero : MonoBehaviour
	{
		[SerializeField]
		private int heroID;

		public float heroSpawnTime;

		private Button button;

		private Image imageButton;

		[SerializeField]
		private Image imageCooldown;

		[SerializeField]
		private GameObject selectedImage;

		private bool toggle;

		[Space]
		[Header("Hero health bar")]
		[SerializeField]
		private RectTransform healthBar;

		private Vector2 healthBarSize;

		[SerializeField]
		private float maxHealthBarValue;

		[SerializeField]
		private float minHealthBarValue;

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
			button = GetComponent<Button>();
			imageButton = GetComponent<Image>();
		}

		public void Init(int heroID)
		{
			HeroID = heroID;
			InitHeroAvatar(heroID);
			int currentHeroLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			Hero heroParameter = HeroParameter.Instance.GetHeroParameter(heroID, currentHeroLevel);
			heroSpawnTime = (float)heroParameter.respawn_time / 1000f;
			ViewEnable();
			int uniqueId = GameTools.GetUniqueId();
			GameEventCenter.Instance.Subscribe(GameEventType.OnSelectHero, new SelectCharacterSubscriberData(uniqueId, HandleSelectingHero));
			GameEventCenter.Instance.Subscribe(GameEventType.OnClickButton, new ClickButtonSubscriberData(uniqueId, OnAButtonClicked));
		}

		private void InitHeroAvatar(int heroID)
		{
			imageButton.sprite = Resources.Load<Sprite>($"Gameplay-HeroIcon/mini_icon_hero_{heroID}");
			imageCooldown.sprite = Resources.Load<Sprite>($"Gameplay-HeroIcon/mini_icon_hero_{heroID}");
		}

		public void UpdateHealthBar(int currentHealth, int originHealth)
		{
			float num = (float)originHealth / (maxHealthBarValue - minHealthBarValue);
			float num2 = (float)currentHealth / num;
			ref Vector2 reference = ref healthBarSize;
			float newX = num2 + minHealthBarValue;
			Vector2 sizeDelta = healthBar.sizeDelta;
			reference.Set(newX, sizeDelta.y);
			healthBar.sizeDelta = healthBarSize;
		}

		public void Refresh()
		{
			if (HeroesManager.Instance.HeroIDChoosing == HeroID)
			{
				ViewSelected();
			}
			else
			{
				HideSelected();
			}
		}

		public void OnClick()
		{
			GameEventCenter.Instance.Trigger(GameEventType.OnSelectHero, HeroID);
			GameEventCenter.Instance.Trigger(GameEventType.OnClickButton, new ClickedObjectData(ClickedObjectType.HeroIconBtn));
		}

		public void HandleSelectingHero(int heroId)
		{
			if (heroId == HeroID)
			{
				UISoundManager.Instance.PlayClick();
				toggle = !toggle;
				if (toggle)
				{
					HeroesManager.Instance.ChooseHero(HeroID);
				}
				else
				{
					HeroesManager.Instance.UnChooseHero(HeroID);
				}
				Refresh();
			}
		}

		public void OnAButtonClicked(ClickedObjectData clickedObjData)
		{
			if (clickedObjData.clickedObjType != 0 && HeroID == HeroesManager.Instance.HeroIDChoosing)
			{
				HandleSelectingHero(HeroID);
			}
		}

		public void DoCooldown()
		{
			DOTween.To(() => 0f, delegate(float x)
			{
				imageCooldown.fillAmount = x;
			}, 1f, heroSpawnTime).SetEase(Ease.Linear).OnComplete(CooldownComplete);
			imageCooldown.gameObject.SetActive(value: true);
		}

		private void CooldownComplete()
		{
			ViewEnable();
			imageCooldown.gameObject.SetActive(value: false);
		}

		private void ViewSelected()
		{
			selectedImage.SetActive(value: true);
			SingletonMonoBehaviour<GameplayUIHeroManager>.Instance.HeroCurrentLevelInformationPopup.Init(HeroID, ReadWriteDataHero.Instance.GetCurrentHeroLevel(HeroID));
		}

		public void HideSelected()
		{
			selectedImage.SetActive(value: false);
			SingletonMonoBehaviour<GameplayUIHeroManager>.Instance.HeroCurrentLevelInformationPopup.Close();
			toggle = false;
		}

		public void ViewEnable()
		{
			button.enabled = true;
			imageButton.color = Color.white;
		}

		public void ViewDisable()
		{
			button.enabled = false;
			imageButton.color = Color.gray;
		}
	}
}
