using System;
using System.Collections.Generic;
using UnityEngine;

public class F3DTime : MonoBehaviour
{
	private class Timer
	{
		public int id;

		public bool isActive;

		public float rate;

		public int ticks;

		public int ticksElapsed;

		public float last;

		public Action callBack;

		public Timer(int id_, float rate_, int ticks_, Action callback_)
		{
			id = id_;
			rate = ((!(rate_ < 0f)) ? rate_ : 0f);
			ticks = ((ticks_ >= 0) ? ticks_ : 0);
			callBack = callback_;
			last = 0f;
			ticksElapsed = 0;
			isActive = true;
		}

		public void Tick()
		{
			last += Time.deltaTime;
			if (isActive && last >= rate)
			{
				last = 0f;
				ticksElapsed++;
				callBack();
				if (ticks > 0 && ticks == ticksElapsed)
				{
					isActive = false;
					time.RemoveTimer(id);
				}
			}
		}
	}

	public static F3DTime time;

	private List<Timer> timers;

	private List<int> removalPending;

	private int idCounter;

	private void Awake()
	{
		time = this;
		timers = new List<Timer>();
		removalPending = new List<int>();
	}

	public int AddTimer(float rate, Action callBack)
	{
		return AddTimer(rate, 0, callBack);
	}

	public int AddTimer(float rate, int ticks, Action callBack)
	{
		Timer timer = new Timer(++idCounter, rate, ticks, callBack);
		timers.Add(timer);
		return timer.id;
	}

	public void RemoveTimer(int timerId)
	{
		removalPending.Add(timerId);
	}

	private void Remove()
	{
		if (removalPending.Count > 0)
		{
			foreach (int item in removalPending)
			{
				for (int i = 0; i < timers.Count; i++)
				{
					if (timers[i].id == item)
					{
						timers.RemoveAt(i);
						break;
					}
				}
			}
			removalPending.Clear();
		}
	}

	private void Tick()
	{
		for (int i = 0; i < timers.Count; i++)
		{
			timers[i].Tick();
		}
	}

	private void Update()
	{
		Remove();
		Tick();
	}
}
