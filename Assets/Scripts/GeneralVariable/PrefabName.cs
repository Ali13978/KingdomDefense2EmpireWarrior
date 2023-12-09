namespace GeneralVariable
{
	public class PrefabName
	{
		private string towerPrefix = "Towers/tower_";

		private string bulletPrefix = "Bullets/bullet_";

		private string mapPrefix = "Maps/map_";

		private string explosionPrefix = "Explosions/explosion_";

		private static PrefabName instance;

		public static PrefabName Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new PrefabName();
				}
				return instance;
			}
		}

		public string GetTowerPrefabName(int id, int level)
		{
			return towerPrefix + id + "_" + level;
		}

		public string GetBulletPrefabName(int id, int level)
		{
			return bulletPrefix + id + "_" + level;
		}

		public string GetExplosionPrefabName(int id)
		{
			return explosionPrefix + id;
		}

		public string GetMapPrefabName(int id)
		{
			return mapPrefix + id;
		}

		public string GetAirCraftRocketExplosion()
		{
			return explosionPrefix + "rocket";
		}

		public string GetMenuBackgroundMusicPrefabName()
		{
			return "AudioSources/Background/mainTheme";
		}

		public string GetGameplayBackgroundMusicPrefabName(int themeId)
		{
			string result = string.Empty;
			switch (themeId)
			{
			case 0:
				result = "AudioSources/Background/gameplay_theme1";
				break;
			case 1:
				result = "AudioSources/Background/gameplay_theme2";
				break;
			case 2:
				result = "AudioSources/Background/gameplay_theme3";
				break;
			}
			return result;
		}
	}
}
