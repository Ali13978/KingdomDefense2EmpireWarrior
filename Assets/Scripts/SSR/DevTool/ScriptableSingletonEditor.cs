using System;
using UnityEngine;

namespace SSR.DevTool
{
	public sealed class ScriptableSingletonEditor : ScriptableObject
	{
		private const string assetName = "ScriptableSingletonEditor_ASSET";

		private static ScriptableSingletonEditor instance;

		public static ScriptableSingletonEditor Instance
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

		private static ScriptableSingletonEditor CreateAsset()
		{
			return null;
		}

		private static ScriptableSingletonEditor LoadAsset()
		{
			return null;
		}
	}
}
