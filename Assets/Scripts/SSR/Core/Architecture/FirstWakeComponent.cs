using UnityEngine;

namespace SSR.Core.Architecture
{
	public abstract class FirstWakeComponent : MonoBehaviour, IFirstWakeComponent
	{
		private bool awoke;

		public bool Awoke
		{
			get
			{
				return awoke;
			}
			set
			{
				awoke = value;
			}
		}

		public abstract void FirstAwake();
	}
}
