using DG.Tweening;
using SSR.Core.Architecture;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class SelectHeroSkill : MonoBehaviour
	{
		[SerializeField]
		private OrderedEventDispatcher OnCooldownDone;

		private const string TAP_TO_USE = "TapToUse";

		private const string TAP_N_CLICK_TO_USE = "TapNClickToUse";

		private int heroID;

		public float cooldownTime;

		private Button button;

		[SerializeField]
		private Image imageButton;

		[SerializeField]
		private Image imageCooldown;

		[SerializeField]
		private GameObject selectedImage;

		[SerializeField]
		private GameObject closeImage;

		private string useTypeValue;

		private bool toggle;

		private Tweener tween;

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
		}

		private void Start()
		{
			GameEventCenter.Instance.Subscribe(GameEventType.OnClickButton, new ClickButtonSubscriberData(GameTools.GetUniqueId(), HandleButtonClicked));
		}

		public void Init(int heroID)
		{
			InitHeroAvatar(heroID);
			HeroID = heroID;
			cooldownTime = SingletonMonoBehaviour<SpawnAlly>.Instance.GetHeroSkillCooldownTime(heroID);
			useTypeValue = SingletonMonoBehaviour<SpawnAlly>.Instance.GetHeroSkillUseType(heroID);
			ViewEnable();
		}

		private void InitHeroAvatar(int heroID)
		{
			imageButton.sprite = Resources.Load<Sprite>($"HeroCamp/SkillIcons/hero_{heroID}_skill_0");
			imageCooldown.sprite = Resources.Load<Sprite>($"HeroCamp/SkillIcons/hero_{heroID}_skill_0");
		}

		public void OnClick()
		{
			UISoundManager.Instance.PlayClick();
			GameEventCenter.Instance.Trigger(GameEventType.OnClickButton, new ClickedObjectData(ClickedObjectType.HeroSkillBtn, HeroID));
		}

		public void HandleButtonClicked(ClickedObjectData clickedObjData)
		{
			if (clickedObjData.clickedObjType == ClickedObjectType.HeroSkillBtn && clickedObjData.id == HeroID)
			{
				string text = useTypeValue;
				if (text == null)
				{
					return;
				}
				if (!(text == "TapToUse"))
				{
					if (text == "TapNClickToUse")
					{
						toggle = !toggle;
						if (toggle)
						{
							HeroesManager.Instance.ChooseHeroSkill(HeroID);
						}
						else
						{
							HeroesManager.Instance.UnChooseHeroSkill(HeroID);
						}
						Refresh();
					}
				}
				else
				{
					UnityEngine.Debug.Log("3");
				}
			}
			else
			{
				if (HeroID == HeroesManager.Instance.HeroSkillIDChoosing)
				{
					HeroesManager.Instance.UnChooseHeroSkill(HeroID);
				}
				Refresh();
			}
		}

		public void Refresh()
		{
			if (HeroesManager.Instance.HeroSkillIDChoosing == HeroID)
			{
				ViewSelected();
			}
			else
			{
				HideSelected();
			}
		}

		public void CancelSelect()
		{
			HideSelected();
		}

		public void DoCooldown()
		{
			CancelSelect();
			ViewDisable();
			tween = DOTween.To(() => 0f, delegate(float x)
			{
				imageCooldown.fillAmount = x;
			}, 1f, cooldownTime).SetEase(Ease.Linear).OnComplete(CooldownComplete);
			imageCooldown.gameObject.SetActive(value: true);
		}

		public void ViewDisableImmediately()
		{
			tween.Kill();
			button.enabled = false;
			imageButton.color = Color.gray;
			imageCooldown.gameObject.SetActive(value: false);
		}

		public void ViewEnableImmediately()
		{
			tween.Kill();
			CooldownComplete();
		}

		private void CooldownComplete()
		{
			ViewEnable();
			imageCooldown.gameObject.SetActive(value: false);
			OnCooldownDone.Dispatch();
		}

		private void ViewSelected()
		{
			selectedImage.SetActive(value: true);
			closeImage.SetActive(value: true);
		}

		public void HideSelected()
		{
			selectedImage.SetActive(value: false);
			closeImage.SetActive(value: false);
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
