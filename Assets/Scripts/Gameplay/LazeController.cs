using MyCustom;
using UnityEngine;

namespace Gameplay
{
	public class LazeController : CustomMonoBehaviour
	{
		private LineRenderer _line;

		[SerializeField]
		private GameObject effectStart;

		[SerializeField]
		private GameObject effectEnd;

		[SerializeField]
		private Transform firePosition;

		private bool isRunning;

		private GameObject target;

		private Vector3 targetLastPos = Vector3.zero;

		private void Awake()
		{
			_line = GetComponent<LineRenderer>();
			_line.useWorldSpace = true;
			InitSize();
		}

		public void Init()
		{
			if (!_line)
			{
				_line = GetComponent<LineRenderer>();
				_line.useWorldSpace = true;
			}
			isRunning = true;
			InitSize();
		}

		private void InitSize()
		{
			_line.SetPosition(0, firePosition.position);
			_line.SetPosition(1, firePosition.position);
		}

		public void Resize(Vector3 endPos)
		{
			_line.SetPosition(0, firePosition.position);
			_line.SetPosition(1, endPos);
			effectStart.SetActive(value: true);
			effectEnd.SetActive(value: true);
			effectStart.transform.position = firePosition.position;
			effectEnd.transform.position = endPos;
		}

		public void StopImmediate()
		{
			InitSize();
			isRunning = false;
			target = null;
			effectStart.SetActive(value: false);
			effectEnd.SetActive(value: false);
		}
	}
}
