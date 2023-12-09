using Data;
using MyCustom;
using UnityEngine;

namespace ApplicationEntry
{
	public class ApplicationEntry : MonoBehaviour
	{
		[SerializeField]
		private ReadWriteRemoteSettingData readWriteRemoteSettingData;

		public ReadWriteRemoteSettingData ReadWriteRemoteSettingData
		{
			get
			{
				return readWriteRemoteSettingData;
			}
			set
			{
				readWriteRemoteSettingData = value;
			}
		}

		public static ApplicationEntry Instance
		{
			get;
			set;
		}

		private void Awake()
		{
			if ((bool)Instance)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			Instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			Application.targetFrameRate = 60;
		}

		private void Start()
		{
			if (StaticMethod.CheckIfDayPassed())
			{
				UnityEngine.Debug.Log("Ngày mới trôi quaaa!");
				ReadWriteDataFreeResources.Instance.ResetFreeResourcesDailyData();
				ReadWriteRemoteSettingData.WriteDefaultRemoteSettingValue_Gem();
				ReadWriteDataDailyTrial.Instance.IncreaseDay();
				ReadWriteDataDailyReward.Instance.TryIncreaseDay();
			}
			ReadWriteDataSaleBundle.Instance.SetLastTimePlay();
		}
	}
}
