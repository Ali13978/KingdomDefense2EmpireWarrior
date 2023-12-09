using UnityEngine;

namespace SSR.Core.Architecture.UI
{
	public abstract class ContentSetter : MonoBehaviour, IUIRefresher
	{
		[Header("ContentSetter")]
		[SerializeField]
		protected bool setContentAtUpdate;

		[SerializeField]
		private bool setContentAtAwake = true;

		[SerializeField]
		private bool setContentAtEnable;

		private bool initialized;

		[ContextMenu("Refresh")]
		public void Refresh()
		{
			if (!initialized)
			{
				Initialize();
			}
			SetContent();
		}

		public void Awake()
		{
			Initialize();
			if (setContentAtAwake)
			{
				SetContent();
			}
		}

		public void OnEnable()
		{
			if (setContentAtEnable)
			{
				SetContent();
			}
		}

		public void Update()
		{
			if (setContentAtUpdate)
			{
				SetContent();
			}
		}

		private void Initialize()
		{
			OnInitialize();
			initialized = true;
		}

		protected abstract void SetContent();

		protected virtual void OnInitialize()
		{
		}
	}
}
