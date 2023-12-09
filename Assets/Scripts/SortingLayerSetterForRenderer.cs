using UnityEngine;

public class SortingLayerSetterForRenderer : MonoBehaviour
{
	public string layer_name;

	[ContextMenu("Set Sorting Layer")]
	public void SetSortingLayer()
	{
		GetComponent<Renderer>().sortingLayerName = layer_name;
	}
}
