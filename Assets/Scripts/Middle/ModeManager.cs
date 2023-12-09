namespace Middle
{
	public class ModeManager
	{
		private static ModeManager instance;

		public GameMode gameMode;

		public static ModeManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ModeManager();
				}
				return instance;
			}
		}
	}
}
