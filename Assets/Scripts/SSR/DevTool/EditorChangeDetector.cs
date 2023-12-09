using System.Linq;
using UnityEngine;

namespace SSR.DevTool
{
	public abstract class EditorChangeDetector : MonoBehaviour, IEditorChangeDetector
	{
		private bool changed;

		protected bool Changed
		{
			get
			{
				return changed;
			}
			set
			{
				changed = value;
			}
		}

		private event ChangeDetectedEventHandler ChangeDetected;

		public void AddListenerWithDulicationCheck(ChangeDetectedEventHandler listener)
		{
			if (this.ChangeDetected == null || !this.ChangeDetected.GetInvocationList().Contains(listener))
			{
				ChangeDetected += listener;
			}
		}

		public void Reset()
		{
			Changed = false;
			OnReset();
		}

		protected void DetectedChanged()
		{
			if (!Changed)
			{
				Changed = true;
				if (this.ChangeDetected != null)
				{
					this.ChangeDetected(this);
				}
			}
		}

		public abstract void OnReset();
	}
}
