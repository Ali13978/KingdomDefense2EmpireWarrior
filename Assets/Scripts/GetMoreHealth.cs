using Gameplay;
using UnityEngine;

public class GetMoreHealth : MonoBehaviour
{
	[SerializeField]
	private int healthAmount;

	public void OnClick()
	{
		GameplayManager.Instance.gameLogicController.IncreaseHealth(healthAmount);
	}
}
