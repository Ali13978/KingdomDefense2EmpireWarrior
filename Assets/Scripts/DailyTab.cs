using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

public class DailyTab : MonoBehaviour
{
	[SerializeField]
	private Image heroAvatar;

	[SerializeField]
	private Image heroName;

	[SerializeField]
	private Text heroLevel;

	[Space]
	[SerializeField]
	private GameObject selectedImage;

	[SerializeField]
	private Image[] itemsAvatar;

	[SerializeField]
	private Text[] itemsQuantity;

	[Space]
	[SerializeField]
	private int day;

	[SerializeField]
	private int notiID;

	[SerializeField]
	private Text dayTitle;

	[Space]
	[SerializeField]
	private GameObject notifyDone;

	[SerializeField]
	private GameObject notifyNotDone;

	private DailyTrialParam param;

	[SerializeField]
	private bool displayInputItems;

	[SerializeField]
	private bool displayRewardItems;

	[SerializeField]
	private GameObject[] listItemHolder;

	public int Day
	{
		get
		{
			return day;
		}
		set
		{
			day = value;
		}
	}

	public void Init()
	{
		param = DailyTrialParameter.Instance.GetParameter(Day);
		InitDayTitle();
		InitHeroAvatar();
		InitItems();
		InitSelectedImage();
		InitMissionStatus();
	}

	private void InitDayTitle()
	{
		if ((bool)dayTitle)
		{
			dayTitle.text = Singleton<NotificationDescription>.Instance.GetNotiContent(notiID) + " " + (day + 1).ToString();
		}
	}

	private void InitHeroAvatar()
	{
		int num = 0;
		if (param.input_hero_id_slot_0 >= 0)
		{
			num = param.input_hero_id_slot_0;
		}
		if (param.input_hero_id_slot_1 >= 0)
		{
			num = param.input_hero_id_slot_1;
		}
		if (param.input_hero_id_slot_2 >= 0)
		{
			num = param.input_hero_id_slot_2;
		}
		if (Day == 6)
		{
			heroAvatar.sprite = Resources.Load<Sprite>("HeroesAvatar/avatar_hero_combo");
			heroLevel.text = string.Empty;
			if ((bool)heroName)
			{
				heroName.gameObject.SetActive(value: false);
			}
			return;
		}
		heroAvatar.sprite = Resources.Load<Sprite>($"HeroesAvatar/avatar_hero_{num}");
		heroLevel.text = (ReadWriteDataDailyTrial.Instance.GetHeroDailyTrialLevel() + 1).ToString();
		if ((bool)heroName)
		{
			heroName.gameObject.SetActive(value: true);
			heroName.sprite = Resources.Load<Sprite>($"HeroesName/name_hero_{num}");
		}
	}

	private void InitItems()
	{
		Image[] array = itemsAvatar;
		foreach (Image image in array)
		{
			image.gameObject.SetActive(value: false);
		}
		Text[] array2 = itemsQuantity;
		foreach (Text text in array2)
		{
			text.text = string.Empty;
		}
		if (displayInputItems)
		{
			int[] array3 = new int[4];
			array3 = DailyTrialParameter.Instance.getListInputItem(Day);
			int num = 0;
			for (int k = 0; k < array3.Length; k++)
			{
				if (array3[k] > 0)
				{
					itemsAvatar[num].gameObject.SetActive(value: true);
					itemsAvatar[num].sprite = Resources.Load<Sprite>($"LuckyChest/Items/lucky_item_pw_{k}");
					itemsQuantity[num].text = array3[k].ToString();
					num++;
				}
			}
		}
		if (!displayRewardItems)
		{
			return;
		}
		int[] array4 = new int[5];
		array4 = DailyTrialParameter.Instance.getListRewardValue(Day);
		int num2 = 0;
		if (array4[4] > 0)
		{
			itemsAvatar[num2].gameObject.SetActive(value: true);
			itemsAvatar[num2].sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_gem");
			itemsQuantity[num2].text = array4[4].ToString();
			num2++;
		}
		for (int l = 0; l < array4.Length - 1; l++)
		{
			if (array4[l] > 0)
			{
				itemsAvatar[num2].gameObject.SetActive(value: true);
				itemsAvatar[num2].sprite = Resources.Load<Sprite>($"LuckyChest/Items/lucky_item_pw_{l}");
				itemsQuantity[num2].text = array4[l].ToString();
				num2++;
			}
		}
		for (int m = 0; m < listItemHolder.Length; m++)
		{
			if (m < num2)
			{
				listItemHolder[m].SetActive(value: true);
			}
			else
			{
				listItemHolder[m].SetActive(value: false);
			}
		}
	}

	private void InitSelectedImage()
	{
		int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
		if ((bool)selectedImage)
		{
			selectedImage.SetActive(Day == currentDayIndex);
		}
	}

	private void InitMissionStatus()
	{
		if ((bool)notifyDone && (bool)notifyNotDone)
		{
			int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
			if (day < currentDayIndex)
			{
				notifyDone.SetActive(value: true);
				notifyNotDone.SetActive(value: false);
			}
			else if (day > currentDayIndex)
			{
				notifyDone.SetActive(value: false);
				notifyNotDone.SetActive(value: false);
			}
			else
			{
				bool flag = ReadWriteDataDailyTrial.Instance.IsDoneMaxTierMission(day);
				notifyDone.SetActive(flag);
				notifyNotDone.SetActive(!flag);
			}
		}
	}
}
