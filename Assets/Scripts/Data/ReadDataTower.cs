using MyCustom;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class ReadDataTower : CustomMonoBehaviour
	{
		private static ReadDataTower instance;

		public static ReadDataTower Instance => instance;

		private void Awake()
		{
			instance = this;
			ReadTowerParameter();
			ReadTowerDefaultSkillParameter();
			ReadTowerSkillParameter();
		}

		private void ReadTowerDefaultSkillParameter()
		{
			string text = "Parameters/tower_default_skill_parameter";
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					TowerDefaultSkill parameter = default(TowerDefaultSkill);
					parameter.id = (int)list[i]["id"];
					parameter.towerID = (int)list[i]["tower_id"];
					parameter.towerLevel = (int)list[i]["level"];
					parameter.name = (string)list[i]["name"];
					parameter.skillParam0 = (int)list[i]["skill_param_0"];
					parameter.skillParam1 = (int)list[i]["skill_param_1"];
					parameter.skillParam2 = (int)list[i]["skill_param_2"];
					parameter.skillParam3 = (int)list[i]["skill_param_3"];
					parameter.skillParam4 = (int)list[i]["skill_param_4"];
					parameter.skillName = (string)list[i]["skill_name"];
					TowerDefaultSkillParameter.Instance.SetParameter(parameter);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
		}

		private void ReadTowerSkillParameter()
		{
			string text = "Parameters/TowerSkills/tower_skill_parameter";
			try
			{
				TowerSkillParameter.Instance.ClearTowerSkillData();
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					TowerSkillParam parameter = default(TowerSkillParam);
					parameter.towerID = (int)list[i]["tower_id"];
					parameter.ultimateBranch = (int)list[i]["ultimate_branch"];
					parameter.skillID = (int)list[i]["skill_id"];
					parameter.skillLevel = (int)list[i]["skill_level"];
					parameter.towerName = ((string)list[i]["tower_name"]).Replace('$', '\n');
					parameter.skillName = ((string)list[i]["skill_name"]).Replace('$', '\n');
					parameter.upgradeCost = (int)list[i]["upgrade_cost"];
					parameter.paramPreview = (string)list[i]["param_preview"];
					parameter.param0 = (int)list[i]["param_0"];
					parameter.param1 = (int)list[i]["param_1"];
					parameter.param2 = (int)list[i]["param_2"];
					parameter.param3 = (int)list[i]["param_3"];
					parameter.param4 = (int)list[i]["param_4"];
					TowerSkillParameter.Instance.SetParameter(parameter);
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
			string text = "Parameters/tower_parameter";
			try
			{
				List<bool> list = new List<bool>();
				List<Dictionary<string, object>> list2 = CSVReader.Read(text);
				for (int i = 0; i < list2.Count; i++)
				{
					Tower towerParameter = default(Tower);
					towerParameter.id = (int)list2[i]["id"];
					towerParameter.name = (string)list2[i]["name"];
					towerParameter.level = (int)list2[i]["level"];
					towerParameter.price = (int)list2[i]["price"];
					towerParameter.reload = (int)list2[i]["reload"];
					towerParameter.ignoreReloadChance = (int)list2[i]["ignore_reload_chance"];
					towerParameter.goldProduce = (int)list2[i]["gold_produce"];
					towerParameter.autoCollectTime = (int)list2[i]["auto_collect_time"];
					towerParameter.attackRangeMax = (int)list2[i]["attack_range"];
					towerParameter.damage_Physics_min = (int)list2[i]["damage_physics_min"];
					towerParameter.damage_Physics_max = (int)list2[i]["damage_physics_max"];
					towerParameter.damage_Magic_min = (int)list2[i]["damage_magic_min"];
					towerParameter.damage_Magic_max = (int)list2[i]["damage_magic_max"];
					towerParameter.instantKillChance = (int)list2[i]["instant_kill_chance"];
					towerParameter.criticalStrikeChance = (int)list2[i]["critical_strike_chance"];
					towerParameter.ignoreArmorChance = (int)list2[i]["ignore_armor_chance"];
					towerParameter.unit_health = (int)list2[i]["unit_health"];
					towerParameter.unit_armor_physics = (int)list2[i]["unit_armor_physics"];
					towerParameter.unit_armor_magic = (int)list2[i]["unit_armor_magic"];
					towerParameter.unit_attackCooldown = (int)list2[i]["unit_attackCooldown"];
					towerParameter.unit_attack_range_min = (int)list2[i]["unit_attack_range_min"];
					towerParameter.unit_attack_range_average = (int)list2[i]["unit_attack_range_average"];
					towerParameter.unit_attack_range_max = (int)list2[i]["unit_attack_range_max"];
					towerParameter.unit_moveSpeed = (int)list2[i]["unit_moveSpeed"];
					towerParameter.unit_dodge_chance = (int)list2[i]["unit_dodge_chance"];
					towerParameter.unit_ignore_armor_chance = (int)list2[i]["unit_ignore_armor_chance"];
					towerParameter.debuffKey = ((string)list2[i]["debuff_effect_key"]).Replace('$', '\n');
					towerParameter.debuffEffectValue = (int)list2[i]["debuff_effect_value"];
					towerParameter.debuffEffectDuration = (int)list2[i]["debuff_effect_duration"];
					towerParameter.debuffChance = (int)list2[i]["debuff_chance"];
					towerParameter.isAirAttack = (((int)list2[i]["air_attack"] > 0) ? true : false);
					towerParameter.isRoundAttack = (((int)list2[i]["round_attack"] > 0) ? true : false);
					towerParameter.bulletAoe = (int)list2[i]["bull_aoe"];
					towerParameter.value = (int)list2[i]["value"];
					TowerParameter.Instance.SetTowerParameter(towerParameter);
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
