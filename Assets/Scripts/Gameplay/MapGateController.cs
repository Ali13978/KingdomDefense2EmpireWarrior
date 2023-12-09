using MyCustom;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class MapGateController : CustomMonoBehaviour
	{
		[SerializeField]
		private bool gateForRunningEnemies;

		[SerializeField]
		private bool gateForFlyingEnemies;

		[NonSerialized]
		public List<GameObject> gates;

		private bool isInited;

		public void Awake()
		{
			Init();
		}

		public void Init()
		{
			if (!isInited)
			{
				isInited = true;
				gates = new List<GameObject>();
				IEnumerator enumerator = base.transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Transform transform = (Transform)enumerator.Current;
						gates.Add(transform.gameObject);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
		}
	}
}
