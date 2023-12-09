using UnityEngine;
using UnityEngine.Events;

public class ToggleMemberController : MonoBehaviour
{
	[SerializeField]
	private UnityEvent selected;

	[SerializeField]
	private UnityEvent deselected;

	public bool devChecked;

	public bool isSlected = true;

	[SerializeField]
	private bool defaultSelected;

	private void Start()
	{
		isSlected = true;
		if (defaultSelected)
		{
			Selected();
		}
	}

	public void OnDrawGizmos()
	{
		devChecked = CheckDev();
	}

	private bool CheckDev()
	{
		if (base.transform.parent.GetComponent<ToggleGroupController>() != null)
		{
			return true;
		}
		UnityEngine.Debug.LogError("Need add component to parent: ToggleGroupController ");
		return false;
	}

	[ContextMenu("Selected")]
	public void Selected()
	{
		selected.Invoke();
		isSlected = true;
		UnSelectedOtherToggleMember();
	}

	[ContextMenu("Deselected")]
	public void Deselected()
	{
		isSlected = false;
		deselected.Invoke();
	}

	private void UnSelectedOtherToggleMember()
	{
		base.transform.parent.GetComponent<ToggleGroupController>().DeSelectedMember(this);
	}
}
