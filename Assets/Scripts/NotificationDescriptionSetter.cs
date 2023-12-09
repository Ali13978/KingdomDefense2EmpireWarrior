using Parameter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationDescriptionSetter : MonoBehaviour
{
	[SerializeField]
	private int notiID;

	[SerializeField]
	private Text notiDescription;

	[SerializeField]
	private TextMesh textMesh;

	public TextMeshProUGUI titleTMPro;

	[Space]
	[Header("Attribute")]
	[SerializeField]
	private bool setOnUpdate;

	private void Start()
	{
		SetDescription();
	}

	private void Update()
	{
		if (setOnUpdate)
		{
			SetDescription();
		}
	}

	public void SetDescription()
	{
		if ((bool)notiDescription)
		{
			notiDescription.text = Singleton<NotificationDescription>.Instance.GetNotiContent(notiID).Replace('@', '\n').Replace('#', '-');
		}
		if ((bool)textMesh)
		{
			textMesh.text = Singleton<NotificationDescription>.Instance.GetNotiContent(notiID).Replace('@', '\n').Replace('#', '-');
		}
		else if (titleTMPro != null)
		{
			titleTMPro.text = Singleton<NotificationDescription>.Instance.GetNotiContent(notiID).Replace('@', '\n').Replace('#', '-');
		}
	}
}
