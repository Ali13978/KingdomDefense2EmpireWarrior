using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class ReorderableListAttribute : PropertyAttribute
	{
		public string Tooltip
		{
			get;
			private set;
		}

		public ReorderableListAttribute(string tooltip)
		{
			Tooltip = tooltip;
		}
	}
}
