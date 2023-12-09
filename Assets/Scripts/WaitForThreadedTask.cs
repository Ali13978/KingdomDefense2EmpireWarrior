using System;
using System.Threading;
using UnityEngine;

internal class WaitForThreadedTask : CustomYieldInstruction
{
	private bool isRunning;

	public override bool keepWaiting => isRunning;

	public WaitForThreadedTask(Action task, System.Threading.ThreadPriority priority = System.Threading.ThreadPriority.Normal)
	{
		isRunning = true;
		new Thread((ThreadStart)delegate
		{
			task();
			isRunning = false;
		}).Start(priority);
	}
}
