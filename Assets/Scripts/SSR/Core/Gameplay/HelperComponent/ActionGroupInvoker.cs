using System.Collections.Generic;
using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class ActionGroupInvoker : MonoBehaviour
	{
		[SerializeField]
		private List<ActionGroup> actionGroups = new List<ActionGroup>();

		public void InvokeActionGroupByName(string groupName)
		{
			foreach (ActionGroup actionGroup in actionGroups)
			{
				if (actionGroup.Name == groupName)
				{
					actionGroup.Actions.Invoke();
					break;
				}
			}
		}

		public void InvokeActionGroupById(int groupId)
		{
			actionGroups[groupId].Actions.Invoke();
		}
	}
}
