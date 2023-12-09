using UnityEngine;

namespace Data
{
	public class WorldMapData : MonoBehaviour
	{
		[SerializeField]
		private ReadDataMapRule readDataMapRule;

		public ReadDataMapRule ReadDataMapRule
		{
			get
			{
				return readDataMapRule;
			}
			set
			{
				readDataMapRule = value;
			}
		}

		public static WorldMapData Instance
		{
			get;
			set;
		}

		private void Awake()
		{
			if ((bool)Instance)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				Instance = this;
			}
		}
	}
}
