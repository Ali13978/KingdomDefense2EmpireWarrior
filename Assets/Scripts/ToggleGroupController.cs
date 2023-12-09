using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGroupController : MonoBehaviour
{
	public List<ToggleMemberController> listMember = new List<ToggleMemberController>();

	public void DeSelectedMember(ToggleMemberController member)
	{
		foreach (ToggleMemberController item in listMember)
		{
			if (item != member && item.isSlected)
			{
				item.Deselected();
			}
		}
	}

	public void SelectedMember(int index)
	{
		listMember[index].Selected();
	}

	[ContextMenu("AddMember")]
	public void AddMember()
	{
		listMember.Clear();
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				ToggleMemberController component = transform.GetComponent<ToggleMemberController>();
				if ((bool)component)
				{
					listMember.Add(component);
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}
}
