using Middle;

namespace Parameter
{
	public class StarParameter
	{
		private static StarParameter instance;

		public static StarParameter Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new StarParameter();
				}
				return instance;
			}
		}

		public int GetStar(int percent)
		{
			int result = 0;
			if (percent > 0 && percent <= Config.Instance.LifePercent2Star)
			{
				result = 1;
			}
			if (percent > Config.Instance.LifePercent2Star && percent < Config.Instance.LifePercent3Star)
			{
				result = 2;
			}
			else if (percent >= Config.Instance.LifePercent3Star)
			{
				result = 3;
			}
			return result;
		}
	}
}
