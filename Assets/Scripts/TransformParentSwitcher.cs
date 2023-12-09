using System.Collections;
using UnityEngine;

public class TransformParentSwitcher : MonoBehaviour
{
	[SerializeField]
	private Transform temporaryParent;

	private Transform originalParent;

	private int orginalSiblingIndex;

	private void Awake()
	{
	}

	[ContextMenu("SwitchToTemporayParent")]
	public void SwitchToTemporayParent()
	{
		if (!(base.transform.parent == temporaryParent))
		{
			originalParent = base.transform.parent;
			orginalSiblingIndex = base.transform.GetSiblingIndex();
			base.transform.SetParent(temporaryParent);
		}
	}

	public void SwitchToTemporayParent(int delayTimeMilisecond)
	{
		StartCoroutine(iSwitchToTemporayParent((float)delayTimeMilisecond / 1000f));
	}

	private IEnumerator iSwitchToTemporayParent(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		if (base.transform.parent == temporaryParent)
		{
		}
		originalParent = base.transform.parent;
		orginalSiblingIndex = base.transform.GetSiblingIndex();
		base.transform.SetParent(temporaryParent);
	}

	[ContextMenu("SwitchBackToOriginalParent")]
	public void SwitchBackToOriginalParent()
	{
		if ((bool)originalParent)
		{
			base.transform.SetParent(originalParent);
			base.transform.SetSiblingIndex(orginalSiblingIndex);
			base.transform.localPosition = Vector3.zero;
		}
	}
}
