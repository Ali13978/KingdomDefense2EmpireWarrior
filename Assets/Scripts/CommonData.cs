using Data;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CommonData : MonoBehaviour
{
	public PetConfig petConfig;

	public TdLocalization tdLocalizationConfig;

	public TournamentConstantConfig tournamentConstantConfig;

	public TournamentPrizeConfig tournamentPrizeConfig;

	public ItemConfig itemConfig;

	public DailyRewardConfig dailyRewardConfig;

	public SaleBundleConfig saleBundleConfig;

	public EventConfig eventConfig;

	public GameObject IgnoreDefPrefab;

	public MultiLanguaguageDataReader multiLanguaguageDataReader;

	[HideInInspector]
	public Dictionary<int, FormationConfigData> formationData = new Dictionary<int, FormationConfigData>();

	public Dictionary<EventQuestType, List<EventConfigData>> eventTypeToEventData = new Dictionary<EventQuestType, List<EventConfigData>>();

	public Dictionary<int, EventConfigData> eventIdToEventData = new Dictionary<int, EventConfigData>();

	public static CommonData Instance
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
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		InitEventQuestData();
		ReadFormationData();
	}

	private void Start()
	{
		ReadWriteData.Instance.WriteDefaultSetting();
	}

	private void InitEventQuestData()
	{
		int num = eventConfig.dataArray.Length;
		for (int i = 0; i < num; i++)
		{
			if (eventConfig.dataArray[i].Durationinday > 0)
			{
				EventQuestType eVENTQUESTTYPE = eventConfig.dataArray[i].EVENTQUESTTYPE;
				if (!eventTypeToEventData.ContainsKey(eVENTQUESTTYPE))
				{
					eventTypeToEventData.Add(eVENTQUESTTYPE, new List<EventConfigData>());
				}
				eventTypeToEventData[eVENTQUESTTYPE].Add(eventConfig.dataArray[i]);
				eventIdToEventData[eventConfig.dataArray[i].Eventid] = eventConfig.dataArray[i];
			}
		}
	}

	private void ReadFormationData()
	{
		string text = "Parameters/MapCampaign/enemy_formation";
		List<Dictionary<string, object>> list = null;
		try
		{
			list = CSVReader.Read(text);
			for (int i = 0; i < list.Count; i++)
			{
				int key = (int)list[i]["formation_id"];
				int num = (int)list[i]["time"];
				if (!formationData.ContainsKey(key))
				{
					formationData.Add(key, new FormationConfigData());
				}
				formationData[key].AddTime((float)num * 0.001f);
			}
		}
		catch (Exception)
		{
			UnityEngine.Debug.LogError("File " + text + ".csv không tồn tại hoặc dữ liệu trong file không đúng định dạng.");
			throw;
		}
	}
}
