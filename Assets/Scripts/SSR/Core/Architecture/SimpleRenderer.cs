using UnityEngine;

namespace SSR.Core.Architecture
{
	public class SimpleRenderer : MonoBehaviour, ISimpleRenderer
	{
		[Header("SimpleRenderer")]
		private bool selfVisible = true;

		private bool initialized;

		protected bool Initialized => initialized;

		public bool SelfVisible
		{
			get
			{
				return selfVisible;
			}
			set
			{
				selfVisible = value;
				base.gameObject.SetActive(selfVisible);
			}
		}

		public bool Visible => base.gameObject.activeInHierarchy;

		public bool IsInitialized => initialized;

		public void Initialize()
		{
			if (!initialized)
			{
				OnInitialize();
				initialized = true;
			}
		}

		protected virtual void OnInitialize()
		{
		}
	}
}
