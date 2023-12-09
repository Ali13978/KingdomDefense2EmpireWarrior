using Data;
using Notify;
using System.Collections.Generic;
using UnityEngine;

namespace WorldMap
{
	public class WorldMapNotificationManager : SingletonMonoBehaviour<WorldMapNotificationManager>
	{
		[SerializeField]
		private List<NotifyUnit> listNotifyUnit = new List<NotifyUnit>();

		private void Start()
		{
			RefreshAllNotifyStatus();
			ReadWriteDataPlayerCurrency.Instance.OnGemChangeEvent += Instance_OnGemChangeEvent;
			ReadWriteDataGlobalUpgrade.Instance.OnStarChangeEvent += Instance_OnStarChangeEvent;
			ReadWriteDataHero.Instance.OnSkillPointChangeEvent += Instance_OnSkillPointChangeEvent;
		}

		private void OnDestroy()
		{
			ReadWriteDataPlayerCurrency.Instance.OnGemChangeEvent -= Instance_OnGemChangeEvent;
			ReadWriteDataGlobalUpgrade.Instance.OnStarChangeEvent -= Instance_OnStarChangeEvent;
			ReadWriteDataHero.Instance.OnSkillPointChangeEvent -= Instance_OnSkillPointChangeEvent;
		}

		private void Instance_OnStarChangeEvent()
		{
			CustomInvoke(RefreshAllNotifyStatus, Time.deltaTime);
		}

		private void Instance_OnGemChangeEvent()
		{
			CustomInvoke(RefreshAllNotifyStatus, Time.deltaTime);
		}

		private void Instance_OnSkillPointChangeEvent()
		{
			CustomInvoke(RefreshAllNotifyStatus, Time.deltaTime);
		}

		public void RefreshAllNotifyStatus()
		{
			foreach (NotifyUnit item in listNotifyUnit)
			{
				item.CheckCondition();
			}
		}
	}
}
