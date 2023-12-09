using System;
using UnityEngine;
using UnityEngine.Events;

namespace SSR.Core.Gameplay.HelperComponent
{
	[Serializable]
	public class ActionGroup
	{
		[SerializeField]
		private string name;

		[SerializeField]
		private UnityEvent actions = new UnityEvent();

		public string Name => name;

		public UnityEvent Actions => actions;
	}
}
