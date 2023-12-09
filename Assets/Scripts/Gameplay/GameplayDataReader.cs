using Data;
using UnityEngine;

namespace Gameplay
{
	public class GameplayDataReader : SingletonMonoBehaviour<GameplayDataReader>
	{
		[SerializeField]
		private ReadDataLuckyChest readDataLuckyChest;

		[SerializeField]
		private ReadDataEndGameVideo readDataEndGameVideo;

		public ReadDataLuckyChest ReadDataLuckyChest
		{
			get
			{
				return readDataLuckyChest;
			}
			set
			{
				readDataLuckyChest = value;
			}
		}

		public ReadDataEndGameVideo ReadDataEndGameVideo
		{
			get
			{
				return readDataEndGameVideo;
			}
			set
			{
				readDataEndGameVideo = value;
			}
		}
	}
}
