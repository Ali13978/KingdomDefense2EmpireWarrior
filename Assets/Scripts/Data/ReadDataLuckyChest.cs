using Gameplay;
using MyCustom;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class ReadDataLuckyChest : CustomMonoBehaviour
	{
		[SerializeField]
		private FreeChestOffer freeChestOffer;

		private void Start()
		{
			ReadParameter(SingletonMonoBehaviour<GameData>.Instance.MapID);
		}

		public void ReadParameter(int mapID)
		{
			string text = "Parameters/LuckyChest/lucky_chest_param_map_" + mapID;
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					LuckyChest parameter = default(LuckyChest);
					parameter.id = (int)list[i]["id"];
					parameter.name = (string)list[i]["name"];
					parameter.turn = (int)list[i]["turn"];
					parameter.rate = (int)list[i]["rate_scale"];
					parameter.value = (int)list[i]["value"];
					parameter.isPreview = (int)list[i]["preview"];
					LuckyChestParameter.Instance.SetParameter(parameter);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv không tồn tại hoặc dữ liệu trong file không đúng định dạng.");
		}

		public int GetTurnAmountToBuyEach()
		{
			return freeChestOffer.param.turnAmountToBuyEach;
		}

		public int GetGemAmountToBuyTurn()
		{
			int num = 2 - SingletonMonoBehaviour<GameData>.Instance.CurrentOpenChestOffer;
			return freeChestOffer.param.gemAmountToBuyTurn[num];
		}
	}
}
