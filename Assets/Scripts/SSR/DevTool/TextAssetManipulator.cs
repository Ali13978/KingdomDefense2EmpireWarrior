using System;
using System.IO;
using UnityEngine;

namespace SSR.DevTool
{
	public static class TextAssetManipulator
	{
		public static void ChangeBytesData(TextAsset textAsset, byte[] bytes, bool refresh)
		{
			throw new NotSupportedException();
		}

		public static void ChangeTextData(TextAsset textAsset, string text, bool refresh)
		{
			throw new NotSupportedException();
		}

		public static TextAsset CreateTextAsset(string text, string path)
		{
			throw new NotSupportedException();
		}

		public static TextAsset CreateTextAsset(byte[] bytes, string path)
		{
			throw new NotSupportedException();
		}

		private static void SaveFile(string text, string path)
		{
			using (StreamWriter streamWriter = File.CreateText(path))
			{
				streamWriter.Write(text);
			}
		}

		private static void CreateTextAssetGeneral(string extension)
		{
		}
	}
}
