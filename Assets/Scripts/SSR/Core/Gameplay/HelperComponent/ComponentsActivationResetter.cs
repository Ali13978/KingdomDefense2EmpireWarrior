using SSR.Core.Architecture.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class ComponentsActivationResetter : MonoBehaviour, IResetableObject
	{
		[SerializeField]
		private Behaviour componentToAdd;

		[SerializeField]
		private List<Behaviour> components = new List<Behaviour>();

		private List<bool> state = new List<bool>();

		public void ResetToLastSavedState()
		{
			for (int i = 0; i < state.Count; i++)
			{
				components[i].enabled = state[i];
			}
		}

		public void SaveCurrentState()
		{
			state = new List<bool>();
			for (int i = 0; i < components.Count; i++)
			{
				try
				{
					state.Add(components[i].enabled);
				}
				catch (Exception exception)
				{
					UnityEngine.Debug.LogException(exception, this);
					throw;
				}
			}
		}

		[ContextMenu("Get All Components")]
		private void GetAllComponents()
		{
			Behaviour[] componentsInChildren = GetComponentsInChildren<Behaviour>(includeInactive: true);
			components = new List<Behaviour>(componentsInChildren);
			components.Remove(this);
		}

		[ContextMenu("Get All Components From Children Only")]
		private void GetAllComponentsFromChildrenOnly()
		{
			components = new List<Behaviour>();
			for (int i = 0; i < base.transform.childCount; i++)
			{
				components.AddRange(base.transform.GetChild(i).GetComponentsInChildren<Behaviour>(includeInactive: true));
			}
		}

		private void OnDrawGizmos()
		{
			if (componentToAdd != null)
			{
				AddComponentToList(componentToAdd);
				componentToAdd = null;
			}
		}

		private void AddComponentToList(Behaviour component)
		{
			if (!components.Contains(component))
			{
				components.Add(component);
			}
		}

		[ContextMenu("Remove null elements")]
		private void RemoveNoneElements()
		{
			for (int num = components.Count - 1; num >= 0; num--)
			{
				if (components[num] == null)
				{
					components.RemoveAt(num);
				}
			}
		}
	}
}
