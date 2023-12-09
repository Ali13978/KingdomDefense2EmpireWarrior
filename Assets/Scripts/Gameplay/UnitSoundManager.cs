using UnityEngine;

namespace Gameplay
{
	public class UnitSoundManager : SingletonMonoBehaviour<UnitSoundManager>
	{
		[SerializeField]
		private ReadDataVolumeAdjust readDataVolumeAdjust;

		public ReadDataVolumeAdjust ReadDataVolumeAdjust
		{
			get
			{
				return readDataVolumeAdjust;
			}
			set
			{
				readDataVolumeAdjust = value;
			}
		}
	}
}
