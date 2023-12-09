using Middle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class ReadDataGameConfig : MonoBehaviour
	{
		private void Awake()
		{
			ReadGameConfig();
		}

		private void ReadGameConfig()
		{
			string text = "Parameters/game_config";
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					Config.Instance.LineCount = (int)list[i]["line_count"];
					Config.Instance.EarlyCallMoney = (int)list[i]["early_call_money"];
					Config.Instance.LifePercent2Star = (int)list[i]["life_2_star"];
					Config.Instance.LifePercent3Star = (int)list[i]["life_3_star"];
					Config.Instance.FirstTimeGemTakenPercentage = (int)list[i]["gem_taken_first_time"];
					Config.Instance.SecondTimeGemTakenPercentage = (int)list[i]["gem_taken_second_time"];
					Config.Instance.ThirdTimeGemTakenPercentage = (int)list[i]["gem_taken_third_time"];
				}
			}
			catch (Exception)
			{
				UnityEngine.Debug.LogError("File " + text + ".csv không tồn tại hoặc dữ liệu trong file không đúng định dạng.");
				throw;
			}
		}
	}
}
