using UnityEngine;

namespace SSR.Core.Architecture
{
	public abstract class CollabMember : MonoBehaviour, ICollabMember
	{
		[SerializeField]
		protected CollabHub collabHub;

		[SerializeField]
		private bool registerAtWake;

		public abstract bool IsFinished
		{
			get;
		}

		public virtual void OnStartWorking()
		{
		}

		public virtual void Awake()
		{
			ICollabHubRegister collabHubRegister = collabHub;
			if (registerAtWake && collabHubRegister != null && collabHubRegister.IsOpening)
			{
				((ICollabHubRegister)collabHub).Register((ICollabMember)this);
			}
		}
	}
}
