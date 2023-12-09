using TMPro;
using UnityEngine;

namespace Gameplay
{
	public class TimerBombCountdown : MonoBehaviour
	{
		private float duration;

		private float timeTracking;

		private bool startCountDown;

		[SerializeField]
		private TextMesh textMesh;

		[SerializeField]
		private TextMeshPro textMeshPro;

		[SerializeField]
		private bool intType;

		[SerializeField]
		private bool floatType;

		private void Update()
		{
			if (!startCountDown || SingletonMonoBehaviour<GameData>.Instance.IsPause)
			{
				return;
			}
			if ((bool)textMesh)
			{
				if (intType)
				{
					textMesh.text = ((int)timeTracking + 1).ToString();
				}
				if (floatType)
				{
					textMesh.text = $"{timeTracking:f1}";
				}
			}
			if ((bool)textMeshPro)
			{
				if (intType)
				{
					textMeshPro.text = ((int)timeTracking + 1).ToString();
				}
				if (floatType)
				{
					textMeshPro.text = $"{timeTracking:f1}";
				}
			}
			if (IsCountDownReachZero())
			{
				FinishCountDown();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public void Init(float duration)
		{
			this.duration = duration;
			timeTracking = duration;
			startCountDown = true;
		}

		private bool IsCountDownReachZero()
		{
			return timeTracking == 0f;
		}

		private void FinishCountDown()
		{
			timeTracking = 0f;
			startCountDown = false;
		}
	}
}
