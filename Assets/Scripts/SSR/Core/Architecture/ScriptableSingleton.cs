using System;
using UnityEngine;

namespace SSR.Core.Architecture
{
	public sealed class ScriptableSingleton : ScriptableObject
	{
		private const string assetName = "ScriptableSingleton_ASSET";

		private static ScriptableSingleton instance;

		public static ScriptableSingleton Instance
		{
			get
			{
				if (instance == null)
				{
					instance = LoadAsset();
					if (instance == null)
					{
						instance = CreateAsset();
					}
					instance.Initialize();
				}
				return instance;
			}
		}

		private void Initialize()
		{
			throw new NotImplementedException();
		}

		private static ScriptableSingleton CreateAsset()
		{
			return null;
		}

		private static ScriptableSingleton LoadAsset()
		{
			string path = ApplicationConstances.ApplicationDataResourcePath + "/ScriptableSingleton_ASSET";
			return Resources.Load<ScriptableSingleton>(path);
		}
	}
}
