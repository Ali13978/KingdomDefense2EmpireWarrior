using Gameplay;
using UnityEngine;

public class GetMoreMoney : MonoBehaviour
{
	[SerializeField]
	private int goldAmount;

	public void OnClick()
	{
		SingletonMonoBehaviour<GameData>.Instance.IncreaseMoney(goldAmount);
	}
}
