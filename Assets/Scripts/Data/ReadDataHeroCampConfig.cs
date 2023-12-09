using Middle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class ReadDataHeroCampConfig : MonoBehaviour
	{
		private void Awake()
		{
			ReadGameConfig();
		}

		private void ReadGameConfig()
		{
			string text = "Parameters/herocamp_config";
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					HeroCampConfig.Instance.Health_max = (int)list[i]["health_max"];
					HeroCampConfig.Instance.Attack_damage_max = (int)list[i]["attack_damage_max"];
					HeroCampConfig.Instance.Physics_armor_max = (int)list[i]["physics_armor_max"];
					HeroCampConfig.Instance.Attack_speed_max = (int)list[i]["attack_speed_max"];
					HeroCampConfig.Instance.Magic_armor_max = (int)list[i]["magic_armor_max"];
					HeroCampConfig.Instance.Health_regen_max = (int)list[i]["health_regen"];
					HeroCampConfig.Instance.Movement_speed_max = (int)list[i]["move_speed"];
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
