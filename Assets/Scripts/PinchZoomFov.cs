using DG.Tweening;
using Gameplay;
using UnityEngine;

public class PinchZoomFov : TouchLogic
{
	private const float Vector2DZ = 10f;

	[SerializeField]
	private float movementSesitivity = 100f;

	[SerializeField]
	private float movementThresHole;

	[SerializeField]
	private float zoomSensitivity = 100f;

	public float zoomSpeed = 5f;

	public float heightBG = 7.68f;

	public float widthBG = 10.24f;

	public float intialFov = -10f;

	public float maxFov = -5f;

	public float minFov = -10f;

	private Vector2 currTouch1 = Vector2.zero;

	private Vector2 lastTouch1 = Vector2.zero;

	private Vector2 currTouch2 = Vector2.zero;

	private Vector2 lastTouch2 = Vector2.zero;

	private float currDist;

	private float lastDist;

	private float zoomFactor;

	private float xMin;

	private float xMax;

	private float yMin;

	private float yMax;

	private float mapHeight = 9.6f;

	private float mapWidth = 12.8f;

	private float ratioScreen;

	private Vector3 originalPos = new Vector3(0f, 0f, -10f);

	private Tweener tween;

	private void Start()
	{
		ratioScreen = (float)Screen.width / (float)Screen.height;
		maxFov = Camera.main.orthographicSize;
		InputFilterManager.Instance.onMoveCamera += Instance_onMoveCamera;
		InputFilterManager.Instance.onZoomCamera += Instance_onZoomCamera;
	}

	private void OnDestroy()
	{
		InputFilterManager.Instance.onMoveCamera -= Instance_onMoveCamera;
		InputFilterManager.Instance.onZoomCamera -= Instance_onZoomCamera;
	}

	private new void Update()
	{
		ClampZoom();
		ClampMove();
	}

	private void Instance_onMoveCamera()
	{
		TryToMove();
		ClampMove();
	}

	private void Instance_onZoomCamera()
	{
		TryToZoom();
		ClampMove();
	}

	private void TryToZoom()
	{
		Camera main = Camera.main;
		currTouch1 = GetWorldPosition(UnityEngine.Input.GetTouch(0).position, main);
		lastTouch1 = currTouch1 - GetDeltaWorldPosition(UnityEngine.Input.GetTouch(0).deltaPosition, main);
		currTouch2 = GetWorldPosition(UnityEngine.Input.GetTouch(1).position, main);
		lastTouch2 = currTouch2 - GetDeltaWorldPosition(UnityEngine.Input.GetTouch(1).deltaPosition, main);
		currDist = Vector2.Distance(currTouch1, currTouch2);
		lastDist = Vector2.Distance(lastTouch1, lastTouch2);
		zoomFactor = Mathf.Clamp(lastDist - currDist, -30f, 30f) * zoomSensitivity;
		float value = Camera.main.orthographicSize + zoomFactor * zoomSpeed * Time.unscaledDeltaTime;
		value = Mathf.Clamp(value, minFov, maxFov);
		Camera.main.orthographicSize = value;
	}

	private void TryToMove()
	{
		Vector3 position = Camera.main.transform.position;
		float z = position.z;
		Vector2 deltaWorldPosition = GetDeltaWorldPosition(UnityEngine.Input.GetTouch(0).deltaPosition, Camera.main);
		Camera.main.transform.Translate(deltaWorldPosition * Time.unscaledDeltaTime * z / movementSesitivity);
	}

	public void TryToMoveToBuildTowerPosition(Vector2 towerPosition)
	{
		float num = mapWidth / 2f / xMax;
		float num2 = mapHeight / 2f / yMax;
		ShortcutExtensions.DOMove(endValue: new Vector2(towerPosition.x / num, towerPosition.y / num2), target: base.transform, duration: 0.2f);
		ClampMove();
	}

	public void MoveToOriginPos()
	{
		base.transform.DOMove(originalPos, 1.5f);
		float currentSize = Camera.main.orthographicSize;
		float size;
		Tweener tweener = DOTween.To(() => currentSize, delegate(float x)
		{
			size = x;
			Camera.main.orthographicSize = size;
		}, maxFov, 1.5f).SetEase(Ease.Linear);
	}

	public void MoveAndZoomToPosition(Vector3 pos, float time, float targetZoom = 3f)
	{
		ZoomIn(time, targetZoom);
		base.transform.DOMove(pos, time).OnComplete(OnMoveComplete);
		ClampMove();
	}

	public void OnMoveComplete()
	{
	}

	private void ZoomIn(float duration, float targetValue)
	{
		tween = DOTween.To(() => maxFov, delegate(float x)
		{
			Camera.main.orthographicSize = x;
		}, targetValue, duration).SetEase(Ease.Linear);
		ClampZoom();
	}

	private Vector2 GetDeltaWorldPosition(Vector2 deltaScreen, Camera camera)
	{
		Vector3 position = deltaScreen;
		position.z = 10f;
		Vector3 position2 = new Vector3(0f, 0f, 10f);
		return camera.ScreenToWorldPoint(position) - camera.ScreenToWorldPoint(position2);
	}

	private Vector2 GetWorldPosition(Vector2 positionScreen, Camera camera)
	{
		return camera.ScreenToWorldPoint(new Vector3(positionScreen.x, positionScreen.y, 10f));
	}

	private void ClampZoom()
	{
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minFov, maxFov);
	}

	private void ClampMove()
	{
		float num = Camera.main.orthographicSize * 2f;
		float num2 = num * (float)Screen.width / (float)Screen.height;
		xMax = (mapWidth - num2) / 2f;
		xMin = 0f - xMax;
		yMax = (mapHeight - num) / 2f;
		yMin = 0f - yMax;
		Transform transform = Camera.main.transform;
		Vector3 position = base.transform.position;
		float x = Mathf.Clamp(position.x, xMin, xMax);
		Vector3 position2 = base.transform.position;
		transform.position = new Vector3(x, Mathf.Clamp(position2.y, yMin, yMax), -10f);
	}
}
