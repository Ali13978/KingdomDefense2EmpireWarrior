using UnityEngine;
using UnityEngine.UI;

public class UITestToggle : MonoBehaviour
{
	private Toggle toggle;

	public GameObject panelTest;

	private void Awake()
	{
		toggle = GetComponent<Toggle>();
	}

	public void OnValueChanged()
	{
		if (toggle.isOn)
		{
			panelTest.SetActive(value: true);
		}
		else
		{
			panelTest.SetActive(value: false);
		}
	}
}
