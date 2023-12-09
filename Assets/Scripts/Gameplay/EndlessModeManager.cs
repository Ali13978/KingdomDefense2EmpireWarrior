using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class EndlessModeManager : MonoBehaviour
	{
		[Header("Wave lặp bắt đầu tại: ")]
		public int waveLoopBegin;

		[Header("Wave lặp kết thúc tại: ")]
		public int waveLoopEnd;

		[Header("Chỉ số máu tăng qua mỗi wave - đơn vị % ")]
		public float healthIncreasePercentage;

		[Header("Chỉ số damage tăng qua mỗi wave - đơn vị % ")]
		public float damageIncreasePercentage;

		[Header("Số lần loop")]
		public int LoopAmount;

		[Header("Số wave đã vượt qua")]
		public int TotalWavesPassed;

		[Header("Enemy cuối cùng của normal wave")]
		public bool IsLastEnemyInNormalWave;

		private int currentWaveEndless;

		private bool increased;

		public int CurrentWaveEndless
		{
			get
			{
				return currentWaveEndless;
			}
			set
			{
				currentWaveEndless = value;
				if (value > waveLoopEnd)
				{
					IncreaseLoopAmount();
					currentWaveEndless = waveLoopBegin;
				}
			}
		}

		public void FirstTimeIncreaseLoopAmount()
		{
			if (!increased)
			{
				IncreaseLoopAmount();
				increased = true;
			}
		}

		public void IncreaseLoopAmount()
		{
			LoopAmount++;
		}

		public void IncreaseWavePassed()
		{
			TotalWavesPassed++;
		}

		public void IncreaseCurrentWaveEndless()
		{
			CurrentWaveEndless++;
		}

		public void SetLastEnemyInNormalWave(bool lastEnemyInBattle)
		{
			IsLastEnemyInNormalWave = lastEnemyInBattle;
		}

		public void Init()
		{
			waveLoopBegin = MapRuleParameter.Instance.GetEndlessWaveLoopBegin();
			waveLoopEnd = MapRuleParameter.Instance.GetEndlessWaveLoopEnd();
			healthIncreasePercentage = MapRuleParameter.Instance.GetEndlessHealthIncreasePercentage();
			damageIncreasePercentage = MapRuleParameter.Instance.GetEndlessDamageIncreasePercentage();
			TotalWavesPassed = 0;
			IsLastEnemyInNormalWave = false;
			LoopAmount = 0;
			CurrentWaveEndless = waveLoopBegin;
		}
	}
}
