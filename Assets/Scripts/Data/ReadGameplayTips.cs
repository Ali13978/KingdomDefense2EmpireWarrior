using Middle;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class ReadGameplayTips : ReadCommonDescription
	{
		public override void ReadParameter()
		{
			base.ReadParameter();
			string text = "Parameters/Description/gameplay_tip_" + Config.Instance.LanguageID;
			try
			{
				Singleton<GameplayTipsDescription>.Instance.ClearData();
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					GameplayTip gameplayTipParameter = default(GameplayTip);
					gameplayTipParameter.id = (int)list[i]["tip_id"];
					gameplayTipParameter.name = (string)list[i]["tip_name"];
					gameplayTipParameter.description = ((string)list[i]["tip_description"]).Replace('$', '\n');
					Singleton<GameplayTipsDescription>.Instance.SetGameplayTipParameter(gameplayTipParameter);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv không tồn tại hoặc dữ liệu trong file không đúng định dạng.");
		}
	}
}
