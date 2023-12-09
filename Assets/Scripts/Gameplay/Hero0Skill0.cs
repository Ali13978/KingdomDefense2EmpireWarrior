using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero0Skill0 : HeroSkillCommon
	{
		private int heroID;

		private int skillID;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private Hero heroParameter;

		private int numberClone;

		private float parameter_scale;

		private float duration;

		private float cooldownTime;

		private string description;

		private string useType;

		private RaycastHit2D hit;

		[SerializeField]
		private LayerMask avaiableMovingLayerMask;

		private Vector2 vectorXUnit = new Vector2(0.3f, 0f);

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			heroParameter = HeroParameter.Instance.GetHeroParameter(heroID, currentLevel);
			HeroSkillParameter_0_0 heroSkillParameter_0_ = new HeroSkillParameter_0_0();
			heroSkillParameter_0_ = (HeroSkillParameter_0_0)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			numberClone = heroSkillParameter_0_.getParam(currentSkillLevel - 1).number_clone;
			parameter_scale = (float)heroSkillParameter_0_.getParam(currentSkillLevel - 1).parameter_Scale / 100f;
			duration = (float)heroSkillParameter_0_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_0_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_0_.getParam(currentSkillLevel - 1).description;
			useType = heroSkillParameter_0_.getParam(currentSkillLevel - 1).use_type;
			SingletonMonoBehaviour<SpawnAlly>.Instance.PushAlliesToPool(100, 100, 0);
		}

		public override float GetCooldownTime()
		{
			return cooldownTime;
		}

		public override string GetUseType()
		{
			return useType;
		}

		private void Start()
		{
			HeroesManager.Instance.onCastHeroSkillToAssignedPosition += Instance_onCastHeroSkillToAssignedPosition;
		}

		private void OnDestroy()
		{
			HeroesManager.Instance.onCastHeroSkillToAssignedPosition -= Instance_onCastHeroSkillToAssignedPosition;
		}

		private void Instance_onCastHeroSkillToAssignedPosition(int heroID, Vector2 targetPosition)
		{
			if (this.heroID == heroID)
			{
				CreateClones(targetPosition);
			}
		}

		private void CreateClones(Vector2 targetPosition)
		{
			UnityEngine.Debug.Log("Hero 0 Cast Skill 0");
			SingletonMonoBehaviour<GameData>.Instance.PlayerKnowHowToUseSkill = true;
			int num = 0;
			for (int i = 0; i < numberClone; i++)
			{
				AllyModel allyModel = SingletonMonoBehaviour<SpawnAlly>.Instance.Get(100, 100);
				Vector3 one = Vector3.one;
				if (num == 0)
				{
					allyModel.transform.position = targetPosition - vectorXUnit;
				}
				else
				{
					allyModel.transform.position = targetPosition + vectorXUnit;
				}
				allyModel.InitFromHero(heroParameter, parameter_scale, duration);
				allyModel.gameObject.SetActive(value: true);
				num++;
			}
			SingletonMonoBehaviour<GameplayUIHeroManager>.Instance.listSelectHeroSkillButton[0].DoCooldown();
		}
	}
}
