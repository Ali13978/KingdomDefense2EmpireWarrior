using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace DailyTrial
{
	public class RewardController : MonoBehaviour
	{
		[SerializeField]
		private int missionTier;

		[SerializeField]
		private Animator animator;

		[SerializeField]
		private Image rewardAvatar;

		[SerializeField]
		private Text rewardQuantity;

		[SerializeField]
		private Text waveRank;

		private DailyTrialRewardParam param;

		private int currentDay;

		private void Start()
		{
			currentDay = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
			Text text = waveRank;
			DailyTrialRewardParam rewardParameter = DailyTrialParameter.Instance.GetRewardParameter(currentDay, missionTier);
			text.text = rewardParameter.wave_require.ToString();
		}

		public void InitRewardInfor()
		{
			currentDay = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
			int doneMissionTier = ReadWriteDataDailyTrial.Instance.GetDoneMissionTier(currentDay);
			if (missionTier > doneMissionTier)
			{
				ProcessReward();
			}
			else
			{
				animator.Play("OpenNoReward");
			}
		}

		private void ProcessReward()
		{
			animator.Play("OpenReward");
			param = DailyTrialParameter.Instance.GetRewardParameter(currentDay, missionTier);
			int num = 0;
			if (param.gem_bonus > 0)
			{
				num = param.gem_bonus;
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(num, isDispatchEventChange: false);
				rewardAvatar.sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_gem");
			}
			if (param.item_freezing_bonus > 0)
			{
				num = param.item_freezing_bonus;
				ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(0, num);
				rewardAvatar.sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_pw_0");
			}
			if (param.item_meteor_bonus > 0)
			{
				num = param.item_meteor_bonus;
				ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(1, num);
				rewardAvatar.sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_pw_1");
			}
			if (param.item_healing_bonus > 0)
			{
				num = param.item_healing_bonus;
				ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(2, num);
				rewardAvatar.sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_pw_2");
			}
			if (param.item_goldchest_bonus > 0)
			{
				num = param.item_goldchest_bonus;
				ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(3, num);
				rewardAvatar.sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_pw_3");
			}
			rewardQuantity.text = num.ToString();
			ReadWriteDataDailyTrial.Instance.SetDoneMissionTier(currentDay, missionTier);
			UISoundManager.Instance.PlayluckyChestSound();
		}
	}
}
