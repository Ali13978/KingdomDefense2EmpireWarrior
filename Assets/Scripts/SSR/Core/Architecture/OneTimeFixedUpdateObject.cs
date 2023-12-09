using UnityEngine;

namespace SSR.Core.Architecture
{
	public abstract class OneTimeFixedUpdateObject : MonoBehaviour, IOneTimeFixedUpdateObject
	{
		bool IOneTimeFixedUpdateObject.Active
		{
			get
			{
				if (this == null)
				{
					return false;
				}
				return base.isActiveAndEnabled;
			}
		}

		public abstract void OneTimeFixedUpdate();

		protected virtual void Awake()
		{
			MonoBehaviourSingleton<IOneTimeFixedUpdateService>.Instance.AddFixedUpdateObject(this);
		}
	}
}
