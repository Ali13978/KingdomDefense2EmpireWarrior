using UnityEngine;

public class RangeCircleController : MonoBehaviour
{
	[SerializeField]
	private float size;

	private float originSize;

	public float Size
	{
		get
		{
			return size;
		}
		set
		{
			size = value;
			SetSize();
		}
	}

	private void Awake()
	{
		Vector3 vector = GetComponent<SpriteRenderer>().sprite.bounds.size;
		originSize = vector.x;
	}

	public void OnDrawGizmosSelected()
	{
	}

	private void SetSize()
	{
		float num = size * 2f / originSize;
		base.transform.localScale = new Vector3(num, num, num);
	}
}
