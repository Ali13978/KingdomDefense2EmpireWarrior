using UnityEngine;

public class F3DTurret : MonoBehaviour
{
	public Transform hub;

	public Transform barrel;

	private RaycastHit hitInfo;

	private bool isFiring;

	private float hubAngle;

	private float barrelAngle;

	private Vector3 ProjectVectorOnPlane(Vector3 planeNormal, Vector3 vector)
	{
		return vector - Vector3.Dot(vector, planeNormal) * planeNormal;
	}

	private float SignedVectorAngle(Vector3 referenceVector, Vector3 otherVector, Vector3 normal)
	{
		Vector3 lhs = Vector3.Cross(normal, referenceVector);
		float num = Vector3.Angle(referenceVector, otherVector);
		return num * Mathf.Sign(Vector3.Dot(lhs, otherVector));
	}

	private void Track()
	{
		if (!(hub != null) || !(barrel != null))
		{
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
		if (Physics.Raycast(ray, out hitInfo, 500f))
		{
			Vector3 vector = ProjectVectorOnPlane(hub.up, hitInfo.point - hub.position);
			Quaternion b = Quaternion.LookRotation(vector);
			hubAngle = SignedVectorAngle(base.transform.forward, vector, Vector3.up);
			if (hubAngle <= -60f)
			{
				b = Quaternion.LookRotation(Quaternion.Euler(0f, -60f, 0f) * base.transform.forward);
			}
			else if (hubAngle >= 60f)
			{
				b = Quaternion.LookRotation(Quaternion.Euler(0f, 60f, 0f) * base.transform.forward);
			}
			hub.rotation = Quaternion.Slerp(hub.rotation, b, Time.deltaTime * 5f);
			Vector3 vector2 = ProjectVectorOnPlane(hub.right, hitInfo.point - barrel.position);
			Quaternion b2 = Quaternion.LookRotation(vector2);
			barrelAngle = SignedVectorAngle(hub.forward, vector2, hub.right);
			if (barrelAngle < -30f)
			{
				b2 = Quaternion.LookRotation(Quaternion.AngleAxis(-30f, hub.right) * hub.forward);
			}
			else if (barrelAngle > 15f)
			{
				b2 = Quaternion.LookRotation(Quaternion.AngleAxis(15f, hub.right) * hub.forward);
			}
			barrel.rotation = Quaternion.Slerp(barrel.rotation, b2, Time.deltaTime * 5f);
		}
	}

	private void Update()
	{
		Track();
		if (!isFiring && UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
		{
			isFiring = true;
			F3DFXController.instance.Fire();
		}
		if (isFiring && UnityEngine.Input.GetKeyUp(KeyCode.Mouse0))
		{
			isFiring = false;
			F3DFXController.instance.Stop();
		}
	}
}
