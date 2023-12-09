using Middle;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class ReadHeroDescription : ReadCommonDescription
	{
		public override void ReadParameter()
		{
			base.ReadParameter();
			string text = "Parameters/Description/hero_description_" + Config.Instance.LanguageID;
			try
			{
				Singleton<HeroDescription>.Instance.ClearData();
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					HeroDes heroParameter = default(HeroDes);
					heroParameter.id = (int)list[i]["id"];
					heroParameter.name = (string)list[i]["name"];
					heroParameter.skillID = (int)list[i]["skill_id"];
					heroParameter.shortDescription = ((string)list[i]["short_description"]).Replace('$', '\n');
					heroParameter.fullDescription = ((string)list[i]["full_description"]).Replace('$', '\n');
					heroParameter.skillName = ((string)list[i]["skill_name"]).Replace('$', '\n');
					heroParameter.skillType = ((string)list[i]["skill_type"]).Replace('$', '\n');
					heroParameter.skillDescription = ((string)list[i]["skill_description"]).Replace('$', '\n');
					heroParameter.skillUnlock = ((string)list[i]["skill_unlock"]).Replace('$', '\n');
					Singleton<HeroDescription>.Instance.SetHeroParameter(heroParameter);
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
