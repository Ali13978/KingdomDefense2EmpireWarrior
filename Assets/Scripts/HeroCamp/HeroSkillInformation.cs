using Data;
using MyCustom;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class HeroSkillInformation : MonoBehaviour
	{
		[SerializeField]
		private Text skillName;

		[SerializeField]
		private Text skillType;

		[SerializeField]
		private Text skillDescription;

		public void Init(int heroID, int skillID)
		{
			InitUltimateInformation(heroID, skillID);
		}

		private void InitUltimateInformation(int heroID, int skillID)
		{
			skillName.text = Singleton<HeroDescription>.Instance.GetHeroSkillName(heroID, skillID).Replace('@', '\n').Replace('#', '-');
			skillType.text = Singleton<HeroDescription>.Instance.GetHeroSkillType(heroID, skillID);
			int numberOfMainParam = HeroSkillParameter.Instance.GetNumberOfMainParam(heroID, skillID);
			int skillPoint = ReadWriteDataHero.Instance.GetSkillPoint(heroID, skillID);
			switch (numberOfMainParam)
			{
			case 0:
				skillDescription.text = Singleton<HeroDescription>.Instance.GetHeroSkillDescription(heroID, skillID);
				break;
			case 1:
			{
				string highLightTextByLevel3 = StaticMethod.GetHighLightTextByLevel(HeroSkillParameter.Instance.GetMainParams0(heroID, skillID)[0].ToString(), HeroSkillParameter.Instance.GetMainParams0(heroID, skillID)[1].ToString(), HeroSkillParameter.Instance.GetMainParams0(heroID, skillID)[2].ToString(), skillPoint);
				skillDescription.text = string.Format(Singleton<HeroDescription>.Instance.GetHeroSkillDescription(heroID, skillID), highLightTextByLevel3);
				break;
			}
			case 2:
			{
				string highLightTextByLevel = StaticMethod.GetHighLightTextByLevel(HeroSkillParameter.Instance.GetMainParams0(heroID, skillID)[0].ToString(), HeroSkillParameter.Instance.GetMainParams0(heroID, skillID)[1].ToString(), HeroSkillParameter.Instance.GetMainParams0(heroID, skillID)[2].ToString(), skillPoint);
				string highLightTextByLevel2 = StaticMethod.GetHighLightTextByLevel(HeroSkillParameter.Instance.GetMainParams1(heroID, skillID)[0].ToString(), HeroSkillParameter.Instance.GetMainParams1(heroID, skillID)[1].ToString(), HeroSkillParameter.Instance.GetMainParams1(heroID, skillID)[2].ToString(), skillPoint);
				skillDescription.text = string.Format(Singleton<HeroDescription>.Instance.GetHeroSkillDescription(heroID, skillID), highLightTextByLevel, highLightTextByLevel2);
				break;
			}
			}
		}
	}
}
