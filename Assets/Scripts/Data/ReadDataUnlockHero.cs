using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class ReadDataUnlockHero : MonoBehaviour
	{
		private void Awake()
		{
			ReadHeroUnlockParameter();
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv không tồn tại hoặc dữ liệu trong file không đúng định dạng.");
		}

		private void ReadHeroUnlockParameter()
		{
			string text = "Parameters/hero_unlock_parameter";
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					HeroUnlockParam heroUnlockParamParameter = default(HeroUnlockParam);
					heroUnlockParamParameter.id = (int)list[i]["id"];
					heroUnlockParamParameter.name = (string)list[i]["name"];
					heroUnlockParamParameter.isUnlockByPlay = (int)list[i]["is_unlock_by_play"];
					heroUnlockParamParameter.mapIDToUnlock = (int)list[i]["unlock_at_map"];
					heroUnlockParamParameter.isUnlockByGem = (int)list[i]["is_unlock_by_gem"];
					heroUnlockParamParameter.gemAmountToUnlock = (int)list[i]["gem_unlock_amount"];
					heroUnlockParamParameter.isUnlockByRealMoney = (int)list[i]["is_unlock_by_real_money"];
					Singleton<UnlockHeroParameter>.Instance.SetHeroUnlockParamParameter(heroUnlockParamParameter);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
		}
	}
}
