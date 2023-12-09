using Middle;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class ReadTutorialDescription : ReadCommonDescription
	{
		public override void ReadParameter()
		{
			base.ReadParameter();
			string text = "Parameters/Description/tutorial_" + Config.Instance.LanguageID;
			try
			{
				Singleton<TutorialDescription>.Instance.ClearData();
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					TutorialDes tutParameter = default(TutorialDes);
					tutParameter.id = (string)list[i]["tut_id"];
					tutParameter.description = ((string)list[i]["tut_description"]).Replace('$', '\n');
					Singleton<TutorialDescription>.Instance.SetTutParameter(tutParameter);
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
