using UnityEngine;
using UnityEngine.Events;

public class UnityAnimation : MonoBehaviour
{
	[HideInInspector]
	public bool isRunning;

	[SerializeField]
	private UnityEvent play = new UnityEvent();

	[SerializeField]
	private UnityEvent stop = new UnityEvent();

	public void Play()
	{
		isRunning = true;
		play.Invoke();
	}

	public void Stop()
	{
		isRunning = false;
		stop.Invoke();
	}
}
