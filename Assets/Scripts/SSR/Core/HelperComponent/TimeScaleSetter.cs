using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class TimeScaleSetter : MonoBehaviour
	{
		public void Set(float value)
		{
			Time.timeScale = value;
		}
	}
}
