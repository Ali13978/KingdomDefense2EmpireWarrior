using System;
using System.Collections.Generic;

namespace DigitalRuby.ThunderAndLightning
{
	[Serializable]
	public class ReorderableList<T> : ReorderableListBase
	{
		public List<T> List;
	}
}
