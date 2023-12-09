using System;
using System.Collections.Generic;
using UnityEngine;

namespace SSR.Core.Architecture
{
	[Serializable]
	public abstract class CumulativeBoostContainter<T> : IBoostContainter<T>
	{
		[SerializeField]
		private List<IBoostComponent<T>> componentsList = new List<IBoostComponent<T>>();

		public abstract T BoostedValue
		{
			get;
		}

		protected List<IBoostComponent<T>> ComponentsList => componentsList;

		public void AddBoostComponent(IBoostComponent<T> boostComponent)
		{
			ComponentsList.Add(boostComponent);
		}

		public void RemoveBoostComponent(IBoostComponent<T> boostComponent)
		{
			ComponentsList.Remove(boostComponent);
		}

		public void ClearBoostComponents()
		{
			ComponentsList.Clear();
		}
	}
}
