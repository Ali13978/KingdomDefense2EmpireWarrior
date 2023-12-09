using Data;
using UnityEngine;
using UnityEngine.UI;

namespace WorldMap
{
	public class TotalStar : MonoBehaviour
	{
		[SerializeField]
		private Text textStar;

		private int totalMap;

		private int maxStarPerMap = 3;

		private void Start()
		{
			totalMap = ReadWriteDataMap.Instance.GetTotalMap();
			UpdateStar();
		}

		private void UpdateStar()
		{
			textStar.text = ReadWriteDataPlayerCurrency.Instance.GetCurrentStar().ToString() + "/" + (totalMap * maxStarPerMap).ToString();
		}
	}
}
