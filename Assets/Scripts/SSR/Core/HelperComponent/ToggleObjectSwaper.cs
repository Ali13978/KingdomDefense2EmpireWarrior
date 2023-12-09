using System;
using UnityEngine;
using UnityEngine.UI;

namespace SSR.Core.HelperComponent
{
	[RequireComponent(typeof(Toggle))]
	[ExecuteInEditMode]
	public class ToggleObjectSwaper : MonoBehaviour
	{
		[Serializable]
		private class SwapObject
		{
			[SerializeField]
			public GameObject mainObject;

			[SerializeField]
			public Graphic targetGraphic;

			public void SetActive(bool active)
			{
				if (mainObject != null)
				{
					mainObject.SetActive(active);
				}
			}
		}

		[SerializeField]
		private SwapObject onObject = new SwapObject();

		[SerializeField]
		private SwapObject offObject = new SwapObject();

		[SerializeField]
		[HideInInspector]
		private Toggle toggle;

		private void Update()
		{
			if (toggle.isOn)
			{
				onObject.SetActive(active: true);
				offObject.SetActive(active: false);
				toggle.targetGraphic = onObject.targetGraphic;
			}
			else
			{
				onObject.SetActive(active: false);
				offObject.SetActive(active: true);
				toggle.targetGraphic = offObject.targetGraphic;
			}
		}

		private void Reset()
		{
			toggle = GetComponent<Toggle>();
		}
	}
}
