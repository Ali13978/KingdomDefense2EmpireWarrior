using System.Collections.Generic;
using UnityEngine;

namespace SSR.Core.Architecture
{
	public class PermutationList
	{
		private int numberOfElements;

		private List<int> indexes;

		public PermutationList(int numberOfElements)
		{
			this.numberOfElements = numberOfElements;
			Reset();
		}

		public void Reset()
		{
			indexes = new List<int>();
			for (int i = 0; i < numberOfElements; i++)
			{
				indexes.Add(i);
			}
		}

		public int GetNextIndex()
		{
			if (indexes.Count == 0)
			{
				Reset();
			}
			int index = Random.Range(0, indexes.Count);
			int result = indexes[index];
			indexes.RemoveAt(index);
			return result;
		}
	}
}
