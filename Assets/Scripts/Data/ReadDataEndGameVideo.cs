using UnityEngine;

namespace Data
{
	public class ReadDataEndGameVideo : MonoBehaviour
	{
		[SerializeField]
		private EndGameVideo endGameVideoParam;

		public int GetCountdownTime()
		{
			return endGameVideoParam.param.countDownTimeSecond;
		}

		public int GetEndGameVideoReward()
		{
			return endGameVideoParam.param.lifeReward;
		}
	}
}
