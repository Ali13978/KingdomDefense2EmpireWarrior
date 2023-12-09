using Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class SetCustomWaveTest : MonoBehaviour
{
	public InputField waveInput;

	public void OnSetWaveBtnClicked()
	{
		int num = int.Parse(waveInput.text);
		GameplayManager.Instance.SetCustomWaveForTest(num);
		UnityEngine.Debug.Log("___Next wave will be wave " + num);
	}
}
