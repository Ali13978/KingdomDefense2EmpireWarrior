using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.EndGame.Reward
{
	public class UnlockCountGroupController : MonoBehaviour
	{
		[SerializeField]
		private List<Image> listCountObject = new List<Image>();

		private void Awake()
		{
			SingletonMonoBehaviour<GameData>.Instance.OnOpenChestTurnChange += Instance_OnOpenChestTurnChange;
		}

		private void Instance_OnOpenChestTurnChange()
		{
			UpdateListCountObject();
		}

		public void UpdateListCountObject()
		{
			int currentOpenChestTurn = SingletonMonoBehaviour<GameData>.Instance.CurrentOpenChestTurn;
			for (int i = 0; i < listCountObject.Count; i++)
			{
				if (i <= currentOpenChestTurn - 1)
				{
					listCountObject[i].color = Color.white;
				}
				else
				{
					listCountObject[i].color = Color.gray;
				}
			}
		}
	}
}
