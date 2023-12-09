using Data;
using UnityEngine;

namespace HotFix
{
	public class LostHeroesDataFixer : MonoBehaviour
	{
		private void Start()
		{
			Fix();
		}

		private void Fix()
		{
			if (!ReadWriteDataHero.Instance.IsHeroOwned(1) && ReadWriteDataMap.Instance.GetMapIDUnlocked() >= 2)
			{
				ReadWriteDataHero.Instance.UnlockHero(1);
			}
		}
	}
}
