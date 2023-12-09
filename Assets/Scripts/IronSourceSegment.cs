using System.Collections.Generic;
using System.Linq;

public class IronSourceSegment
{
	public int age;

	public string gender;

	public int level;

	public int isPaying;

	public long userCreationDate;

	public double iapt;

	public string segmentName;

	public Dictionary<string, string> customs;

	public IronSourceSegment()
	{
		customs = new Dictionary<string, string>();
		age = -1;
		level = -1;
		isPaying = -1;
		userCreationDate = -1L;
		iapt = 0.0;
	}

	public void setCustom(string key, string value)
	{
		customs.Add(key, value);
	}

	public Dictionary<string, string> getSegmentAsDict()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (age != -1)
		{
			dictionary.Add("age", age + string.Empty);
		}
		if (!string.IsNullOrEmpty(gender))
		{
			dictionary.Add("gender", gender);
		}
		if (level != -1)
		{
			dictionary.Add("level", level + string.Empty);
		}
		if (isPaying > -1 && isPaying < 2)
		{
			dictionary.Add("isPaying", isPaying + string.Empty);
		}
		if (userCreationDate != -1)
		{
			dictionary.Add("userCreationDate", userCreationDate + string.Empty);
		}
		if (!string.IsNullOrEmpty(segmentName))
		{
			dictionary.Add("segmentName", segmentName);
		}
		if (iapt > 0.0)
		{
			dictionary.Add("iapt", iapt + string.Empty);
		}
		return (from d in dictionary.Concat(customs)
			group d by d.Key).ToDictionary((IGrouping<string, KeyValuePair<string, string>> d) => d.Key, (IGrouping<string, KeyValuePair<string, string>> d) => d.First().Value);
	}
}
