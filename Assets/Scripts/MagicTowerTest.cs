using UnityEngine;

public class MagicTowerTest : MonoBehaviour
{
	public TargetScript target;

	public GameObject prefabBullet;

	public int throwForce;

	public float duration;

	public Transform firePos;

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
		{
			CreateBullet();
		}
	}

	private void CreateBullet()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(prefabBullet);
		gameObject.transform.position = firePos.position;
		gameObject.GetComponent<BulletMagic>().Init(target);
		WildMover component = gameObject.GetComponent<WildMover>();
		QuadraticPath component2 = gameObject.GetComponent<QuadraticPath>();
		component.StartPos = firePos.transform.position;
		component.FinishPos = target.preMovePos;
		Vector3 vector = target.transform.position - base.gameObject.transform.position;
		component2.direction = 1f;
		component2.controlMagnitude = throwForce;
		component.Duration = duration;
		component.StartMoving();
		GameObject gameObject2 = UnityEngine.Object.Instantiate(prefabBullet);
		gameObject2.transform.position = firePos.position;
		gameObject2.GetComponent<BulletMagic>().Init(target);
		WildMover component3 = gameObject2.GetComponent<WildMover>();
		QuadraticPath component4 = gameObject2.GetComponent<QuadraticPath>();
		component3.StartPos = firePos.transform.position;
		component3.FinishPos = target.preMovePos;
		Vector3 vector2 = target.transform.position - base.gameObject.transform.position;
		component4.direction = -1f;
		component4.controlMagnitude = throwForce;
		component3.Duration = duration;
		component3.StartMoving();
	}
}
