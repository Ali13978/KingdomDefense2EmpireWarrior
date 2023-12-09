using System.Collections.Generic;

namespace SSR.Core.Architecture.Helper
{
	public class IdsService : IIdsService
	{
		private List<int> freeIds = new List<int>();

		private List<int> ids = new List<int>();

		private int currentMaxId = -1;

		void IIdsService.FreeId(int Id)
		{
			ids.Remove(Id);
			freeIds.Add(Id);
		}

		int IIdsService.GetNewId()
		{
			if (freeIds.Count > 0)
			{
				int num = freeIds[freeIds.Count - 1];
				freeIds.RemoveAt(freeIds.Count - 1);
				ids.Add(num);
				return num;
			}
			currentMaxId++;
			ids.Add(currentMaxId);
			return currentMaxId;
		}

		void IIdsService.Initialize(int[] initialIds)
		{
			freeIds = new List<int>();
			ids = new List<int>();
			foreach (int num in initialIds)
			{
				ids.Add(num);
				if (num > currentMaxId)
				{
					currentMaxId = num;
				}
			}
		}
	}
}
