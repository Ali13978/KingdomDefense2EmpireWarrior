using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizeComponent : MonoBehaviour
{
	public Text localText;

	public string localizeKey;

	[Space]
	[Header("Attribute")]
	[SerializeField]
	private bool setOnUpdate;

	private void Start()
	{
		SetLocalizeContent();
	}

	private void Update()
	{
		if (setOnUpdate)
		{
			SetLocalizeContent();
		}
	}

	private void SetLocalizeContent()
	{
		localText.text = GameTools.GetLocalization(localizeKey);
	}
}
