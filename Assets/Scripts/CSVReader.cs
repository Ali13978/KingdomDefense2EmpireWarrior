using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVReader
{
	private static string SPLIT_RE = ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))";

	private static string LINE_SPLIT_RE = "\\r\\n|\\n\\r|\\n|\\r";

	private static char[] TRIM_CHARS = new char[1]
	{
		'"'
	};

	public static List<Dictionary<string, object>> Read(string file)
	{
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		TextAsset textAsset = Resources.Load(file) as TextAsset;
		string[] array = Regex.Split(textAsset.text, LINE_SPLIT_RE);
		if (array.Length <= 1)
		{
			return list;
		}
		string[] array2 = Regex.Split(array[0], SPLIT_RE);
		for (int i = 1; i < array.Length; i++)
		{
			string[] array3 = Regex.Split(array[i], SPLIT_RE);
			if (array3.Length == 0 || array3[0] == string.Empty)
			{
				continue;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			for (int j = 0; j < array2.Length && j < array3.Length; j++)
			{
				string text = array3[j];
				text = text.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", string.Empty);
				object value = text;
				float result2;
				if (int.TryParse(text, out int result))
				{
					value = result;
				}
				else if (float.TryParse(text, out result2))
				{
					value = result2;
				}
				dictionary[array2[j]] = value;
			}
			list.Add(dictionary);
		}
		return list;
	}
}
