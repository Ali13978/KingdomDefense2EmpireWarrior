using System.Collections.Generic;

namespace Gameplay
{
	public class BuildRegionManager : SingletonMonoBehaviour<BuildRegionManager>
	{
		public List<BuildRegionController> listRegions = new List<BuildRegionController>();

		public void InvokeClickk(int regionID)
		{
			listRegions[regionID].TryToClick();
		}
	}
}
