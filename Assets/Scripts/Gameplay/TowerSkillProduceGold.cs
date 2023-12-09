using Parameter;
using SSR.Core.Architecture;
using UnityEngine;

namespace Gameplay
{
	public class TowerSkillProduceGold : TowerController
	{
		[SerializeField]
		private OrderedEventDispatcher onProduceGold;

		[SerializeField]
		private ProducedGoldController producedGoldController;

		private int goldProduce;

		private float cooldownTimeProduce;

		private float cooldownTimeProduceTracking;

		private float cooldownTimeAutoCollect;

		private float cooldownTimeAutoCollectTracking;

		private bool skillReady;

		private bool isProducingGold;

		public override void OnAppear()
		{
			base.OnAppear();
			SetParameter();
			producedGoldController.ResetParameter();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			skillReady = false;
			producedGoldController.ResetParameter();
		}

		private void SetParameter()
		{
			goldProduce = TowerParameter.Instance.GetGoldProduce(base.TowerModel.Id, base.TowerModel.Level);
			cooldownTimeProduce = TowerParameter.Instance.GetCooldownTime(base.TowerModel.Id, base.TowerModel.Level);
			cooldownTimeProduceTracking = cooldownTimeProduce;
			cooldownTimeAutoCollect = TowerParameter.Instance.GetAutoCollectProduceGoldTime(base.TowerModel.Id, base.TowerModel.Level);
			cooldownTimeAutoCollectTracking = cooldownTimeAutoCollect;
			skillReady = true;
		}

		public override void Update()
		{
			base.Update();
			if (SingletonMonoBehaviour<GameData>.Instance.IsGameStart && skillReady)
			{
				if (IsCooldownProduceDone())
				{
					ProduceGold();
				}
				cooldownTimeProduceTracking = Mathf.MoveTowards(cooldownTimeProduceTracking, 0f, Time.deltaTime);
				if (IsCooldownAutoCollectDone())
				{
					AutoCollect();
				}
				if (isProducingGold)
				{
					cooldownTimeAutoCollectTracking = Mathf.MoveTowards(cooldownTimeAutoCollectTracking, 0f, Time.deltaTime);
				}
			}
		}

		private bool IsCooldownProduceDone()
		{
			return cooldownTimeProduceTracking == 0f;
		}

		private void ResetCooldownProduce()
		{
			cooldownTimeProduceTracking = cooldownTimeProduce;
		}

		private void ProduceGold()
		{
			producedGoldController.Init(goldProduce);
			ResetCooldownProduce();
			onProduceGold.Dispatch();
			isProducingGold = true;
		}

		private bool IsCooldownAutoCollectDone()
		{
			return cooldownTimeAutoCollectTracking == 0f;
		}

		private void ResetCooldownAutoCollect()
		{
			cooldownTimeAutoCollectTracking = cooldownTimeAutoCollect;
		}

		private void AutoCollect()
		{
			producedGoldController.TapOnGold();
			ResetCooldownAutoCollect();
			isProducingGold = false;
		}
	}
}
