using UnityEngine;

namespace SSR.Core
{
	public class InspectorCommandAttribute : PropertyAttribute
	{
		public int IntPara
		{
			get;
			set;
		}

		public string StringPara
		{
			get;
			set;
		}
	}
}
