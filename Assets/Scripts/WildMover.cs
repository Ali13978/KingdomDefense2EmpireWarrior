using SSR.Core.Architecture;
using System.Collections;
using UnityEngine;

public class WildMover : MonoBehaviour
{
	[SerializeField]
	private WildPath path;

	[SerializeField]
	private AnimationCurve movingPlan = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	[SerializeField]
	private float duration = 5f;

	[Header("Events")]
	[SerializeField]
	private OrderedEventDispatcher onFinish = new OrderedEventDispatcher();

	[Header("Pre-points")]
	private Vector3 startPos;

	private Vector3 finishPos;

	private Coroutine movingCoroutine;

	public float Duration
	{
		get
		{
			return duration;
		}
		set
		{
			duration = value;
		}
	}

	public Vector3 StartPos
	{
		get
		{
			return startPos;
		}
		set
		{
			startPos = value;
		}
	}

	public Vector3 FinishPos
	{
		get
		{
			return finishPos;
		}
		set
		{
			finishPos = value;
		}
	}

	public void OnDisable()
	{
		if (movingCoroutine != null)
		{
			StopCoroutine(movingCoroutine);
			movingCoroutine = null;
		}
	}

	[ContextMenu("StartMoving")]
	public void StartMoving()
	{
		StartMoving(StartPos, FinishPos);
	}

	public void StartMoving(Vector3 startPoint, Vector3 finishPoint)
	{
		movingCoroutine = null;
		path.SetPoints(startPoint, finishPoint);
		movingCoroutine = StartCoroutine(Move());
	}

	private IEnumerator Move()
	{
		float timeTracking = 0f;
		do
		{
			float pathTracking = movingPlan.Evaluate(timeTracking / Duration);
			base.transform.position = path.GetPosition(pathTracking);
			timeTracking += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		while (timeTracking <= Duration);
		movingCoroutine = null;
		onFinish.Dispatch();
	}

	[ContextMenu("SetUniformPlan")]
	private void SetUniformPlan()
	{
		movingPlan = AnimationCurve.Linear(0f, 0f, 1f, 1f);
	}
}
