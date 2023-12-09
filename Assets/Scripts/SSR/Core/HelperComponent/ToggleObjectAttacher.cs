using UnityEngine;
using UnityEngine.UI;

namespace SSR.Core.HelperComponent
{
	[RequireComponent(typeof(Toggle))]
	[ExecuteInEditMode]
	public class ToggleObjectAttacher : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		private Toggle toggle;

		[SerializeField]
		private GameObject attachedObject;

		public void Awake()
		{
			if (toggle == null)
			{
				toggle = GetComponent<Toggle>();
			}
			SetAttachedObjectActive(toggle.isOn);
			toggle.onValueChanged.AddListener(delegate(bool isOn)
			{
				SetAttachedObjectActive(isOn);
			});
		}

		private void SetAttachedObjectActive(bool active)
		{
			if (attachedObject != null)
			{
				attachedObject.SetActive(active);
			}
		}

		private void Reset()
		{
			toggle = GetComponent<Toggle>();
		}
	}
}
