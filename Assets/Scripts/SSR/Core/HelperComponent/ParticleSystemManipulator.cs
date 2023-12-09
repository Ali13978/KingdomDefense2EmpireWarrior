using UnityEngine;

namespace SSR.Core.HelperComponent
{
	[RequireComponent(typeof(ParticleSystem))]
	public abstract class ParticleSystemManipulator : MonoBehaviour
	{
		protected enum SystemState
		{
			Stop,
			Play,
			Pause,
			Unkown
		}

		[SerializeField]
		[HideInInspector]
		private ParticleSystem targetParticleSystem;

		[SerializeField]
		[ReadOnly]
		private SystemState currentState;

		public ParticleSystem TargetParticleSystem => targetParticleSystem;

		protected SystemState CurrentState
		{
			get
			{
				return currentState;
			}
			private set
			{
				currentState = value;
			}
		}

		public void Awake()
		{
			currentState = DetermineCurrentSystemState(currentState, dispatchEvent: false);
		}

		public void Update()
		{
			currentState = DetermineCurrentSystemState(currentState, dispatchEvent: true);
			if (currentState == SystemState.Play)
			{
				UpdateParticleSystem();
			}
		}

		public void Reset()
		{
			targetParticleSystem = GetComponent<ParticleSystem>();
		}

		private SystemState DetermineCurrentSystemState(SystemState oldState, bool dispatchEvent)
		{
			if (targetParticleSystem.isStopped)
			{
				if (dispatchEvent && oldState != 0)
				{
					OnStop();
				}
				return SystemState.Stop;
			}
			if (targetParticleSystem.isPlaying)
			{
				if (dispatchEvent && oldState != SystemState.Play)
				{
					OnPlay();
				}
				return SystemState.Play;
			}
			if (targetParticleSystem.isPaused)
			{
				if (dispatchEvent && oldState != SystemState.Pause)
				{
					OnPause();
				}
				return SystemState.Pause;
			}
			return SystemState.Unkown;
		}

		protected virtual void OnStop()
		{
		}

		protected virtual void OnPause()
		{
		}

		protected virtual void OnPlay()
		{
		}

		protected abstract void UpdateParticleSystem();
	}
}
