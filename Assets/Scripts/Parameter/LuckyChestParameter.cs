using System.Collections.Generic;

namespace Parameter
{
	public class LuckyChestParameter
	{
		public List<List<LuckyChest>> listParam = new List<List<LuckyChest>>();

		private static LuckyChestParameter instance;

		public static LuckyChestParameter Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new LuckyChestParameter();
				}
				return instance;
			}
		}

		public void SetParameter(LuckyChest chest)
		{
			int count = listParam.Count;
			if (count <= chest.id)
			{
				List<LuckyChest> list = new List<LuckyChest>();
				list.Insert(chest.turn, chest);
				listParam.Insert(chest.id, list);
			}
			else
			{
				List<LuckyChest> list2 = listParam[chest.id];
				list2.Insert(chest.turn, chest);
			}
		}

		public LuckyChest GetChestParameter(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				return listParam[id][level];
			}
			return default(LuckyChest);
		}

		private bool CheckParameter(int id, int level)
		{
			if (id >= GetNumberOfItem() || level > GetNumberOfLevel())
			{
				return false;
			}
			return true;
		}

		public int GetNumberOfItem()
		{
			return listParam.Count;
		}

		public int GetNumberOfLevel()
		{
			if (GetNumberOfItem() > 0)
			{
				return listParam[0].Count;
			}
			return 0;
		}

		public List<int> GetListItemsPreview()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < listParam.Count; i++)
			{
				LuckyChest chestParameter = GetChestParameter(i, 0);
				if (chestParameter.isPreview == 1 && !list.Contains(i))
				{
					list.Add(i);
				}
			}
			return list;
		}

		public int GetChestRate(int id, int turn)
		{
			if (id >= 0 && id < listParam.Count)
			{
				LuckyChest luckyChest = listParam[id][turn];
				return luckyChest.rate;
			}
			return -1;
		}

		public int GetChestValue(int id, int turn)
		{
			if (id >= 0 && id < listParam.Count)
			{
				LuckyChest luckyChest = listParam[id][turn];
				return luckyChest.value;
			}
			return -1;
		}
	}
}
