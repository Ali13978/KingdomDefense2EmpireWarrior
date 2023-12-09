using Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class GetMoreSpeed : MonoBehaviour
{
	[SerializeField]
	private Text speedText;

	private void Update()
	{
		speedText.text = "X " + GameplayManager.Instance.gameSpeedController.GameSpeed.ToString();
	}

	public void OnClick()
	{
		GameplayManager.Instance.gameSpeedController.GameSpeed += 2f;
	}
}
