using System;
using System.Threading;
using UnityEngine;

public class WaitForThreadedTaskWithChecker : CustomYieldInstruction
{
	private TaskCompleteChecker checker;

	public override bool keepWaiting => !checker.isTaskCompleted;

	public WaitForThreadedTaskWithChecker(Action task, TaskCompleteChecker checker, System.Threading.ThreadPriority priority = System.Threading.ThreadPriority.Normal)
	{
		this.checker = checker;
		new Thread((ThreadStart)delegate
		{
			task();
		}).Start(priority);
	}
}
