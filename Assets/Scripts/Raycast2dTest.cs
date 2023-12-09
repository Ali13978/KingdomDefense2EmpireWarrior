using UnityEngine;

public class Raycast2dTest : MonoBehaviour
{
	public LayerMask layerMask;

	private void Start()
	{
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
		{
			RaycastHit2D raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Vector3.back, 5f, layerMask);
			UnityEngine.Debug.DrawRay(base.transform.position, Vector3.back, Color.red, 1.5f);
			if (raycastHit2D.collider != null)
			{
				UnityEngine.Debug.Log(raycastHit2D.collider.name);
				Vector3 worldPositionOnPlane = GetWorldPositionOnPlane(UnityEngine.Input.mousePosition, 0f);
				UnityEngine.Debug.Log(worldPositionOnPlane);
				UnityEngine.Debug.Log(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition));
			}
		}
	}

	private Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
	{
		Ray ray = Camera.main.ScreenPointToRay(screenPosition);
		new Plane(Vector3.forward, new Vector3(0f, 0f, z)).Raycast(ray, out float enter);
		return ray.GetPoint(enter);
	}
}
