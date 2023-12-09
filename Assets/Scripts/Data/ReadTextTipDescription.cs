using Middle;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class ReadTextTipDescription : ReadCommonDescription
	{
		public override void ReadParameter()
		{
			base.ReadParameter();
			string text = "Parameters/Description/text_tip_" + Config.Instance.LanguageID;
			try
			{
				Singleton<TextTipParameter>.Instance.ClearData();
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					TextTip textTipParameter = default(TextTip);
					textTipParameter.level = (int)list[i]["level"];
					textTipParameter.id = (int)list[i]["id"];
					textTipParameter.textTipContent = (string)list[i]["text_tip_content"];
					Singleton<TextTipParameter>.Instance.SetTextTipParameter(textTipParameter);
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
