using System.Collections.Generic;
using UnityEngine;

namespace SSR.Core.Architecture
{
	public class OneTimeFixedUpdateService : MonoBehaviourSingleton<IOneTimeFixedUpdateService>, IOneTimeFixedUpdateService
	{
		private int lastFrameCount;

		private List<IOneTimeFixedUpdateObject> fixedUpdateObjects = new List<IOneTimeFixedUpdateObject>();

		public void FixedUpdate()
		{
			if (Time.frameCount == lastFrameCount)
			{
				return;
			}
			for (int num = fixedUpdateObjects.Count - 1; num >= 0; num--)
			{
				IOneTimeFixedUpdateObject oneTimeFixedUpdateObject = fixedUpdateObjects[num];
				if (oneTimeFixedUpdateObject == null)
				{
					fixedUpdateObjects.RemoveAt(num);
				}
				else if (oneTimeFixedUpdateObject.Active)
				{
					oneTimeFixedUpdateObject.OneTimeFixedUpdate();
				}
			}
			lastFrameCount = Time.frameCount;
		}

		void IOneTimeFixedUpdateService.AddFixedUpdateObject(IOneTimeFixedUpdateObject fixedUpdateObject)
		{
			fixedUpdateObjects.Add(fixedUpdateObject);
		}

		void IOneTimeFixedUpdateService.RemoveFixedUpdateObject(IOneTimeFixedUpdateObject fixedUpdateObject)
		{
			fixedUpdateObjects.Remove(fixedUpdateObject);
		}

		protected override IOneTimeFixedUpdateService GetInstance()
		{
			return this;
		}
	}
}
