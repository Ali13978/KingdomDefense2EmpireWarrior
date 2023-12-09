using UnityEngine;

public class SmartSortingLayer : MonoBehaviour
{
	[SerializeField]
	private bool setOneTime;

	[SerializeField]
	private bool setRealTime;

	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private bool higherIsCover;

	[SerializeField]
	private bool lowerIsCover;

	[SerializeField]
	private bool flyObject;

	[Space]
	[Header("Set layer order depend on other")]
	[SerializeField]
	private SpriteRenderer targetGameObject;

	[SerializeField]
	private bool isDependOnOther;

	[SerializeField]
	private int offset;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		if (setOneTime)
		{
			SetSortingOrder();
		}
		if (isDependOnOther && setOneTime)
		{
			SetSortingOrderDepend();
		}
	}

	private void OnEnable()
	{
		if (setOneTime)
		{
			SetSortingOrder();
		}
		if (isDependOnOther && setOneTime)
		{
			SetSortingOrderDepend();
		}
	}

	private void Update()
	{
		if ((bool)spriteRenderer)
		{
			if (setRealTime)
			{
				SetSortingOrder();
			}
			if (isDependOnOther && setRealTime)
			{
				SetSortingOrderDepend();
			}
		}
	}

	private void SetSortingOrder()
	{
		if (lowerIsCover)
		{
			SpriteRenderer obj = spriteRenderer;
			Vector3 position = base.transform.position;
			obj.sortingOrder = -Mathf.RoundToInt(position.y * 100f);
		}
		if (higherIsCover)
		{
			SpriteRenderer obj2 = spriteRenderer;
			Vector3 position2 = base.transform.position;
			obj2.sortingOrder = Mathf.RoundToInt(position2.y * 100f);
		}
		if (flyObject)
		{
			SetSortingOrder(40);
		}
	}

	private void SetSortingOrderDepend()
	{
		spriteRenderer.sortingOrder = targetGameObject.sortingOrder + offset;
	}

	private void SetSortingOrder(int value)
	{
		spriteRenderer.sortingOrder = value;
	}
}
