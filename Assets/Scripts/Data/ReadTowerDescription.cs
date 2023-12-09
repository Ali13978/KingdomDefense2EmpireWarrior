using Middle;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class ReadTowerDescription : ReadCommonDescription
	{
		public override void ReadParameter()
		{
			base.ReadParameter();
			ReadTowerParameter();
			ReadTowerSkillParameter();
		}

		private void ReadTowerSkillParameter()
		{
			string text = "Parameters/Description/tower_skill_description_" + Config.Instance.LanguageID;
			try
			{
				Singleton<TowerDescription>.Instance.ClearTowerSkillData();
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					TowerSkillDes towerSkillParameter = default(TowerSkillDes);
					towerSkillParameter.id = (int)list[i]["id"];
					towerSkillParameter.level = (int)list[i]["level"];
					towerSkillParameter.ultimateBranch = (int)list[i]["ultimate_id"];
					towerSkillParameter.skillID = (int)list[i]["skill_id"];
					towerSkillParameter.name = (string)list[i]["name"];
					towerSkillParameter.ultimateName = ((string)list[i]["ultimate_name"]).Replace('$', '\n');
					towerSkillParameter.ultimateDescription = ((string)list[i]["ultimate_description"]).Replace('$', '\n');
					Singleton<TowerDescription>.Instance.SetTowerSkillParameter(towerSkillParameter);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
		}

		private void ReadTowerParameter()
		{
			string text = "Parameters/Description/tower_description_" + Config.Instance.LanguageID;
			try
			{
				Singleton<TowerDescription>.Instance.ClearTowerData();
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					TowerDes towerParameter = default(TowerDes);
					towerParameter.id = (int)list[i]["id"];
					towerParameter.name = (string)list[i]["name"];
					towerParameter.type = (string)list[i]["type"];
					towerParameter.level = (int)list[i]["level"];
					towerParameter.shortDescription = ((string)list[i]["short_description"]).Replace('$', '\n');
					towerParameter.fullDescription = ((string)list[i]["description"]).Replace('$', '\n');
					towerParameter.unlockDescription = ((string)list[i]["unlock_description"]).Replace('$', '\n');
					Singleton<TowerDescription>.Instance.SetTowerParameter(towerParameter);
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
