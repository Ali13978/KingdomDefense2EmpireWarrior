using Parameter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BalanceUIManager : MonoBehaviour
{
	public InputField mapInput;

	public InputField lineInput;

	public InputField startGateInput;

	public InputField monsterIdInput;

	public InputField lineDelayInput;

	public InputField minDiffInput;

	public Text lineDataText;

	public Text lineRangeText;

	public Toggle showOtherEnemyToggle;

	public BalanceMonsterDemo lineEnemySample;

	public InputField formationInput;

	public BalanceMonsterDemo formEnemySample;

	public Text formationInfoText;

	public BalanceAddFormationPopup addFormationToMapPopup;

	private float secondToUnit = 0.3f;

	private float randYRange = 0.25f;

	private int curMapId = -1;

	private int curLineIndex;

	private List<EnemyData> mapData = new List<EnemyData>();

	private float[] durationEachWave = new float[30];

	private float[] delayBeforeThisWave = new float[30];

	private List<BalanceMonsterDemo> lineEnemies = new List<BalanceMonsterDemo>();

	private List<BalanceMonsterDemo> otherlineEnemies = new List<BalanceMonsterDemo>();

	private int numOfWave;

	private string originalMoney;

	private string originalHealth;

	private string originalDeltaTime;

	private int curFormationId;

	private Dictionary<int, FormationConfigData> formationData = new Dictionary<int, FormationConfigData>();

	private FormationConfigData curFormConfig;

	private List<BalanceMonsterDemo> formEnemies = new List<BalanceMonsterDemo>();

	private void Start()
	{
		InitFormationWindow();
		InitMapWindow();
	}

	public void ReadWaveData(int mapId)
	{
		if (curMapId != mapId)
		{
			curMapId = mapId;
			string text = "Parameters/MapCampaign/map_" + mapId;
			mapData.Clear();
			List<Dictionary<string, object>> list = null;
			try
			{
				list = CSVReader.Read(text);
				originalMoney = ((int)list[0]["money"]).ToString();
				originalHealth = ((int)list[0]["health"]).ToString();
				originalDeltaTime = ((int)list[0]["delta_time"]).ToString();
				for (int i = 0; i < list.Count; i++)
				{
					int wave = (int)list[i]["wave"];
					int time = (int)list[i]["time"];
					int id = (int)list[i]["enemy_id"];
					int gate = (int)list[i]["gate"];
					int formationId = (int)list[i]["formation_id"];
					int minDifficulty = (int)list[i]["min_difficulty"];
					mapData.Add(new EnemyData(wave, time, id, isLastInWave: false, gate, formationId, minDifficulty));
				}
			}
			catch (Exception)
			{
				UnityEngine.Debug.LogError("File " + text + ".csv không tồn tại hoặc dữ liệu trong file không đúng định dạng.");
				throw;
			}
		}
	}

	public void ReadFormationData()
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

	public void WriteMapConfigToCSV()
	{
		string filePath = Application.dataPath + "/Resources/Parameters/MapCampaign/map_" + curMapId + ".csv";
		List<string[]> list = new List<string[]>();
		list.Add(new string[11]
		{
			"wave",
			"no",
			"time",
			"is_last_in_wave",
			"enemy_id",
			"gate",
			"formation_id",
			"min_difficulty",
			"money",
			"health",
			"delta_time"
		});
		for (int i = 0; i < mapData.Count; i++)
		{
			string[] array = new string[11];
			string[] array2 = array;
			EnemyData enemyData = mapData[i];
			array2[0] = enemyData.wave.ToString();
			string[] array3 = array;
			EnemyData enemyData2 = mapData[i];
			array3[2] = enemyData2.time.ToString();
			string[] array4 = array;
			EnemyData enemyData3 = mapData[i];
			array4[4] = enemyData3.id.ToString();
			string[] array5 = array;
			EnemyData enemyData4 = mapData[i];
			array5[5] = enemyData4.gate.ToString();
			string[] array6 = array;
			EnemyData enemyData5 = mapData[i];
			array6[6] = enemyData5.formationId.ToString();
			string[] array7 = array;
			EnemyData enemyData6 = mapData[i];
			array7[7] = enemyData6.minDifficulty.ToString();
			list.Add(array);
		}
		list[1][8] = originalMoney;
		list[1][9] = originalHealth;
		list[1][10] = originalDeltaTime;
		WriteToCSV(list, filePath);
	}

	public void WriteFormationConfigToCSV()
	{
		string filePath = Application.dataPath + "/Resources/Parameters/MapCampaign/enemy_formation.csv";
		List<string[]> list = new List<string[]>();
		list.Add(new string[2]
		{
			"formation_id",
			"time"
		});
		for (int i = 1; i <= formationData.Count; i++)
		{
			for (int j = 0; j < formationData[i].times.Count; j++)
			{
				list.Add(new string[2]
				{
					i.ToString(),
					Mathf.RoundToInt(formationData[i].times[j] * 1000f).ToString()
				});
			}
		}
		WriteToCSV(list, filePath);
	}

	private void WriteToCSV(List<string[]> rowData, string filePath)
	{
		string[][] array = new string[rowData.Count][];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = rowData[i];
		}
		int length = array.GetLength(0);
		string separator = ",";
		StringBuilder stringBuilder = new StringBuilder();
		for (int j = 0; j < length; j++)
		{
			stringBuilder.AppendLine(string.Join(separator, array[j]));
		}
		StreamWriter streamWriter = File.CreateText(filePath);
		streamWriter.WriteLine(stringBuilder);
		streamWriter.Close();
	}

	private void InitFormationWindow()
	{
		formationInput.text = "1";
		ReadFormationData();
		SetFormation(1);
	}

	private void SetFormation(int pformationId)
	{
		if (pformationId > 0 && pformationId <= formationData.Count)
		{
			curFormationId = pformationId;
			curFormConfig = formationData[curFormationId];
			int count = curFormConfig.times.Count;
			float num = curFormConfig.times[count - 1];
			formationInfoText.text = $"FORMATION {curFormationId} (contains {count} enemies) - spawn within {num}s";
			ShowFormation(formEnemySample, curFormConfig, formEnemies);
		}
	}

	public void OnLoadFormationBtnClicked()
	{
		int formation = int.Parse(formationInput.text);
		SetFormation(formation);
	}

	public void OnCreateNewFormationClicked()
	{
		int num = formationData.Count + 1;
		formationData.Add(num, new FormationConfigData());
		formationData[num].AddTime(0f);
		SetFormation(num);
	}

	public void OnAddNewMonsterToCurFormation()
	{
		int count = curFormConfig.times.Count;
		curFormConfig.times.Add(curFormConfig.times[count - 1] + 1f);
		formEnemySample.gameObject.SetActive(value: true);
		formEnemies.Add(ObjectPool.Spawn(formEnemySample, formEnemySample.transform.parent));
		formEnemySample.gameObject.SetActive(value: false);
		int index = formEnemies.Count - 1;
		formEnemies[index].transform.position = formEnemySample.transform.position + new Vector3(curFormConfig.times[index] * secondToUnit, UnityEngine.Random.Range(0f - randYRange, randYRange), 0f);
		formationInfoText.text = $"FORMATION {curFormationId} (contains {count + 1} enemies) - spawn within {curFormConfig.times[index]}s";
	}

	public void OnAddFormationToCurMap()
	{
		addFormationToMapPopup.gameObject.SetActive(value: true);
		BalanceAddFormationPopup balanceAddFormationPopup = addFormationToMapPopup;
		int formation = curFormationId;
		List<EnemyData> list = mapData;
		EnemyData enemyData = mapData[curLineIndex];
		balanceAddFormationPopup.Init(formation, list, enemyData.wave, numOfWave, delegate
		{
			OnFinishAddFormationToMap();
			ShowOtherEnemiesCloseToCurLine();
		});
	}

	public void OnSaveFormationModification()
	{
		int count = curFormConfig.times.Count;
		for (int i = 0; i < count; i++)
		{
			Vector3 position = formEnemies[i].transform.position;
			float x = position.x;
			Vector3 position2 = formEnemySample.transform.position;
			float num = (x - position2.x) / secondToUnit;
			if (num < 0f)
			{
				num = 0f;
			}
			curFormConfig.times[i] = num;
		}
		curFormConfig.times.Sort();
		ShowOtherEnemiesCloseToCurLine();
	}

	public void OnSaveAsNewFormationBtnClicked()
	{
		List<float> list = new List<float>();
		int count = curFormConfig.times.Count;
		for (int i = 0; i < count; i++)
		{
			List<float> list2 = list;
			Vector3 position = formEnemies[i].transform.position;
			float x = position.x;
			Vector3 position2 = formEnemySample.transform.position;
			list2.Add((x - position2.x) / secondToUnit);
		}
		int key = formationData.Count + 1;
		formationData.Add(key, new FormationConfigData(list));
		curFormationId = key;
		curFormConfig = formationData[key];
		formationInfoText.text = $"FORMATION {curFormationId} (contains {curFormConfig.times.Count} enemies) - spawn within {curFormConfig.GetDuration()}s";
	}

	public void OnPrevFormBtnClicked()
	{
		int num = curFormationId - 1;
		if (num <= 0)
		{
			num = formationData.Count;
		}
		SetFormation(num);
	}

	public void OnNextFormBtnClicked()
	{
		int num = curFormationId + 1;
		if (num > formationData.Count)
		{
			num = 1;
		}
		SetFormation(num);
	}

	public void InitMapWindow()
	{
		mapInput.text = "0";
		lineInput.text = "0";
		SetMap(0);
		SetLineAndLineInfoText(0);
		ShowCurLineEnemies();
	}

	private void SetMap(int pMapId)
	{
		if (curMapId != pMapId)
		{
			ReadWaveData(pMapId);
			curMapId = pMapId;
			lineRangeText.text = $"(0..{mapData.Count - 1})";
			OnFinishAddFormationToMap();
		}
	}

	private void SetLineAndLineInfoText(int lineIndex)
	{
		curLineIndex = lineIndex;
		if (lineIndex < 0 || lineIndex >= mapData.Count)
		{
			lineDataText.text = $"INVALID LINE INDEX: line {lineIndex} is out of range (0..{mapData.Count - 1})";
			return;
		}
		EnemyData curLineData = mapData[curLineIndex];
		FormationConfigData formationConfigData = null;
		if (curLineData.formationId > 0)
		{
			formationConfigData = formationData[curLineData.formationId];
		}
		startGateInput.text = curLineData.gate.ToString();
		monsterIdInput.text = curLineData.id.ToString();
		lineDelayInput.text = ((float)curLineData.time * 0.001f).ToString();
		minDiffInput.text = curLineData.minDifficulty.ToString();
		int numOfEnemyInWave = 0;
		int firstEnemyIndex = 0;
		int lastEnemyIndex = 0;
		float timeToThisWave = 0f;
		GetLineStatic(curLineData, out numOfEnemyInWave, out firstEnemyIndex, out lastEnemyIndex, out timeToThisWave);
		lineDataText.text = string.Format("Showing map {0} - wave {1} (contains: {2} enemies) - line {3} (contains {4} enemies ({5} to {6})) - {7}\nThis line spawn within {8}s, starts after the wave started for {9}s, after the game started for {10}s", curMapId, curLineData.wave, numOfEnemyInWave, curLineIndex, lastEnemyIndex - firstEnemyIndex + 1, firstEnemyIndex, lastEnemyIndex, (curLineData.formationId <= 0) ? string.Empty : ("Formation " + curLineData.formationId), (curLineData.formationId <= 0) ? 0f : formationConfigData.GetDuration(), lineDelayInput.text, timeToThisWave + (float)curLineData.time * 0.001f);
	}

	private void GetLineStatic(EnemyData curLineData, out int numOfEnemyInWave, out int firstEnemyIndex, out int lastEnemyIndex, out float timeToThisWave)
	{
		int num = 0;
		numOfEnemyInWave = 0;
		for (int i = 0; i < mapData.Count; i++)
		{
			EnemyData enemyData = mapData[i];
			int num2;
			if (enemyData.formationId == 0)
			{
				num2 = 1;
			}
			else
			{
				Dictionary<int, FormationConfigData> dictionary = formationData;
				EnemyData enemyData2 = mapData[i];
				num2 = dictionary[enemyData2.formationId].times.Count;
			}
			int num3 = num2;
			EnemyData enemyData3 = mapData[i];
			if (enemyData3.wave == curLineData.wave)
			{
				numOfEnemyInWave += num3;
			}
			EnemyData enemyData4 = mapData[i];
			if (enemyData4.wave >= curLineData.wave)
			{
				EnemyData enemyData5 = mapData[i];
				if (enemyData5.wave != curLineData.wave)
				{
					continue;
				}
				EnemyData enemyData6 = mapData[i];
				if (enemyData6.time >= curLineData.time)
				{
					continue;
				}
			}
			num += num3;
		}
		firstEnemyIndex = num + 1;
		lastEnemyIndex = num + ((curLineData.formationId == 0) ? 1 : formationData[curLineData.formationId].times.Count);
		timeToThisWave = delayBeforeThisWave[curLineData.wave] * 0.001f;
		timeToThisWave += 15 * curLineData.wave;
	}

	private void ShowCurLineEnemies()
	{
		if (curLineIndex < 0 || curLineIndex >= mapData.Count)
		{
			return;
		}
		EnemyData enemyData = mapData[curLineIndex];
		FormationConfigData curformation = null;
		if (enemyData.formationId > 0)
		{
			curformation = formationData[enemyData.formationId];
		}
		if (enemyData.formationId == 0)
		{
			lineEnemySample.gameObject.SetActive(value: true);
			lineEnemySample.Init(enemyData.id);
			for (int i = 0; i < lineEnemies.Count; i++)
			{
				lineEnemies[i].Recycle();
			}
			lineEnemies.Clear();
		}
		else
		{
			ShowFormation(lineEnemySample, curformation, lineEnemies);
			for (int j = 0; j < lineEnemies.Count; j++)
			{
				lineEnemies[j].Init(enemyData.id);
			}
		}
		ShowOtherEnemiesCloseToCurLine();
	}

	private void ShowOtherEnemiesCloseToCurLine()
	{
		if (curLineIndex < 0 || curLineIndex >= mapData.Count)
		{
			return;
		}
		for (int i = 0; i < otherlineEnemies.Count; i++)
		{
			otherlineEnemies[i].Recycle();
		}
		otherlineEnemies.Clear();
		if (!showOtherEnemyToggle.isOn)
		{
			return;
		}
		EnemyData enemyData = mapData[curLineIndex];
		float num = enemyData.time;
		float num2 = num + 40000f;
		lineEnemySample.gameObject.SetActive(value: true);
		for (int j = 0; j < mapData.Count; j++)
		{
			EnemyData enemyData2 = mapData[j];
			if (enemyData2.wave != enemyData.wave)
			{
				continue;
			}
			EnemyData enemyData3 = mapData[j];
			if (enemyData3.gate != enemyData.gate || j == curLineIndex)
			{
				continue;
			}
			EnemyData enemyData4 = mapData[j];
			if (enemyData4.formationId == 0)
			{
				EnemyData enemyData5 = mapData[j];
				if ((float)enemyData5.time >= num)
				{
					EnemyData enemyData6 = mapData[j];
					if ((float)enemyData6.time < num2)
					{
						EnemyData enemyData7 = mapData[j];
						int id = enemyData7.id;
						int lineIndex = j;
						EnemyData enemyData8 = mapData[j];
						ShowOtherEnemyAtTime(id, lineIndex, (float)enemyData8.time - num);
					}
				}
				continue;
			}
			EnemyData enemyData9 = mapData[j];
			int id2 = enemyData9.id;
			int lineIndex2 = j;
			Dictionary<int, FormationConfigData> dictionary = formationData;
			EnemyData enemyData10 = mapData[j];
			FormationConfigData formationConfigData = dictionary[enemyData10.formationId];
			for (int k = 0; k < formationConfigData.times.Count; k++)
			{
				EnemyData enemyData11 = mapData[j];
				int num3 = enemyData11.time + Mathf.RoundToInt(formationConfigData.times[k] * 1000f);
				if ((float)num3 >= num && (float)num3 < num2)
				{
					ShowOtherEnemyAtTime(id2, lineIndex2, (float)num3 - num);
				}
			}
		}
		if (enemyData.formationId != 0)
		{
			lineEnemySample.gameObject.SetActive(value: false);
		}
	}

	private void ShowOtherEnemyAtTime(int monsterId, int lineIndex, float timeInMiliInCompareToCurLine)
	{
		BalanceMonsterDemo balanceMonsterDemo = ObjectPool.Spawn(lineEnemySample, lineEnemySample.transform.parent);
		otherlineEnemies.Add(balanceMonsterDemo);
		balanceMonsterDemo.transform.position = lineEnemySample.transform.position + new Vector3(timeInMiliInCompareToCurLine * 0.001f * secondToUnit, UnityEngine.Random.Range(0f - randYRange, randYRange) * 1.3f, 0f);
		balanceMonsterDemo.Init(monsterId, new Color(1f, 1f, 1f, 0.4f));
		balanceMonsterDemo.line = lineIndex;
	}

	public void OnLoadMapLineBtnClicked()
	{
		int map = int.Parse(mapInput.text);
		int lineAndLineInfoText = int.Parse(lineInput.text);
		SetMap(map);
		SetLineAndLineInfoText(lineAndLineInfoText);
		ShowCurLineEnemies();
	}

	public void OnModifyLineData_SetBtnClicked()
	{
		EnemyData value = mapData[curLineIndex];
		value.gate = int.Parse(startGateInput.text);
		value.id = int.Parse(monsterIdInput.text);
		value.time = Mathf.RoundToInt(float.Parse(lineDelayInput.text) * 1000f);
		value.minDifficulty = int.Parse(minDiffInput.text);
		mapData[curLineIndex] = value;
		SetLineAndLineInfoText(curLineIndex);
		ShowCurLineEnemies();
	}

	public void OnShowOtherEnemyBtnClicked()
	{
		ShowOtherEnemiesCloseToCurLine();
	}

	public void OnDeleteLine()
	{
		mapData.RemoveAt(curLineIndex);
		if (curLineIndex > 0)
		{
			curLineIndex--;
		}
		SetLineAndLineInfoText(curLineIndex);
		ShowCurLineEnemies();
	}

	public void OnPrevLineBtnClicked()
	{
		if (curLineIndex > 0)
		{
			curLineIndex--;
			SetLineAndLineInfoText(curLineIndex);
			ShowCurLineEnemies();
			lineInput.text = curLineIndex.ToString();
		}
	}

	public void OnNextLineBtnClicked()
	{
		if (curLineIndex < mapData.Count - 1)
		{
			curLineIndex++;
			SetLineAndLineInfoText(curLineIndex);
			ShowCurLineEnemies();
			lineInput.text = curLineIndex.ToString();
		}
	}

	public void OnFinishAddFormationToMap()
	{
		numOfWave = 0;
		HashSet<int> hashSet = new HashSet<int>();
		for (int i = 0; i < durationEachWave.Length; i++)
		{
			durationEachWave[i] = 0f;
		}
		for (int j = 0; j < mapData.Count; j++)
		{
			HashSet<int> hashSet2 = hashSet;
			EnemyData enemyData = mapData[j];
			if (!hashSet2.Contains(enemyData.wave))
			{
				HashSet<int> hashSet3 = hashSet;
				EnemyData enemyData2 = mapData[j];
				hashSet3.Add(enemyData2.wave);
				numOfWave++;
			}
			EnemyData enemyData3 = mapData[j];
			float num = enemyData3.time;
			EnemyData enemyData4 = mapData[j];
			float num2;
			if (enemyData4.formationId > 0)
			{
				Dictionary<int, FormationConfigData> dictionary = formationData;
				EnemyData enemyData5 = mapData[j];
				num2 = dictionary[enemyData5.formationId].GetDuration();
			}
			else
			{
				num2 = 0f;
			}
			float num3 = num + num2;
			float num4 = num3;
			float[] array = durationEachWave;
			EnemyData enemyData6 = mapData[j];
			if (num4 > array[enemyData6.wave])
			{
				float[] array2 = durationEachWave;
				EnemyData enemyData7 = mapData[j];
				array2[enemyData7.wave] = num3;
			}
		}
		delayBeforeThisWave[0] = 0f;
		for (int k = 1; k < delayBeforeThisWave.Length; k++)
		{
			delayBeforeThisWave[k] = delayBeforeThisWave[k - 1] + durationEachWave[k - 1];
		}
	}

	public void ShowFormation(BalanceMonsterDemo sample, FormationConfigData curformation, List<BalanceMonsterDemo> listEnemy)
	{
		sample.gameObject.SetActive(value: true);
		int count = curformation.times.Count;
		for (int i = listEnemy.Count; i < count; i++)
		{
			BalanceMonsterDemo item = ObjectPool.Spawn(sample, sample.transform.parent);
			listEnemy.Add(item);
		}
		for (int num = listEnemy.Count - 1; num >= count; num--)
		{
			listEnemy[num].Recycle();
			listEnemy.RemoveAt(num);
		}
		sample.gameObject.SetActive(value: false);
		for (int j = 0; j < count; j++)
		{
			listEnemy[j].transform.position = sample.transform.position + new Vector3(curformation.times[j] * secondToUnit, UnityEngine.Random.Range(0f - randYRange, randYRange), 0f);
		}
	}

	public void OnPushBtnClicked()
	{
		try
		{
			WriteFormationConfigToCSV();
			WriteMapConfigToCSV();
			UnityEngine.Debug.Log("SAVED!!!!!");
		}
		catch (Exception)
		{
			UnityEngine.Debug.LogError("Close all excel/csv file before saving");
		}
	}
}
