using UnityEngine;

public class StarGroupController : MonoBehaviour
{
	[SerializeField]
	private GameObject[] listStarGroup;

	public void DisplayStarGroup(int starAmount)
	{
		HideAllStars();
		if (starAmount < 1)
		{
			return;
		}
		for (int i = 0; i < listStarGroup.Length; i++)
		{
			if (i <= starAmount - 1)
			{
				listStarGroup[i].SetActive(value: true);
			}
		}
	}

	private void HideAllStars()
	{
		GameObject[] array = listStarGroup;
		foreach (GameObject gameObject in array)
		{
			gameObject.gameObject.SetActive(value: false);
		}
	}
}
