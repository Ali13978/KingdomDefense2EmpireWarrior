using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningBoltPathScript : LightningBoltPathScriptBase
	{
		[Tooltip("How fast the lightning moves through the points or objects. 1 is normal speed, 0.01 is slower, so the lightning will move slowly between the points or objects.")]
		[Range(0.01f, 1f)]
		public float Speed = 1f;

		[SingleLineClamp("When each new point is moved to, this can provide a random value to make the movement to the next point appear more staggered or random. Leave as 1 and 1 to have constant speed. Use a higher maximum to create more randomness.", 1.0, 500.0)]
		public RangeOfFloats SpeedIntervalRange = new RangeOfFloats
		{
			Minimum = 1f,
			Maximum = 1f
		};

		[Tooltip("Repeat when the path completes?")]
		public bool Repeat = true;

		private float nextInterval = 1f;

		private int nextIndex;

		private Vector3? lastPoint;

		public override void CreateLightningBolt(LightningBoltParameters parameters)
		{
			Vector3? vector = null;
			List<GameObject> currentPathObjects = GetCurrentPathObjects();
			if (currentPathObjects.Count < 2)
			{
				return;
			}
			if (nextIndex >= currentPathObjects.Count)
			{
				if (!Repeat)
				{
					return;
				}
				if (currentPathObjects[currentPathObjects.Count - 1] == currentPathObjects[0])
				{
					nextIndex = 1;
				}
				else
				{
					nextIndex = 0;
					lastPoint = null;
				}
			}
			try
			{
				Vector3? vector2 = lastPoint;
				if (!vector2.HasValue)
				{
					lastPoint = currentPathObjects[nextIndex++].transform.position;
				}
				vector = currentPathObjects[nextIndex].transform.position;
				Vector3? vector3 = lastPoint;
				if (vector3.HasValue && vector.HasValue)
				{
					parameters.Start = lastPoint.Value;
					parameters.End = vector.Value;
					base.CreateLightningBolt(parameters);
					if ((nextInterval -= Speed) <= 0f)
					{
						float num = UnityEngine.Random.Range(SpeedIntervalRange.Minimum, SpeedIntervalRange.Maximum);
						nextInterval = num + nextInterval;
						lastPoint = vector;
						nextIndex++;
					}
				}
			}
			catch (NullReferenceException)
			{
			}
		}

		public void Reset()
		{
			lastPoint = null;
			nextIndex = 0;
			nextInterval = 1f;
		}
	}
}
