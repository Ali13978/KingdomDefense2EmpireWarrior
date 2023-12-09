using UnityEngine;

public class TestClickObject : MonoBehaviour
{
	private void Update()
	{
		if (!Input.GetKeyDown(KeyCode.Mouse0))
		{
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
		UnityEngine.Debug.DrawRay(UnityEngine.Input.mousePosition, Vector3.forward, Color.green);
		if (Physics.Raycast(ray, out RaycastHit hitInfo, float.PositiveInfinity))
		{
			if (hitInfo.collider.tag == "BuildRegion")
			{
				UnityEngine.Debug.Log("1");
			}
		}
		else
		{
			UnityEngine.Debug.Log("Click wtf!");
		}
	}
}
