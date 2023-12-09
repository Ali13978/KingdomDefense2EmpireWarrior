using System;
using UnityEngine;

namespace Gameplay
{
	public static class BuffStackLogicHelper
	{
		public static float GetStackedValue(BuffStackLogic stackLogic, float oldValue, float newValue)
		{
			switch (stackLogic)
			{
			case BuffStackLogic.StackUp:
				return oldValue + newValue;
			case BuffStackLogic.ChooseMin:
				return Mathf.Min(oldValue, newValue);
			case BuffStackLogic.ChooseMax:
				return Mathf.Max(oldValue, newValue);
			case BuffStackLogic.ChooseNew:
				return newValue;
			default:
				throw new NotSupportedException();
			}
		}
	}
}
