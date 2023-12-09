using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
	public float lifeTime;

	private void Start()
	{
		UnityEngine.Object.Destroy(base.gameObject, lifeTime);
	}
}
