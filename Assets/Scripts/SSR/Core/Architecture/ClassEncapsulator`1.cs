using UnityEngine;

namespace SSR.Core.Architecture
{
	public abstract class ClassEncapsulator<T> : MonoBehaviour
	{
		private bool firstReference = true;

		public T EncapsulatedClass
		{
			get
			{
				if (firstReference)
				{
					OnFirstReference();
					firstReference = false;
				}
				return GetEncapsulatedClass();
			}
		}

		protected abstract T GetEncapsulatedClass();

		protected virtual void OnFirstReference()
		{
		}
	}
}
