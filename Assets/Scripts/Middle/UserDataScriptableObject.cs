using SSR.Core;
using UnityEngine;

namespace Middle
{
	public class UserDataScriptableObject : ScriptableObject
	{
		private const string assetName = "UserDataScriptableObject_ASSET";

		private static UserDataScriptableObject instance;

		[SerializeField]
		private GlobalUpgradeProgress globalUpgradeProgress;

		public static UserDataScriptableObject Instance
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

		public GlobalUpgradeProgress GlobalUpgradeProgress
		{
			get
			{
				return globalUpgradeProgress;
			}
			private set
			{
				globalUpgradeProgress = value;
			}
		}

		private void Initialize()
		{
		}

		private static UserDataScriptableObject CreateAsset()
		{
			return null;
		}

		private static UserDataScriptableObject LoadAsset()
		{
			string path = ApplicationConstances.ApplicationDataResourcePath + "/UserDataScriptableObject_ASSET";
			return Resources.Load<UserDataScriptableObject>(path);
		}
	}
}
