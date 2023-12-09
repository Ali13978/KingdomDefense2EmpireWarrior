using UnityEngine;

namespace SSR.Core.Architecture
{
	public class FirstAwakeService : MonoBehaviour
	{
		private void Awake()
		{
			Transform[] array = UnityEngine.Object.FindObjectsOfType<Transform>();
			Transform[] array2 = array;
			foreach (Transform transform in array2)
			{
				if (transform.gameObject.scene != base.gameObject.scene)
				{
					continue;
				}
				IFirstWakeComponent[] components = transform.GetComponents<IFirstWakeComponent>();
				IFirstWakeComponent[] array3 = components;
				foreach (IFirstWakeComponent firstWakeComponent in array3)
				{
					if (!firstWakeComponent.Awoke)
					{
						firstWakeComponent.FirstAwake();
						firstWakeComponent.Awoke = true;
					}
				}
			}
		}
	}
}
