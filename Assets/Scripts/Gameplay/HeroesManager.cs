using MyCustom;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class HeroesManager : CustomMonoBehaviour
	{
		private Dictionary<int, HeroModel> listHeroes = new Dictionary<int, HeroModel>();

		private int heroIDChoosing;

		private int lastHeroIDChoosed;

		private int heroSkillIDChoosing;

		public int HeroIDChoosing
		{
			get
			{
				return heroIDChoosing;
			}
			private set
			{
				heroIDChoosing = value;
			}
		}

		public int HeroSkillIDChoosing
		{
			get
			{
				return heroSkillIDChoosing;
			}
			private set
			{
				heroSkillIDChoosing = value;
			}
		}

		public static HeroesManager Instance
		{
			get;
			set;
		}

		public event Action<int> onChooseHero;

		public event Action<int> onUnChooseHero;

		public event Action<int, Vector2> onHeroMoveToAssignedPosition;

		public event Action<int> onChooseHeroSkill;

		public event Action<int> onUnChooseHeroSkill;

		public event Action<int, Vector2> onCastHeroSkillToAssignedPosition;

		private void Awake()
		{
			if ((bool)Instance)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			Instance = this;
			InitDefaultData();
		}

		private void InitDefaultData()
		{
			HeroIDChoosing = -1;
			HeroSkillIDChoosing = -1;
		}

		public void AddToList(int heroID, HeroModel heroModel)
		{
			if (!listHeroes.ContainsKey(heroID))
			{
				listHeroes.Add(heroID, heroModel);
			}
		}

		public HeroModel GetHero(int heroId)
		{
			if (!listHeroes.ContainsKey(heroId))
			{
				return null;
			}
			return listHeroes[heroId];
		}

		public void CalculateExp()
		{
			int totalExp = SingletonMonoBehaviour<GameData>.Instance.TotalExp;
			foreach (HeroModel value in listHeroes.Values)
			{
				int num = 0;
				float num2 = (float)UnityEngine.Random.Range(85, 115) / 100f;
				num = (int)((float)(totalExp / listHeroes.Count) * num2);
				value.AddExp(num);
			}
		}

		public void ChooseHero(int heroID)
		{
			HeroIDChoosing = heroID;
			lastHeroIDChoosed = HeroIDChoosing;
			if (this.onChooseHero != null)
			{
				this.onChooseHero(heroID);
			}
		}

		public void UnChooseHero(int heroID)
		{
			if (heroID != -1)
			{
				if (this.onUnChooseHero != null)
				{
					this.onUnChooseHero(heroID);
				}
				HeroIDChoosing = -1;
			}
		}

		public void MoveHeroToAssignedPosition(int heroID, Vector3 targetPosition)
		{
			listHeroes[heroID].GetFSMController().GetCurrentState().OnInput(StateInputType.UserAssignPosition, targetPosition);
			DispatchOnMoveHeroToAssignedPosition(heroID, targetPosition);
		}

		private void DispatchOnMoveHeroToAssignedPosition(int heroID, Vector3 targetPosition)
		{
			if (this.onHeroMoveToAssignedPosition != null)
			{
				this.onHeroMoveToAssignedPosition(heroID, targetPosition);
			}
		}

		public void ChooseHeroSkill(int heroID)
		{
			HeroSkillIDChoosing = heroID;
			if (this.onChooseHeroSkill != null)
			{
				this.onChooseHeroSkill(heroID);
			}
		}

		public void UnChooseHeroSkill(int heroID)
		{
			if (this.onUnChooseHeroSkill != null)
			{
				this.onUnChooseHeroSkill(heroID);
			}
			HeroSkillIDChoosing = -1;
		}

		public void CastHeroSkillToAssignedPosition(int heroID, Vector3 targetPosition)
		{
			if (this.onCastHeroSkillToAssignedPosition != null)
			{
				this.onCastHeroSkillToAssignedPosition(heroID, targetPosition);
			}
		}
	}
}
