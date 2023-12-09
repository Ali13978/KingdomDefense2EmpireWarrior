using Middle;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class ReadPowerupItemDescription : ReadCommonDescription
	{
		public override void ReadParameter()
		{
			base.ReadParameter();
			string text = "Parameters/Description/powerup_item_description_" + Config.Instance.LanguageID;
			try
			{
				Singleton<PowerupItemDescription>.Instance.ClearData();
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					PowerupItemDes powerupItemParameter = default(PowerupItemDes);
					powerupItemParameter.id = (int)list[i]["item_id"];
					powerupItemParameter.name = (string)list[i]["item_name"];
					powerupItemParameter.description = ((string)list[i]["item_description"]).Replace('$', '\n');
					Singleton<PowerupItemDescription>.Instance.SetPowerupItemParameter(powerupItemParameter);
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
