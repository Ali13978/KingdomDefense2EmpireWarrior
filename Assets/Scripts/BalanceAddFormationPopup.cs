using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalanceAddFormationPopup : MonoBehaviour
{
	public InputField enemyIdInput;

	public InputField delayInput;

	public InputField gateInput;

	public InputField minDiffInput;

	public GameObject addToCurBtn;

	public Text addnewWaveText;

	public Text addToCurWaveText;

	private int formation;

	private int curWaveIndex;

	private int newWaveIndex;

	private List<EnemyData> mapData;

	private Action callback;

	public void Init(int formation, List<EnemyData> mapData, int curWaveIndex, int newWaveIndex, Action callback)
	{
		this.formation = formation;
		this.mapData = mapData;
		this.curWaveIndex = curWaveIndex;
		this.newWaveIndex = newWaveIndex;
		this.callback = callback;
		enemyIdInput.text = "1";
		delayInput.text = "0";
		gateInput.text = "0";
		minDiffInput.text = "0";
		addToCurBtn.SetActive(newWaveIndex > curWaveIndex);
		addnewWaveText.text = $"Add as new wave (Wave {newWaveIndex})";
		addToCurWaveText.text = $"Add to current wave (Wave {curWaveIndex})";
	}

	public void OnCloseBtnClicked()
	{
		base.gameObject.SetActive(value: false);
	}

	public void OnAddNewWaveBtnClicked()
	{
		int id = int.Parse(enemyIdInput.text);
		int time = Mathf.RoundToInt(float.Parse(delayInput.text) * 1000f);
		int gate = int.Parse(gateInput.text);
		int minDifficulty = int.Parse(minDiffInput.text);
		mapData.Add(new EnemyData(newWaveIndex, time, id, isLastInWave: false, gate, formation, minDifficulty));
		callback();
		OnCloseBtnClicked();
	}

	public void OnAddToCurWaveBtnClicked()
	{
		int id = int.Parse(enemyIdInput.text);
		int time = Mathf.RoundToInt(float.Parse(delayInput.text) * 1000f);
		int gate = int.Parse(gateInput.text);
		int minDifficulty = int.Parse(minDiffInput.text);
		for (int num = mapData.Count - 1; num >= 0; num--)
		{
			EnemyData enemyData = mapData[num];
			if (enemyData.wave == curWaveIndex)
			{
				mapData.Insert(num + 1, new EnemyData(curWaveIndex, time, id, isLastInWave: false, gate, formation, minDifficulty));
				break;
			}
		}
		callback();
		OnCloseBtnClicked();
	}
}
