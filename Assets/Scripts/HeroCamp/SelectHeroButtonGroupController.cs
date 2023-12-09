using Data;
using System.Collections.Generic;
using UnityEngine;

namespace HeroCamp
{
	public class SelectHeroButtonGroupController : MonoBehaviour
	{
		public List<SelectHeroButtonController> listSelectHeroButton = new List<SelectHeroButtonController>();

		private void Start()
		{
			ReadWriteDataHero.Instance.OnSkillPointChangeEvent += Instance_OnSkillPointChangeEvent;
			ReadWriteDataHero.Instance.OnHeroLevelChangeEvent += Instance_OnHeroLevelChangeEvent;
			UpdateNotify();
		}

		private void OnDestroy()
		{
			ReadWriteDataHero.Instance.OnSkillPointChangeEvent -= Instance_OnSkillPointChangeEvent;
			ReadWriteDataHero.Instance.OnHeroLevelChangeEvent -= Instance_OnHeroLevelChangeEvent;
		}

		private void Instance_OnSkillPointChangeEvent()
		{
			UpdateNotify();
		}

		private void Instance_OnHeroLevelChangeEvent()
		{
			UpdateNotify();
		}

		public void AutoChoseHero(int heroID)
		{
			foreach (SelectHeroButtonController item in listSelectHeroButton)
			{
				if (item.HeroID == heroID)
				{
					item.OnClick();
				}
			}
		}

		public void Init()
		{
			foreach (SelectHeroButtonController item in listSelectHeroButton)
			{
				item.UpdateHeroLevel();
			}
		}

		private void UpdateNotify()
		{
			foreach (SelectHeroButtonController item in listSelectHeroButton)
			{
				item.UpdateNotifyHeroSkill();
			}
		}
	}
}
