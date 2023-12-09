using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class MultiLanguaguageDataReader : MonoBehaviour
	{
		[SerializeField]
		private List<ReadCommonDescription> listDescriptionReader = new List<ReadCommonDescription>();

		private void Awake()
		{
			ReadParameters();
		}

		private void ReadParameters()
		{
			foreach (ReadCommonDescription item in listDescriptionReader)
			{
				item.ReadParameter();
			}
		}

		public void ReloadParameters()
		{
			ReadParameters();
		}
	}
}
