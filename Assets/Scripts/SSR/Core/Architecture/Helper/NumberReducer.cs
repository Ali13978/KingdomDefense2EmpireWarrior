namespace SSR.Core.Architecture.Helper
{
	public static class NumberReducer
	{
		private const float OneMega = 1000000f;

		private const float OneKilo = 1000f;

		public static string ToMegaFormat(float value)
		{
			if (value >= 1000000f)
			{
				return $"{value / 1000000f:00}M";
			}
			return value.ToString();
		}

		public static string ToKiloFormat(float value)
		{
			if (value >= 1000f)
			{
				return $"{value / 1000f:00}K";
			}
			return value.ToString();
		}
	}
}
