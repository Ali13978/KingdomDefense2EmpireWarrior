using Data;
using SSR.Core.Architecture;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
	public abstract class TutorialUnit : MonoBehaviour
	{
		[Space]
		[SerializeField]
		private OrderedEventDispatcher onShowTutorial;

		[SerializeField]
		private OrderedEventDispatcher onPassedTutorial;

		[Space]
		[SerializeField]
		private List<OrderedEventDispatcher> steps = new List<OrderedEventDispatcher>();

		private bool showed;

		private int currentStep;

		protected abstract bool ShouldShowTutorial();

		protected abstract void SaveTutorialPassed();

		public void CheckCondition()
		{
			if (ShouldShowTutorial())
			{
				ReadWriteDataTutorial.Instance.currentTutorial = this;
				UnityEngine.Debug.Log("Show Tut thôi!");
				showed = true;
				onShowTutorial.Dispatch();
				TryDispactStep(0);
			}
			else
			{
				UnityEngine.Debug.Log("Đã hoàn thành tut, hoặc không đủ điều kiện để hiện!");
			}
		}

		public void SetTutorialPassed()
		{
			if (showed)
			{
				showed = false;
				onPassedTutorial.Dispatch();
				SaveTutorialPassed();
			}
		}

		public void TryToSetTutorialPassed()
		{
			onPassedTutorial.Dispatch();
			SaveTutorialPassed();
		}

		public void TryToSaveTutorialPassed()
		{
			SaveTutorialPassed();
		}

		public void NextStep()
		{
			if (showed && currentStep < steps.Count - 1)
			{
				currentStep++;
				TryDispactStep(currentStep);
			}
		}

		public void TryMoveToStep(int stepId)
		{
			if (showed && currentStep <= stepId && currentStep != stepId)
			{
				TryDispactStep(stepId);
			}
		}

		private void TryDispactStep(int stepId)
		{
			while (stepId < steps.Count && steps[stepId].UnityEventsCount <= 0)
			{
				stepId++;
			}
			if (stepId < steps.Count)
			{
				currentStep = stepId;
				steps[currentStep].Dispatch();
			}
		}
	}
}
