using Middle;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class ReadEnemyDescription : ReadCommonDescription
	{
		public override void ReadParameter()
		{
			base.ReadParameter();
			string text = "Parameters/Description/enemy_description_" + Config.Instance.LanguageID;
			try
			{
				Singleton<EnemyDescription>.Instance.ClearData();
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					EnemyDes enemyDescription = default(EnemyDes);
					enemyDescription.id = (int)list[i]["id"];
					enemyDescription.name = (string)list[i]["name"];
					enemyDescription.level = (int)list[i]["level"];
					enemyDescription.decription = (string)list[i]["desctiption"];
					enemyDescription.specialAbility = (string)list[i]["special_ability"];
					Singleton<EnemyDescription>.Instance.SetEnemyDescription(enemyDescription);
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
