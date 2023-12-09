using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SSR.DevTool
{
	public static class FileNameUtil
	{
		public static string GetFileName(string filePath)
		{
			string[] array = filePath.Split('/', '\\');
			return array[array.Length - 1];
		}

		public static string GetParentFolder(string filePath)
		{
			List<string> list = new List<string>(filePath.Split('/', '\\'));
			list.RemoveAt(list.Count - 1);
			return string.Join("/", list.ToArray());
		}

		public static List<string> GetAssetsFilePathFromFolder(string folderPath, string searchPattern)
		{
			List<string> list = new List<string>();
			string[] files = Directory.GetFiles(folderPath, searchPattern);
			foreach (string text in files)
			{
				string item = text.Replace(Application.dataPath, string.Empty).Replace('\\', '/');
				list.Add(item);
			}
			return list;
		}
	}
}
