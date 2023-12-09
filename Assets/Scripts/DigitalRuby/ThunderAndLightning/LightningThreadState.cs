using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningThreadState
	{
		private Thread lightningThread;

		private AutoResetEvent lightningThreadEvent = new AutoResetEvent(initialState: false);

		private readonly Queue<Action> actionsForBackgroundThread = new Queue<Action>();

		private readonly Queue<KeyValuePair<Action, ManualResetEvent>> actionsForMainThread = new Queue<KeyValuePair<Action, ManualResetEvent>>();

		public bool Running = true;

		private bool isTerminating;

		public LightningThreadState()
		{
			lightningThread = new Thread(BackgroundThreadMethod)
			{
				IsBackground = true,
				Name = "LightningBoltScriptThread"
			};
			lightningThread.Start();
		}

		private bool UpdateMainThreadActionsOnce()
		{
			KeyValuePair<Action, ManualResetEvent> keyValuePair;
			lock (actionsForMainThread)
			{
				if (actionsForMainThread.Count == 0)
				{
					return false;
				}
				keyValuePair = actionsForMainThread.Dequeue();
			}
			keyValuePair.Key();
			if (keyValuePair.Value != null)
			{
				keyValuePair.Value.Set();
			}
			return true;
		}

		private void BackgroundThreadMethod()
		{
			Action action = null;
			while (Running)
			{
				try
				{
					if (lightningThreadEvent.WaitOne(500))
					{
						while (true)
						{
							lock (actionsForBackgroundThread)
							{
								if (actionsForBackgroundThread.Count != 0)
								{
									action = actionsForBackgroundThread.Dequeue();
									goto IL_005b;
								}
							}
							break;
							IL_005b:
							action();
						}
					}
				}
				catch (ThreadAbortException)
				{
				}
				catch (Exception ex2)
				{
					UnityEngine.Debug.LogErrorFormat("Lightning thread exception: {0}", ex2);
				}
			}
		}

		public void TerminateAndWaitForEnd()
		{
			isTerminating = true;
			while (true)
			{
				if (!UpdateMainThreadActionsOnce())
				{
					lock (actionsForBackgroundThread)
					{
						if (actionsForBackgroundThread.Count == 0)
						{
							return;
						}
					}
				}
			}
		}

		public void UpdateMainThreadActions()
		{
			while (UpdateMainThreadActionsOnce())
			{
			}
		}

		public bool AddActionForMainThread(Action action, bool waitForAction = false)
		{
			if (isTerminating)
			{
				return false;
			}
			ManualResetEvent manualResetEvent = (!waitForAction) ? null : new ManualResetEvent(initialState: false);
			lock (actionsForMainThread)
			{
				actionsForMainThread.Enqueue(new KeyValuePair<Action, ManualResetEvent>(action, manualResetEvent));
			}
			manualResetEvent?.WaitOne(10000);
			return true;
		}

		public bool AddActionForBackgroundThread(Action action)
		{
			if (isTerminating)
			{
				return false;
			}
			lock (actionsForBackgroundThread)
			{
				actionsForBackgroundThread.Enqueue(action);
			}
			lightningThreadEvent.Set();
			return true;
		}
	}
}
