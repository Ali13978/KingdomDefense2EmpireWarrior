using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public static class CurrencyFormat
{
	public static readonly Dictionary<string, CultureInfo> ISOCurrenciesToACultureMap = (from c in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
		select new
		{
			c,
			new RegionInfo(c.LCID).ISOCurrencySymbol
		} into x
		group x by x.ISOCurrencySymbol).ToDictionary(g => g.Key, g => g.First().c, StringComparer.OrdinalIgnoreCase);

	private static string GetFormatCurrency(string ISOCurrencyCode, decimal amount)
	{
		int num = BitConverter.GetBytes(decimal.GetBits(amount)[3])[2];
		if (ISOCurrenciesToACultureMap.TryGetValue(ISOCurrencyCode, out CultureInfo value))
		{
			return amount.ToString("C" + num, value);
		}
		return amount.ToString("0.00");
	}
}
