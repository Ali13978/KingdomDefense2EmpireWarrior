using UnityEngine;

public class SortingLayerSetterForParticle : MonoBehaviour
{
	public string layer_name;

	[ContextMenu("Set Sorting Layer")]
	public void SetSortingLayer()
	{
		GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = layer_name;
	}
}
