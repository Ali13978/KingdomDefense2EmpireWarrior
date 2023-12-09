using UnityEngine;

namespace SSR.Core.Architecture
{
	public abstract class MonoBehaviourSingleton<T> : MonoBehaviour, IFirstWakeComponent where T : class
	{
		private static T instance;

		private static MonoBehaviourSingleton<T> currentInstance;

		private static bool instantiated;

		private bool awoke;

		public static T Instance
		{
			get
			{
				if (!instantiated)
				{
					return (T)null;
				}
				return instance;
			}
		}

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

		protected abstract T GetInstance();

		public virtual void FirstAwake()
		{
			if (instantiated)
			{
				SSRLog.LogWarning("A MonoBehaviourSingleton is instantiated while current instance is still alive.");
			}
			instance = GetInstance();
			instantiated = true;
			currentInstance = this;
		}

		public virtual void OnDestroy()
		{
			if (currentInstance == this)
			{
				instance = (T)null;
				instantiated = false;
			}
		}
	}
}
