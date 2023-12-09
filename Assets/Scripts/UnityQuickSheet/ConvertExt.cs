using System;
using System.Linq;

namespace UnityQuickSheet
{
	public class ConvertExt
	{
		public static object[] Split(string value)
		{
			string text = new string((from ch in value.ToCharArray()
				where !char.IsWhiteSpace(ch)
				select ch).ToArray());
			char[] trimChars = new char[2]
			{
				',',
				' '
			};
			text = text.TrimEnd(trimChars);
			return text.Split(',');
		}

		public static float[] ToSingleArray(string value)
		{
			object[] source = Split(value);
			return (from e in source
				select Convert.ChangeType(e, typeof(float)) into e
				select (float)e).ToArray();
		}

		public static double[] ToDoubleArray(string value)
		{
			object[] source = Split(value);
			return (from e in source
				select Convert.ChangeType(e, typeof(double)) into e
				select (double)e).ToArray();
		}

		public static short[] ToInt16Array(string value)
		{
			object[] source = Split(value);
			return (from e in source
				select Convert.ChangeType(e, typeof(short)) into e
				select (short)e).ToArray();
		}

		public static int[] ToInt32Array(string value)
		{
			object[] source = Split(value);
			return (from e in source
				select Convert.ChangeType(e, typeof(int)) into e
				select (int)e).ToArray();
		}

		public static long[] ToInt64Array(string value)
		{
			object[] source = Split(value);
			return (from e in source
				select Convert.ChangeType(e, typeof(long)) into e
				select (long)e).ToArray();
		}

		public static string[] ToStringArray(string value)
		{
			object[] source = Split(value);
			return (from e in source
				select Convert.ChangeType(e, typeof(string)) into e
				select (string)e).ToArray();
		}
	}
}
