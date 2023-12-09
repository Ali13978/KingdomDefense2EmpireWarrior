using UnityEngine;

public class TowerRangeController : MonoBehaviour
{
	[HideInInspector]
	public Transform target;

	[SerializeField]
	private float size;

	private float originSize;

	private void Awake()
	{
		Vector3 vector = GetComponent<SpriteRenderer>().sprite.bounds.size;
		originSize = vector.x;
	}

	public void Update()
	{
		if (null != target)
		{
			base.transform.position = target.position;
		}
	}

	public void SetRangeAttackMax(float size)
	{
		this.size = size;
		base.gameObject.SetActive(value: true);
		float num = size * 2.25f / originSize;
		base.transform.localScale = new Vector3(num, num, num);
	}

	public void HideRange()
	{
		base.gameObject.SetActive(value: false);
		target = null;
		base.transform.position = new Vector3(-100f, 100f, 0f);
	}
}
