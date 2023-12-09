using UnityEngine;

public class SelectedImageController : MonoBehaviour
{
	private Transform targetTransform;

	public void Init(Transform targetTransform)
	{
		this.targetTransform = targetTransform;
	}

	private void Update()
	{
		if ((bool)targetTransform)
		{
			base.transform.position = targetTransform.position;
		}
	}
}
