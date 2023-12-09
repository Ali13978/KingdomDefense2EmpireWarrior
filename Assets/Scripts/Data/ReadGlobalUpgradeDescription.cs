using Middle;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class ReadGlobalUpgradeDescription : ReadCommonDescription
	{
		public override void ReadParameter()
		{
			base.ReadParameter();
			string text = "Parameters/Description/global_upgrade_description_" + Config.Instance.LanguageID;
			try
			{
				Singleton<GlobalUpgradeDescription>.Instance.ClearData();
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					GlobalUpgradeDes globalUpgradeParameters = default(GlobalUpgradeDes);
					globalUpgradeParameters.id = (int)list[i]["upgrade_id"];
					globalUpgradeParameters.title = (string)list[i]["upgrade_title"];
					globalUpgradeParameters.description = ((string)list[i]["upgrade_description"]).Replace('$', '\n');
					Singleton<GlobalUpgradeDescription>.Instance.SetGlobalUpgradeParameters(globalUpgradeParameters);
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
