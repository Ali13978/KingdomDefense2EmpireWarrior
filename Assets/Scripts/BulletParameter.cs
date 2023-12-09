using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
	[Serializable]
	public struct BulletParameter
	{
		public Transform gunBarrel;

		public float delayTime;

		public int bulletDirection;

		public UnityEvent OnCreateBullet;
	}
}
