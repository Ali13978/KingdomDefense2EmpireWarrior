using MyCustom;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class ReadDataHero : CustomMonoBehaviour
	{
		private void Awake()
		{
			ReadHeroParameter();
			ReadHeroSkillParameter();
		}

		private void ReadHeroParameter()
		{
			string text = "Parameters/hero_parameter";
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					Hero heroParameter = default(Hero);
					heroParameter.id = (int)list[i]["id"];
					heroParameter.name = (string)list[i]["name"];
					heroParameter.level = (int)list[i]["level"];
					heroParameter.exp_per_level = (int)list[i]["exp_per_level"];
					heroParameter.respawn_time = (int)list[i]["respawn_time"];
					heroParameter.health = (int)list[i]["health"];
					heroParameter.health_regen = (int)list[i]["health_regen"];
					heroParameter.health_regen_cooldown = (int)list[i]["health_regen_cooldown"];
					heroParameter.armor_physics = (int)list[i]["armor_physics"];
					heroParameter.armor_magic = (int)list[i]["armor_magic"];
					heroParameter.critical_strike_change = (int)list[i]["critical_strike_chance"];
					heroParameter.attack_physics_min = (int)list[i]["attack_physics_min"];
					heroParameter.attack_physics_max = (int)list[i]["attack_physics_max"];
					heroParameter.attack_magic_min = (int)list[i]["attack_magic_min"];
					heroParameter.attack_magic_max = (int)list[i]["attack_magic_max"];
					heroParameter.attack_cooldown = (int)list[i]["attack_cooldown"];
					heroParameter.attack_range_min = (int)list[i]["attack_range_min"];
					heroParameter.attack_range_average = (int)list[i]["attack_range_average"];
					heroParameter.attack_range_max = (int)list[i]["attack_range_max"];
					heroParameter.activate_range = (int)list[i]["activate_range"];
					heroParameter.speed = (int)list[i]["speed"];
					heroParameter.canAttackAir = (int)list[i]["can_attack_air"];
					heroParameter.skillPointBonus = (int)list[i]["skill_point_bonus"];
					HeroParameter.Instance.SetHeroParameter(heroParameter);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
		}

		private void ReadHeroSkillParameter()
		{
			ReadHeroSkillParam_Wukong();
			ReadHeroSkillParam_Ashi();
			ReadHeroSkillParam_Galahad();
			ReadHeroSkillParam_ShamanKing();
			ReadHeroSkillParam_Golem();
			ReadHeroSkillParam_NatureQueen();
			ReadHeroSkillParam_Thor();
			ReadHeroSkillParam_Ninja();
			ReadHeroSkillParam_Tristana();
			ReadHeroSkillParam_JungleLord();
			ReadHeroSkillParam_SteelDragon();
			ReadHeroSkillParam_Phoenix();
		}

		private void ReadHeroSkillParam_Wukong()
		{
			int num = 0;
			int num2 = 0;
			string text = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				HeroSkillParameter_0_0 heroSkillParameter_0_ = new HeroSkillParameter_0_0();
				for (int i = 0; i < list.Count; i++)
				{
					Hero0Skill0Param hero0Skill0Param = new Hero0Skill0Param();
					hero0Skill0Param.skill_level = (int)list[i]["skill_level"];
					hero0Skill0Param.number_clone = (int)list[i]["number_clone"];
					hero0Skill0Param.parameter_Scale = (int)list[i]["parameter_scale"];
					hero0Skill0Param.duration = (int)list[i]["duration"];
					hero0Skill0Param.cooldown_time = (int)list[i]["cooldown_time"];
					hero0Skill0Param.description = (string)list[i]["description"];
					hero0Skill0Param.use_type = (string)list[i]["use_type"];
					heroSkillParameter_0_.AddParamToList(hero0Skill0Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_0_);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
			num = 0;
			num2 = 1;
			string text2 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list2 = CSVReader.Read(text2);
				HeroSkillParameter_0_1 heroSkillParameter_0_2 = new HeroSkillParameter_0_1();
				for (int j = 0; j < list2.Count; j++)
				{
					Hero0Skill1Param hero0Skill1Param = new Hero0Skill1Param();
					hero0Skill1Param.skill_level = (int)list2[j]["skill_level"];
					hero0Skill1Param.damage = (int)list2[j]["damage"];
					hero0Skill1Param.duration = (int)list2[j]["duration"];
					hero0Skill1Param.change_percent = (int)list2[j]["change_percent"];
					hero0Skill1Param.aoeRange = (int)list2[j]["aoe_range"];
					hero0Skill1Param.description = (string)list2[j]["description"];
					heroSkillParameter_0_2.AddParamToList(hero0Skill1Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_0_2);
				}
			}
			catch (Exception)
			{
				ShowError(text2);
				throw;
			}
			num = 0;
			num2 = 2;
			string text3 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list3 = CSVReader.Read(text3);
				HeroSkillParameter_0_2 heroSkillParameter_0_3 = new HeroSkillParameter_0_2();
				for (int k = 0; k < list3.Count; k++)
				{
					Hero0Skill2Param hero0Skill2Param = new Hero0Skill2Param();
					hero0Skill2Param.skill_level = (int)list3[k]["skill_level"];
					hero0Skill2Param.armor = (int)list3[k]["armor"];
					hero0Skill2Param.description = (string)list3[k]["description"];
					heroSkillParameter_0_3.AddParamToList(hero0Skill2Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_0_3);
				}
			}
			catch (Exception)
			{
				ShowError(text3);
				throw;
			}
			num = 0;
			num2 = 3;
			string text4 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list4 = CSVReader.Read(text4);
				HeroSkillParameter_0_3 heroSkillParameter_0_4 = new HeroSkillParameter_0_3();
				for (int l = 0; l < list4.Count; l++)
				{
					Hero0Skill3Param hero0Skill3Param = new Hero0Skill3Param();
					hero0Skill3Param.skill_level = (int)list4[l]["skill_level"];
					hero0Skill3Param.skill_range = (int)list4[l]["skill_range"];
					hero0Skill3Param.armor_per_unit = (int)list4[l]["armor_per_unit"];
					hero0Skill3Param.armor_max = (int)list4[l]["armor_max"];
					hero0Skill3Param.description = (string)list4[l]["description"];
					heroSkillParameter_0_4.AddParamToList(hero0Skill3Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_0_4);
				}
			}
			catch (Exception)
			{
				ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_Ashi()
		{
			int num = 1;
			int num2 = 0;
			string text = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				HeroSkillParameter_1_0 heroSkillParameter_1_ = new HeroSkillParameter_1_0();
				for (int i = 0; i < list.Count; i++)
				{
					Hero1Skill0Param hero1Skill0Param = new Hero1Skill0Param();
					hero1Skill0Param.skill_level = (int)list[i]["skill_level"];
					hero1Skill0Param.number_of_projectile = (int)list[i]["number_of_projectile"];
					hero1Skill0Param.range = (int)list[i]["range"];
					hero1Skill0Param.offsetHigh = (int)list[i]["offset_high"];
					hero1Skill0Param.duration = (int)list[i]["duration"];
					hero1Skill0Param.delayTime = (int)list[i]["delay_time"];
					hero1Skill0Param.damage = (int)list[i]["damage"];
					hero1Skill0Param.cooldown_time = (int)list[i]["cooldown_time"];
					hero1Skill0Param.description = (string)list[i]["description"];
					hero1Skill0Param.use_type = (string)list[i]["use_type"];
					heroSkillParameter_1_.AddParamToList(hero1Skill0Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_1_);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
			num = 1;
			num2 = 1;
			string text2 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list2 = CSVReader.Read(text2);
				HeroSkillParameter_1_1 heroSkillParameter_1_2 = new HeroSkillParameter_1_1();
				for (int j = 0; j < list2.Count; j++)
				{
					Hero1Skill1Param hero1Skill1Param = new Hero1Skill1Param();
					hero1Skill1Param.skill_level = (int)list2[j]["skill_level"];
					hero1Skill1Param.bonus_crit = (int)list2[j]["ctitical_strike_change_bonus"];
					hero1Skill1Param.description = (string)list2[j]["description"];
					heroSkillParameter_1_2.AddParamToList(hero1Skill1Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_1_2);
				}
			}
			catch (Exception)
			{
				ShowError(text2);
				throw;
			}
			num = 1;
			num2 = 2;
			string text3 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list3 = CSVReader.Read(text3);
				HeroSkillParameter_1_2 heroSkillParameter_1_3 = new HeroSkillParameter_1_2();
				for (int k = 0; k < list3.Count; k++)
				{
					Hero1Skill2Param hero1Skill2Param = new Hero1Skill2Param();
					hero1Skill2Param.skill_level = (int)list3[k]["skill_level"];
					hero1Skill2Param.attack_speed_increase = (int)list3[k]["attack_speed_increase"];
					hero1Skill2Param.duration = (int)list3[k]["duration"];
					hero1Skill2Param.cooldown_time = (int)list3[k]["cooldown_time"];
					hero1Skill2Param.description = (string)list3[k]["description"];
					heroSkillParameter_1_3.AddParamToList(hero1Skill2Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_1_3);
				}
			}
			catch (Exception)
			{
				ShowError(text3);
				throw;
			}
			num = 1;
			num2 = 3;
			string text4 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list4 = CSVReader.Read(text4);
				HeroSkillParameter_1_3 heroSkillParameter_1_4 = new HeroSkillParameter_1_3();
				for (int l = 0; l < list4.Count; l++)
				{
					Hero1Skill3Param hero1Skill3Param = new Hero1Skill3Param();
					hero1Skill3Param.skill_level = (int)list4[l]["skill_level"];
					hero1Skill3Param.chance_to_cast = (int)list4[l]["chance_to_cast"];
					hero1Skill3Param.number_of_projectile = (int)list4[l]["number_of_projectile"];
					hero1Skill3Param.damage = (int)list4[l]["damage"];
					hero1Skill3Param.slow_percent = (int)list4[l]["slow_percent"];
					hero1Skill3Param.slow_time = (int)list4[l]["slow_time"];
					hero1Skill3Param.description = (string)list4[l]["description"];
					heroSkillParameter_1_4.AddParamToList(hero1Skill3Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_1_4);
				}
			}
			catch (Exception)
			{
				ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_Galahad()
		{
			int num = 2;
			int num2 = 0;
			string text = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				HeroSkillParameter_2_0 heroSkillParameter_2_ = new HeroSkillParameter_2_0();
				for (int i = 0; i < list.Count; i++)
				{
					Hero2Skill0Param hero2Skill0Param = new Hero2Skill0Param();
					hero2Skill0Param.skill_level = (int)list[i]["skill_level"];
					hero2Skill0Param.number_clone = (int)list[i]["number_clone"];
					hero2Skill0Param.parameter_Scale = (int)list[i]["parameter_scale"];
					hero2Skill0Param.duration = (int)list[i]["duration"];
					hero2Skill0Param.cooldown_time = (int)list[i]["cooldown_time"];
					hero2Skill0Param.description = (string)list[i]["description"];
					hero2Skill0Param.use_type = (string)list[i]["use_type"];
					heroSkillParameter_2_.AddParamToList(hero2Skill0Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_2_);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
			num = 2;
			num2 = 1;
			string text2 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list2 = CSVReader.Read(text2);
				HeroSkillParameter_2_1 heroSkillParameter_2_2 = new HeroSkillParameter_2_1();
				for (int j = 0; j < list2.Count; j++)
				{
					Hero2Skill1Param hero2Skill1Param = new Hero2Skill1Param();
					hero2Skill1Param.skill_level = (int)list2[j]["skill_level"];
					hero2Skill1Param.armorBonus = (int)list2[j]["armor_bonus"];
					hero2Skill1Param.duration = (int)list2[j]["duration"];
					hero2Skill1Param.cooldown_time = (int)list2[j]["cooldown_time"];
					hero2Skill1Param.description = (string)list2[j]["description"];
					heroSkillParameter_2_2.AddParamToList(hero2Skill1Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_2_2);
				}
			}
			catch (Exception)
			{
				ShowError(text2);
				throw;
			}
			num = 2;
			num2 = 2;
			string text3 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list3 = CSVReader.Read(text3);
				HeroSkillParameter_2_2 heroSkillParameter_2_3 = new HeroSkillParameter_2_2();
				for (int k = 0; k < list3.Count; k++)
				{
					Hero2Skill2Param hero2Skill2Param = new Hero2Skill2Param();
					hero2Skill2Param.skill_level = (int)list3[k]["skill_level"];
					hero2Skill2Param.count_crit = (int)list3[k]["count_crit"];
					hero2Skill2Param.description = (string)list3[k]["description"];
					heroSkillParameter_2_3.AddParamToList(hero2Skill2Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_2_3);
				}
			}
			catch (Exception)
			{
				ShowError(text3);
				throw;
			}
			num = 2;
			num2 = 3;
			string text4 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list4 = CSVReader.Read(text4);
				HeroSkillParameter_2_3 heroSkillParameter_2_4 = new HeroSkillParameter_2_3();
				for (int l = 0; l < list4.Count; l++)
				{
					Hero2Skill3Param hero2Skill3Param = new Hero2Skill3Param();
					hero2Skill3Param.skill_level = (int)list4[l]["skill_level"];
					hero2Skill3Param.aoeRange = (int)list4[l]["aoe_range"];
					hero2Skill3Param.damage = (int)list4[l]["damage"];
					hero2Skill3Param.slow_value = (int)list4[l]["slow_value"];
					hero2Skill3Param.slow_duration = (int)list4[l]["slow_duration"];
					hero2Skill3Param.cooldown_time = (int)list4[l]["cooldown"];
					hero2Skill3Param.description = (string)list4[l]["description"];
					heroSkillParameter_2_4.AddParamToList(hero2Skill3Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_2_4);
				}
			}
			catch (Exception)
			{
				ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_ShamanKing()
		{
			int num = 3;
			int num2 = 0;
			string text = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				HeroSkillParameter_3_0 heroSkillParameter_3_ = new HeroSkillParameter_3_0();
				for (int i = 0; i < list.Count; i++)
				{
					Hero3Skill0Param hero3Skill0Param = new Hero3Skill0Param();
					hero3Skill0Param.skill_level = (int)list[i]["skill_level"];
					hero3Skill0Param.physics_damage = (int)list[i]["physics_damage"];
					hero3Skill0Param.magic_damage = (int)list[i]["magic_damage"];
					hero3Skill0Param.skill_range = (int)list[i]["skill_range"];
					hero3Skill0Param.meteor_speed = (int)list[i]["meteor_speed"];
					hero3Skill0Param.duration = (int)list[i]["duration"];
					hero3Skill0Param.cooldown_time = (int)list[i]["cooldown_time"];
					hero3Skill0Param.description = (string)list[i]["description"];
					hero3Skill0Param.use_type = (string)list[i]["use_type"];
					heroSkillParameter_3_.AddParamToList(hero3Skill0Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_3_);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
			num = 3;
			num2 = 1;
			string text2 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list2 = CSVReader.Read(text2);
				HeroSkillParameter_3_1 heroSkillParameter_3_2 = new HeroSkillParameter_3_1();
				for (int j = 0; j < list2.Count; j++)
				{
					Hero3Skill1Param hero3Skill1Param = new Hero3Skill1Param();
					hero3Skill1Param.skill_level = (int)list2[j]["skill_level"];
					hero3Skill1Param.magic_damage_bonus = (int)list2[j]["magic_damage_bonus"];
					hero3Skill1Param.attack_speed_increase = (int)list2[j]["attack_speed_increase"];
					hero3Skill1Param.duration = (int)list2[j]["duration"];
					hero3Skill1Param.cooldown_time = (int)list2[j]["cooldown_time"];
					hero3Skill1Param.description = (string)list2[j]["description"];
					heroSkillParameter_3_2.AddParamToList(hero3Skill1Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_3_2);
				}
			}
			catch (Exception)
			{
				ShowError(text2);
				throw;
			}
			num = 3;
			num2 = 2;
			string text3 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list3 = CSVReader.Read(text3);
				HeroSkillParameter_3_2 heroSkillParameter_3_3 = new HeroSkillParameter_3_2();
				for (int k = 0; k < list3.Count; k++)
				{
					Hero3Skill2Param hero3Skill2Param = new Hero3Skill2Param();
					hero3Skill2Param.skill_level = (int)list3[k]["skill_level"];
					hero3Skill2Param.skill_range = (int)list3[k]["skill_range"];
					hero3Skill2Param.slow_percent = (int)list3[k]["slow_percent"];
					hero3Skill2Param.damage_burn = (int)list3[k]["damage_burn"];
					hero3Skill2Param.duration = (int)list3[k]["duration"];
					hero3Skill2Param.cooldown_time = (int)list3[k]["cooldown_time"];
					hero3Skill2Param.description = (string)list3[k]["description"];
					heroSkillParameter_3_3.AddParamToList(hero3Skill2Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_3_3);
				}
			}
			catch (Exception)
			{
				ShowError(text3);
				throw;
			}
			num = 3;
			num2 = 3;
			string text4 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list4 = CSVReader.Read(text4);
				HeroSkillParameter_3_3 heroSkillParameter_3_4 = new HeroSkillParameter_3_3();
				for (int l = 0; l < list4.Count; l++)
				{
					Hero3Skill3Param hero3Skill3Param = new Hero3Skill3Param();
					hero3Skill3Param.skill_level = (int)list4[l]["skill_level"];
					hero3Skill3Param.skill_range = (int)list4[l]["skill_range"];
					hero3Skill3Param.physics_damage = (int)list4[l]["physics_damage"];
					hero3Skill3Param.magic_damage = (int)list4[l]["magic_damage"];
					hero3Skill3Param.time_step = (int)list4[l]["time_step"];
					hero3Skill3Param.cooldown_time = (int)list4[l]["cooldown_time"];
					hero3Skill3Param.description = (string)list4[l]["description"];
					heroSkillParameter_3_4.AddParamToList(hero3Skill3Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_3_4);
				}
			}
			catch (Exception)
			{
				ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_Golem()
		{
			int num = 4;
			int num2 = 0;
			string text = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				HeroSkillParameter_4_0 heroSkillParameter_4_ = new HeroSkillParameter_4_0();
				for (int i = 0; i < list.Count; i++)
				{
					Hero4Skill0Param hero4Skill0Param = new Hero4Skill0Param();
					hero4Skill0Param.skill_level = (int)list[i]["skill_level"];
					hero4Skill0Param.physics_damage = (int)list[i]["physics_damage"];
					hero4Skill0Param.skill_range = (int)list[i]["skill_range"];
					hero4Skill0Param.duration = (int)list[i]["duration"];
					hero4Skill0Param.cooldown_time = (int)list[i]["cooldown_time"];
					hero4Skill0Param.description = (string)list[i]["description"];
					hero4Skill0Param.use_type = (string)list[i]["use_type"];
					heroSkillParameter_4_.AddParamToList(hero4Skill0Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_4_);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
			num = 4;
			num2 = 1;
			string text2 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list2 = CSVReader.Read(text2);
				HeroSkillParameter_4_1 heroSkillParameter_4_2 = new HeroSkillParameter_4_1();
				for (int j = 0; j < list2.Count; j++)
				{
					Hero4Skill1Param hero4Skill1Param = new Hero4Skill1Param();
					hero4Skill1Param.skill_level = (int)list2[j]["skill_level"];
					hero4Skill1Param.physics_armor_bonus = (int)list2[j]["physics_armor_bonus"];
					hero4Skill1Param.magic_armor_bonus = (int)list2[j]["magic_armor_bonus"];
					hero4Skill1Param.duration = (int)list2[j]["duration"];
					hero4Skill1Param.cooldown_time = (int)list2[j]["cooldown_time"];
					hero4Skill1Param.description = (string)list2[j]["description"];
					heroSkillParameter_4_2.AddParamToList(hero4Skill1Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_4_2);
				}
			}
			catch (Exception)
			{
				ShowError(text2);
				throw;
			}
			num = 4;
			num2 = 2;
			string text3 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list3 = CSVReader.Read(text3);
				HeroSkillParameter_4_2 heroSkillParameter_4_3 = new HeroSkillParameter_4_2();
				for (int k = 0; k < list3.Count; k++)
				{
					Hero4Skill2Param hero4Skill2Param = new Hero4Skill2Param();
					hero4Skill2Param.skill_level = (int)list3[k]["skill_level"];
					hero4Skill2Param.change_to_stun = (int)list3[k]["change_to_stun"];
					hero4Skill2Param.duration = (int)list3[k]["duration"];
					hero4Skill2Param.description = (string)list3[k]["description"];
					heroSkillParameter_4_3.AddParamToList(hero4Skill2Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_4_3);
				}
			}
			catch (Exception)
			{
				ShowError(text3);
				throw;
			}
			num = 4;
			num2 = 3;
			string text4 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list4 = CSVReader.Read(text4);
				HeroSkillParameter_4_3 heroSkillParameter_4_4 = new HeroSkillParameter_4_3();
				for (int l = 0; l < list4.Count; l++)
				{
					Hero4Skill3Param hero4Skill3Param = new Hero4Skill3Param();
					hero4Skill3Param.skill_level = (int)list4[l]["skill_level"];
					hero4Skill3Param.general_attack_damage_bonus = (int)list4[l]["general_attack_damage_bonus"];
					hero4Skill3Param.attack_speed_bonus = (int)list4[l]["attack_speed_bonus"];
					hero4Skill3Param.movement_speed_bonus = (int)list4[l]["movement_speed_bonus"];
					hero4Skill3Param.duration = (int)list4[l]["duration"];
					hero4Skill3Param.cooldown_time = (int)list4[l]["cooldown_time"];
					hero4Skill3Param.description = (string)list4[l]["description"];
					heroSkillParameter_4_4.AddParamToList(hero4Skill3Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_4_4);
				}
			}
			catch (Exception)
			{
				ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_NatureQueen()
		{
			int num = 5;
			int num2 = 0;
			string text = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				HeroSkillParameter_5_0 heroSkillParameter_5_ = new HeroSkillParameter_5_0();
				for (int i = 0; i < list.Count; i++)
				{
					Hero5Skill0Param hero5Skill0Param = new Hero5Skill0Param();
					hero5Skill0Param.skill_level = (int)list[i]["skill_level"];
					hero5Skill0Param.heal_amount = (int)list[i]["heal_amount"];
					hero5Skill0Param.skill_range = (int)list[i]["skill_range"];
					hero5Skill0Param.cooldown_time = (int)list[i]["cooldown_time"];
					hero5Skill0Param.description = (string)list[i]["description"];
					hero5Skill0Param.use_type = (string)list[i]["use_type"];
					heroSkillParameter_5_.AddParamToList(hero5Skill0Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_5_);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
			num = 5;
			num2 = 1;
			string text2 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list2 = CSVReader.Read(text2);
				HeroSkillParameter_5_1 heroSkillParameter_5_2 = new HeroSkillParameter_5_1();
				for (int j = 0; j < list2.Count; j++)
				{
					Hero5Skill1Param hero5Skill1Param = new Hero5Skill1Param();
					hero5Skill1Param.skill_level = (int)list2[j]["skill_level"];
					hero5Skill1Param.bonus_damage = (int)list2[j]["bonus_damage"];
					hero5Skill1Param.duration = (int)list2[j]["duration"];
					hero5Skill1Param.description = (string)list2[j]["description"];
					heroSkillParameter_5_2.AddParamToList(hero5Skill1Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_5_2);
				}
			}
			catch (Exception)
			{
				ShowError(text2);
				throw;
			}
			num = 5;
			num2 = 2;
			string text3 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list3 = CSVReader.Read(text3);
				HeroSkillParameter_5_2 heroSkillParameter_5_3 = new HeroSkillParameter_5_2();
				for (int k = 0; k < list3.Count; k++)
				{
					Hero5Skill2Param hero5Skill2Param = new Hero5Skill2Param();
					hero5Skill2Param.skill_level = (int)list3[k]["skill_level"];
					hero5Skill2Param.skill_range = (int)list3[k]["skill_range"];
					hero5Skill2Param.enemy_affected = (int)list3[k]["enemy_affectd"];
					hero5Skill2Param.enemy_min = (int)list3[k]["enemy_min"];
					hero5Skill2Param.duration = (int)list3[k]["duration"];
					hero5Skill2Param.cooldown_time = (int)list3[k]["cooldown_time"];
					hero5Skill2Param.description = (string)list3[k]["description"];
					heroSkillParameter_5_3.AddParamToList(hero5Skill2Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_5_3);
				}
			}
			catch (Exception)
			{
				ShowError(text3);
				throw;
			}
			num = 5;
			num2 = 3;
			string text4 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list4 = CSVReader.Read(text4);
				HeroSkillParameter_5_3 heroSkillParameter_5_4 = new HeroSkillParameter_5_3();
				for (int l = 0; l < list4.Count; l++)
				{
					Hero5Skill3Param hero5Skill3Param = new Hero5Skill3Param();
					hero5Skill3Param.skill_level = (int)list4[l]["skill_level"];
					hero5Skill3Param.arrow_number = (int)list4[l]["arrow_number"];
					hero5Skill3Param.skill_range = (int)list4[l]["skill_range"];
					hero5Skill3Param.damage_physics = (int)list4[l]["damage_physics"];
					hero5Skill3Param.duration = (int)list4[l]["duration"];
					hero5Skill3Param.delay_time = (int)list4[l]["delay_time"];
					hero5Skill3Param.cooldown_time = (int)list4[l]["cooldown_time"];
					hero5Skill3Param.description = (string)list4[l]["description"];
					heroSkillParameter_5_4.AddParamToList(hero5Skill3Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_5_4);
				}
			}
			catch (Exception)
			{
				ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_Thor()
		{
			int num = 6;
			int num2 = 0;
			string text = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				HeroSkillParameter_6_0 heroSkillParameter_6_ = new HeroSkillParameter_6_0();
				for (int i = 0; i < list.Count; i++)
				{
					Hero6Skill0Param hero6Skill0Param = new Hero6Skill0Param();
					hero6Skill0Param.skill_level = (int)list[i]["skill_level"];
					hero6Skill0Param.physics_damage = (int)list[i]["physics_damage"];
					hero6Skill0Param.magic_damage = (int)list[i]["magic_damage"];
					hero6Skill0Param.stun_duration = (int)list[i]["stun_duration"];
					hero6Skill0Param.skill_range = (int)list[i]["skill_range"];
					hero6Skill0Param.cooldown_time = (int)list[i]["cooldown_time"];
					hero6Skill0Param.description = (string)list[i]["description"];
					hero6Skill0Param.use_type = (string)list[i]["use_type"];
					heroSkillParameter_6_.AddParamToList(hero6Skill0Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_6_);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
			num = 6;
			num2 = 1;
			string text2 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list2 = CSVReader.Read(text2);
				HeroSkillParameter_6_1 heroSkillParameter_6_2 = new HeroSkillParameter_6_1();
				for (int j = 0; j < list2.Count; j++)
				{
					Hero6Skill1Param hero6Skill1Param = new Hero6Skill1Param();
					hero6Skill1Param.skill_level = (int)list2[j]["skill_level"];
					hero6Skill1Param.percent_attack_damage_bonus = (int)list2[j]["percent_attack_damage_bonus"];
					hero6Skill1Param.duration = (int)list2[j]["duration"];
					hero6Skill1Param.cooldown_time = (int)list2[j]["cooldown_time"];
					hero6Skill1Param.description = (string)list2[j]["description"];
					heroSkillParameter_6_2.AddParamToList(hero6Skill1Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_6_2);
				}
			}
			catch (Exception)
			{
				ShowError(text2);
				throw;
			}
			num = 6;
			num2 = 2;
			string text3 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list3 = CSVReader.Read(text3);
				HeroSkillParameter_6_2 heroSkillParameter_6_3 = new HeroSkillParameter_6_2();
				for (int k = 0; k < list3.Count; k++)
				{
					Hero6Skill2Param hero6Skill2Param = new Hero6Skill2Param();
					hero6Skill2Param.skill_level = (int)list3[k]["skill_level"];
					hero6Skill2Param.aoe_range = (int)list3[k]["aoe_range"];
					hero6Skill2Param.physics_damage = (int)list3[k]["physics_damage"];
					hero6Skill2Param.magic_damage = (int)list3[k]["magic_damage"];
					hero6Skill2Param.cooldown_time = (int)list3[k]["cooldown_time"];
					hero6Skill2Param.description = (string)list3[k]["description"];
					heroSkillParameter_6_3.AddParamToList(hero6Skill2Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_6_3);
				}
			}
			catch (Exception)
			{
				ShowError(text3);
				throw;
			}
			num = 6;
			num2 = 3;
			string text4 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list4 = CSVReader.Read(text4);
				HeroSkillParameter_6_3 heroSkillParameter_6_4 = new HeroSkillParameter_6_3();
				for (int l = 0; l < list4.Count; l++)
				{
					Hero6Skill3Param hero6Skill3Param = new Hero6Skill3Param();
					hero6Skill3Param.skill_level = (int)list4[l]["skill_level"];
					hero6Skill3Param.physics_damage = (int)list4[l]["physics_damage"];
					hero6Skill3Param.magic_damage = (int)list4[l]["magic_damage"];
					hero6Skill3Param.skill_range = (int)list4[l]["skill_range"];
					hero6Skill3Param.cooldown_time = (int)list4[l]["cooldown_time"];
					hero6Skill3Param.description = (string)list4[l]["description"];
					heroSkillParameter_6_4.AddParamToList(hero6Skill3Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_6_4);
				}
			}
			catch (Exception)
			{
				ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_Ninja()
		{
			int num = 7;
			int num2 = 0;
			string text = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				HeroSkillParameter_7_0 heroSkillParameter_7_ = new HeroSkillParameter_7_0();
				for (int i = 0; i < list.Count; i++)
				{
					Hero7Skill0Param hero7Skill0Param = new Hero7Skill0Param();
					hero7Skill0Param.skill_level = (int)list[i]["skill_level"];
					hero7Skill0Param.physics_damage = (int)list[i]["physics_damage"];
					hero7Skill0Param.magic_damage = (int)list[i]["magic_damage"];
					hero7Skill0Param.skill_range = (int)list[i]["skill_range"];
					hero7Skill0Param.cooldown_time = (int)list[i]["cooldown_time"];
					hero7Skill0Param.description = (string)list[i]["description"];
					hero7Skill0Param.use_type = (string)list[i]["use_type"];
					heroSkillParameter_7_.AddParamToList(hero7Skill0Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_7_);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
			num = 7;
			num2 = 1;
			string text2 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list2 = CSVReader.Read(text2);
				HeroSkillParameter_7_1 heroSkillParameter_7_2 = new HeroSkillParameter_7_1();
				for (int j = 0; j < list2.Count; j++)
				{
					Hero7Skill1Param hero7Skill1Param = new Hero7Skill1Param();
					hero7Skill1Param.skill_level = (int)list2[j]["skill_level"];
					hero7Skill1Param.percent_health_activate = (int)list2[j]["percent_health_activate"];
					hero7Skill1Param.amount_of_trap = (int)list2[j]["amount_of_trap"];
					hero7Skill1Param.trap_life_time = (int)list2[j]["trap_life_time"];
					hero7Skill1Param.physics_damage = (int)list2[j]["physics_damage"];
					hero7Skill1Param.magic_damage = (int)list2[j]["magic_damage"];
					hero7Skill1Param.slow_percent = (int)list2[j]["slow_percent"];
					hero7Skill1Param.slow_duration = (int)list2[j]["slow_duration"];
					hero7Skill1Param.skill_range = (int)list2[j]["skill_range"];
					hero7Skill1Param.cooldown_time = (int)list2[j]["cooldown_time"];
					hero7Skill1Param.description = (string)list2[j]["description"];
					heroSkillParameter_7_2.AddParamToList(hero7Skill1Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_7_2);
				}
			}
			catch (Exception)
			{
				ShowError(text2);
				throw;
			}
			num = 7;
			num2 = 2;
			string text3 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list3 = CSVReader.Read(text3);
				HeroSkillParameter_7_2 heroSkillParameter_7_3 = new HeroSkillParameter_7_2();
				for (int k = 0; k < list3.Count; k++)
				{
					Hero7Skill2Param hero7Skill2Param = new Hero7Skill2Param();
					hero7Skill2Param.skill_level = (int)list3[k]["skill_level"];
					hero7Skill2Param.chance_to_cast = (int)list3[k]["chance_to_cast"];
					hero7Skill2Param.skill_range = (int)list3[k]["skill_range"];
					hero7Skill2Param.physics_damage = (int)list3[k]["physics_damage"];
					hero7Skill2Param.magic_damage = (int)list3[k]["magic_damage"];
					hero7Skill2Param.cooldown_time = (int)list3[k]["cooldown_time"];
					hero7Skill2Param.description = (string)list3[k]["description"];
					heroSkillParameter_7_3.AddParamToList(hero7Skill2Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_7_3);
				}
			}
			catch (Exception)
			{
				ShowError(text3);
				throw;
			}
			num = 7;
			num2 = 3;
			string text4 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list4 = CSVReader.Read(text4);
				HeroSkillParameter_7_3 heroSkillParameter_7_4 = new HeroSkillParameter_7_3();
				for (int l = 0; l < list4.Count; l++)
				{
					Hero7Skill3Param hero7Skill3Param = new Hero7Skill3Param();
					hero7Skill3Param.skill_level = (int)list4[l]["skill_level"];
					hero7Skill3Param.chance_to_cast = (int)list4[l]["chance_to_cast"];
					hero7Skill3Param.physics_damage = (int)list4[l]["physics_damage"];
					hero7Skill3Param.magic_damage = (int)list4[l]["magic_damage"];
					hero7Skill3Param.skill_range = (int)list4[l]["skill_range"];
					hero7Skill3Param.countdown_time = (int)list4[l]["countdown_time"];
					hero7Skill3Param.cooldown_time = (int)list4[l]["cooldown_time"];
					hero7Skill3Param.description = (string)list4[l]["description"];
					heroSkillParameter_7_4.AddParamToList(hero7Skill3Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_7_4);
				}
			}
			catch (Exception)
			{
				ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_Tristana()
		{
			int num = 8;
			int num2 = 0;
			string text = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				HeroSkillParameter_8_0 heroSkillParameter_8_ = new HeroSkillParameter_8_0();
				for (int i = 0; i < list.Count; i++)
				{
					Hero8Skill0Param hero8Skill0Param = new Hero8Skill0Param();
					hero8Skill0Param.skill_level = (int)list[i]["skill_level"];
					hero8Skill0Param.number_of_projectile = (int)list[i]["number_of_projectile"];
					hero8Skill0Param.physics_damage = (int)list[i]["physics_damage"];
					hero8Skill0Param.magic_damage = (int)list[i]["magic_damage"];
					hero8Skill0Param.skill_range = (int)list[i]["skill_range"];
					hero8Skill0Param.cooldown_time = (int)list[i]["cooldown_time"];
					hero8Skill0Param.description = (string)list[i]["description"];
					hero8Skill0Param.use_type = (string)list[i]["use_type"];
					heroSkillParameter_8_.AddParamToList(hero8Skill0Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_8_);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
			num = 8;
			num2 = 1;
			string text2 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list2 = CSVReader.Read(text2);
				HeroSkillParameter_8_1 heroSkillParameter_8_2 = new HeroSkillParameter_8_1();
				for (int j = 0; j < list2.Count; j++)
				{
					Hero8Skill1Param hero8Skill1Param = new Hero8Skill1Param();
					hero8Skill1Param.skill_level = (int)list2[j]["skill_level"];
					hero8Skill1Param.physics_damage = (int)list2[j]["physics_damage"];
					hero8Skill1Param.magic_damage = (int)list2[j]["magic_damage"];
					hero8Skill1Param.attack_damage_decrease_percentage = (int)list2[j]["attack_damage_decrease_percentage"];
					hero8Skill1Param.duration = (int)list2[j]["duration"];
					hero8Skill1Param.knock_back_distance = (int)list2[j]["knock_back_distance"];
					hero8Skill1Param.skill_range = (int)list2[j]["skill_range"];
					hero8Skill1Param.cooldown_time = (int)list2[j]["cooldown_time"];
					hero8Skill1Param.description = (string)list2[j]["description"];
					heroSkillParameter_8_2.AddParamToList(hero8Skill1Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_8_2);
				}
			}
			catch (Exception)
			{
				ShowError(text2);
				throw;
			}
			num = 8;
			num2 = 2;
			string text3 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list3 = CSVReader.Read(text3);
				HeroSkillParameter_8_2 heroSkillParameter_8_3 = new HeroSkillParameter_8_2();
				for (int k = 0; k < list3.Count; k++)
				{
					Hero8Skill2Param hero8Skill2Param = new Hero8Skill2Param();
					hero8Skill2Param.skill_level = (int)list3[k]["skill_level"];
					hero8Skill2Param.skill_range = (int)list3[k]["skill_range"];
					hero8Skill2Param.duration = (int)list3[k]["duration"];
					hero8Skill2Param.cooldown_time = (int)list3[k]["cooldown_time"];
					hero8Skill2Param.description = (string)list3[k]["description"];
					heroSkillParameter_8_3.AddParamToList(hero8Skill2Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_8_3);
				}
			}
			catch (Exception)
			{
				ShowError(text3);
				throw;
			}
			num = 8;
			num2 = 3;
			string text4 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list4 = CSVReader.Read(text4);
				HeroSkillParameter_8_3 heroSkillParameter_8_4 = new HeroSkillParameter_8_3();
				for (int l = 0; l < list4.Count; l++)
				{
					Hero8Skill3Param hero8Skill3Param = new Hero8Skill3Param();
					hero8Skill3Param.skill_level = (int)list4[l]["skill_level"];
					hero8Skill3Param.attack_range_bonus_percentage = (int)list4[l]["attack_range_bonus_percentage"];
					hero8Skill3Param.description = (string)list4[l]["description"];
					heroSkillParameter_8_4.AddParamToList(hero8Skill3Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_8_4);
				}
			}
			catch (Exception)
			{
				ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_JungleLord()
		{
			int num = 9;
			int num2 = 0;
			string text = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				HeroSkillParameter_9_0 heroSkillParameter_9_ = new HeroSkillParameter_9_0();
				for (int i = 0; i < list.Count; i++)
				{
					Hero9Skill0Param hero9Skill0Param = new Hero9Skill0Param();
					hero9Skill0Param.skill_level = (int)list[i]["skill_level"];
					hero9Skill0Param.physics_damage = (int)list[i]["physics_damage"];
					hero9Skill0Param.magic_damage = (int)list[i]["magic_damage"];
					hero9Skill0Param.number_of_minion = (int)list[i]["number_of_minion"];
					hero9Skill0Param.minion_attack_range = (int)list[i]["minion_attack_range"];
					hero9Skill0Param.minion_attack_cooldown = (int)list[i]["minion_attack_cooldown"];
					hero9Skill0Param.minion_lifetime = (int)list[i]["minion_lifetime"];
					hero9Skill0Param.skill_range = (int)list[i]["skill_range"];
					hero9Skill0Param.cooldown_time = (int)list[i]["cooldown_time"];
					hero9Skill0Param.description = (string)list[i]["description"];
					hero9Skill0Param.use_type = (string)list[i]["use_type"];
					heroSkillParameter_9_.AddParamToList(hero9Skill0Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_9_);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
			num = 9;
			num2 = 1;
			string text2 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list2 = CSVReader.Read(text2);
				HeroSkillParameter_9_1 heroSkillParameter_9_2 = new HeroSkillParameter_9_1();
				for (int j = 0; j < list2.Count; j++)
				{
					Hero9Skill1Param hero9Skill1Param = new Hero9Skill1Param();
					hero9Skill1Param.skill_level = (int)list2[j]["skill_level"];
					hero9Skill1Param.enemy_affected = (int)list2[j]["enemy_affected"];
					hero9Skill1Param.knock_back_distance = (int)list2[j]["knock_back_distance"];
					hero9Skill1Param.knock_back_duration = (int)list2[j]["knock_back_duration"];
					hero9Skill1Param.skill_range = (int)list2[j]["skill_range"];
					hero9Skill1Param.cooldown_time = (int)list2[j]["cooldown_time"];
					hero9Skill1Param.description = (string)list2[j]["description"];
					heroSkillParameter_9_2.AddParamToList(hero9Skill1Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_9_2);
				}
			}
			catch (Exception)
			{
				ShowError(text2);
				throw;
			}
			num = 9;
			num2 = 2;
			string text3 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list3 = CSVReader.Read(text3);
				HeroSkillParameter_9_2 heroSkillParameter_9_3 = new HeroSkillParameter_9_2();
				for (int k = 0; k < list3.Count; k++)
				{
					Hero9Skill2Param hero9Skill2Param = new Hero9Skill2Param();
					hero9Skill2Param.skill_level = (int)list3[k]["skill_level"];
					hero9Skill2Param.health_percentage_active = (int)list3[k]["health_percentage_active"];
					hero9Skill2Param.health_amount = (int)list3[k]["health_amount"];
					hero9Skill2Param.duration = (int)list3[k]["duration"];
					hero9Skill2Param.cooldown_time = (int)list3[k]["cooldown_time"];
					hero9Skill2Param.description = (string)list3[k]["description"];
					heroSkillParameter_9_3.AddParamToList(hero9Skill2Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_9_3);
				}
			}
			catch (Exception)
			{
				ShowError(text3);
				throw;
			}
			num = 9;
			num2 = 3;
			string text4 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list4 = CSVReader.Read(text4);
				HeroSkillParameter_9_3 heroSkillParameter_9_4 = new HeroSkillParameter_9_3();
				for (int l = 0; l < list4.Count; l++)
				{
					Hero9Skill3Param hero9Skill3Param = new Hero9Skill3Param();
					hero9Skill3Param.skill_level = (int)list4[l]["skill_level"];
					hero9Skill3Param.number_clone = (int)list4[l]["number_clone"];
					hero9Skill3Param.parameter_Scale = (int)list4[l]["parameter_Scale"];
					hero9Skill3Param.duration = (int)list4[l]["duration"];
					hero9Skill3Param.cooldown_time = (int)list4[l]["cooldown_time"];
					hero9Skill3Param.description = (string)list4[l]["description"];
					heroSkillParameter_9_4.AddParamToList(hero9Skill3Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_9_4);
				}
			}
			catch (Exception)
			{
				ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_SteelDragon()
		{
			int num = 10;
			int num2 = 0;
			string text = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				HeroSkillParameter_10_0 heroSkillParameter_10_ = new HeroSkillParameter_10_0();
				for (int i = 0; i < list.Count; i++)
				{
					Hero10Skill0Param hero10Skill0Param = new Hero10Skill0Param();
					hero10Skill0Param.skill_level = (int)list[i]["skill_level"];
					hero10Skill0Param.skill_range = (int)list[i]["skill_range"];
					hero10Skill0Param.duration = (int)list[i]["duration"];
					hero10Skill0Param.magic_damage = (int)list[i]["magic_damage"];
					hero10Skill0Param.cooldown_time = (int)list[i]["cooldown_time"];
					hero10Skill0Param.use_type = (string)list[i]["use_type"];
					heroSkillParameter_10_.AddParamToList(hero10Skill0Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_10_);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
			num2 = 1;
			string text2 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list2 = CSVReader.Read(text2);
				HeroSkillParameter_10_1 heroSkillParameter_10_2 = new HeroSkillParameter_10_1();
				for (int j = 0; j < list2.Count; j++)
				{
					Hero10Skill1Param hero10Skill1Param = new Hero10Skill1Param();
					hero10Skill1Param.skill_level = (int)list2[j]["skill_level"];
					hero10Skill1Param.skill_range = (int)list2[j]["skill_range"];
					hero10Skill1Param.magic_damage = (int)list2[j]["magic_damage"];
					hero10Skill1Param.def_down_duration = (int)list2[j]["def_down_duration"];
					hero10Skill1Param.def_down_percent = (int)list2[j]["def_down_percent"];
					hero10Skill1Param.cooldown_time = (int)list2[j]["cooldown_time"];
					heroSkillParameter_10_2.AddParamToList(hero10Skill1Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_10_2);
				}
			}
			catch (Exception)
			{
				ShowError(text2);
				throw;
			}
			num2 = 2;
			string text3 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list3 = CSVReader.Read(text3);
				HeroSkillParameter_10_2 heroSkillParameter_10_3 = new HeroSkillParameter_10_2();
				for (int k = 0; k < list3.Count; k++)
				{
					Hero10Skill2Param hero10Skill2Param = new Hero10Skill2Param();
					hero10Skill2Param.skill_level = (int)list3[k]["skill_level"];
					hero10Skill2Param.physic_damage = (int)list3[k]["physic_damage"];
					hero10Skill2Param.cooldown_time = (int)list3[k]["cooldown_time"];
					heroSkillParameter_10_3.AddParamToList(hero10Skill2Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_10_3);
				}
			}
			catch (Exception)
			{
				ShowError(text3);
				throw;
			}
			num2 = 3;
			string text4 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list4 = CSVReader.Read(text4);
				HeroSkillParameter_10_3 heroSkillParameter_10_4 = new HeroSkillParameter_10_3();
				for (int l = 0; l < list4.Count; l++)
				{
					Hero10Skill3Param hero10Skill3Param = new Hero10Skill3Param();
					hero10Skill3Param.skill_level = (int)list4[l]["skill_level"];
					hero10Skill3Param.magic_damage = (int)list4[l]["magic_damage"];
					hero10Skill3Param.skill_range = (int)list4[l]["skill_range"];
					hero10Skill3Param.stun_duration = (int)list4[l]["stun_duration"];
					hero10Skill3Param.cooldown_time = (int)list4[l]["cooldown_time"];
					heroSkillParameter_10_4.AddParamToList(hero10Skill3Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_10_4);
				}
			}
			catch (Exception)
			{
				ShowError(text4);
				throw;
			}
		}

		private void ReadHeroSkillParam_Phoenix()
		{
			int num = 11;
			int num2 = 0;
			string text = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				HeroSkillParameter_11_0 heroSkillParameter_11_ = new HeroSkillParameter_11_0();
				for (int i = 0; i < list.Count; i++)
				{
					Hero11Skill0Param hero11Skill0Param = new Hero11Skill0Param();
					hero11Skill0Param.skill_level = (int)list[i]["skill_level"];
					hero11Skill0Param.skill_range = (int)list[i]["skill_range"];
					hero11Skill0Param.fire_road_duration = (int)list[i]["fire_road_duration"];
					hero11Skill0Param.magic_damage = (int)list[i]["magic_damage"];
					hero11Skill0Param.cooldown_time = (int)list[i]["cooldown_time"];
					hero11Skill0Param.use_type = (string)list[i]["use_type"];
					heroSkillParameter_11_.AddParamToList(hero11Skill0Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_11_);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
			num2 = 1;
			string text2 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list2 = CSVReader.Read(text2);
				HeroSkillParameter_11_1 heroSkillParameter_11_2 = new HeroSkillParameter_11_1();
				for (int j = 0; j < list2.Count; j++)
				{
					Hero11Skill1Param hero11Skill1Param = new Hero11Skill1Param();
					hero11Skill1Param.skill_level = (int)list2[j]["skill_level"];
					hero11Skill1Param.skill_range = (int)list2[j]["skill_range"];
					hero11Skill1Param.magic_damage = (int)list2[j]["magic_damage"];
					hero11Skill1Param.cooldown_time = (int)list2[j]["cooldown_time"];
					heroSkillParameter_11_2.AddParamToList(hero11Skill1Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_11_2);
				}
			}
			catch (Exception)
			{
				ShowError(text2);
				throw;
			}
			num2 = 2;
			string text3 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list3 = CSVReader.Read(text3);
				HeroSkillParameter_11_2 heroSkillParameter_11_3 = new HeroSkillParameter_11_2();
				for (int k = 0; k < list3.Count; k++)
				{
					Hero11Skill2Param hero11Skill2Param = new Hero11Skill2Param();
					hero11Skill2Param.skill_level = (int)list3[k]["skill_level"];
					hero11Skill2Param.magic_damage = (int)list3[k]["magic_damage"];
					hero11Skill2Param.explode_range = (int)list3[k]["explode_range"];
					hero11Skill2Param.minion_lifetime = (int)list3[k]["minion_lifetime"];
					hero11Skill2Param.minion_quantity = (int)list3[k]["minion_quantity"];
					hero11Skill2Param.cooldown_time = (int)list3[k]["cooldown_time"];
					heroSkillParameter_11_3.AddParamToList(hero11Skill2Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_11_3);
				}
			}
			catch (Exception)
			{
				ShowError(text3);
				throw;
			}
			num2 = 3;
			string text4 = $"Parameters/HeroSkills/hero_skill_{num}_{num2}";
			try
			{
				List<Dictionary<string, object>> list4 = CSVReader.Read(text4);
				HeroSkillParameter_11_3 heroSkillParameter_11_4 = new HeroSkillParameter_11_3();
				for (int l = 0; l < list4.Count; l++)
				{
					Hero11Skill3Param hero11Skill3Param = new Hero11Skill3Param();
					hero11Skill3Param.skill_level = (int)list4[l]["skill_level"];
					hero11Skill3Param.total_heal = (int)list4[l]["total_heal"];
					hero11Skill3Param.heal_range = (int)list4[l]["heal_range"];
					hero11Skill3Param.heal_duration = (int)list4[l]["heal_duration"];
					hero11Skill3Param.cooldown_time = (int)list4[l]["cooldown_time"];
					heroSkillParameter_11_4.AddParamToList(hero11Skill3Param);
					HeroSkillParameter.Instance.SetHeroSkillParameter(num, num2, heroSkillParameter_11_4);
				}
			}
			catch (Exception)
			{
				ShowError(text4);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv không tồn tại hoặc dữ liệu trong file không đúng định dạng.");
		}
	}
}
