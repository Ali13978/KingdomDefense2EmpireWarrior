namespace SSR.Core.Architecture
{
	public abstract class Singleton<T> where T : new()
	{
		private static bool instantiated;

		private static T instance;

		public static T Instance
		{
			get
			{
				if (!instantiated)
				{
					instance = new T();
					instantiated = true;
				}
				return instance;
			}
		}
	}
}
