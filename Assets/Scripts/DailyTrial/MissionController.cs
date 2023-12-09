using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace DailyTrial
{
	public class MissionController : MonoBehaviour
	{
		[SerializeField]
		private int missionTier;

		[SerializeField]
		private Image rewardAvatar;

		[SerializeField]
		private Text rewardQuantity;

		[SerializeField]
		private Text missionDescription;

		[SerializeField]
		private int missionNotifyID;

		[Space]
		[SerializeField]
		private GameObject rewardGroup;

		[SerializeField]
		private GameObject notifyDone;

		[SerializeField]
		private GameObject notifyUnDone;

		private DailyTrialRewardParam param;

		public void InitState()
		{
			int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
			int doneMissionTier = ReadWriteDataDailyTrial.Instance.GetDoneMissionTier(currentDayIndex);
			param = DailyTrialParameter.Instance.GetRewardParameter(currentDayIndex, missionTier);
			if (doneMissionTier < 0)
			{
				ViewMissionUnDone();
			}
			else if (missionTier > doneMissionTier && doneMissionTier >= 0)
			{
				ViewMissionUnDone();
			}
			else
			{
				ViewMissionDone();
			}
		}

		private void ViewMissionDone()
		{
			rewardGroup.SetActive(value: false);
			notifyUnDone.SetActive(value: false);
			notifyDone.SetActive(value: true);
			missionDescription.text = Singleton<NotificationDescription>.Instance.GetNotiContent(missionNotifyID) + " " + param.wave_require.ToString();
		}

		private void ViewMissionUnDone()
		{
			rewardGroup.SetActive(value: true);
			notifyUnDone.SetActive(value: true);
			notifyDone.SetActive(value: false);
			int num = 0;
			if (param.gem_bonus > 0)
			{
				num = param.gem_bonus;
				rewardAvatar.sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_gem");
			}
			if (param.item_freezing_bonus > 0)
			{
				num = param.item_freezing_bonus;
				rewardAvatar.sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_pw_0");
			}
			if (param.item_meteor_bonus > 0)
			{
				num = param.item_meteor_bonus;
				rewardAvatar.sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_pw_1");
			}
			if (param.item_healing_bonus > 0)
			{
				num = param.item_healing_bonus;
				rewardAvatar.sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_pw_2");
			}
			if (param.item_goldchest_bonus > 0)
			{
				num = param.item_goldchest_bonus;
				rewardAvatar.sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_pw_3");
			}
			rewardQuantity.text = num.ToString();
			missionDescription.text = Singleton<NotificationDescription>.Instance.GetNotiContent(missionNotifyID) + " " + param.wave_require.ToString();
		}
	}
}
