using UnityEngine;
using UnityEngine.UI;

public class ItemQuantityController : MonoBehaviour
{
	[SerializeField]
	private Text itemQuantity;

	private int itemAmount;

	public void Init(int itemAmount)
	{
		this.itemAmount = itemAmount;
		if (itemAmount > 0)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}

	private void Show()
	{
		base.gameObject.SetActive(value: true);
		itemQuantity.text = "+ " + itemAmount.ToString();
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}
}
