using System.IO;
using UnityEngine;

namespace SSR.Core.Architecture.WritableData
{
	public static class PersistentFileHelper
	{
		public static void SaveFile(byte[] bytes, string fileName)
		{
			BinaryFileHelper.SaveFile(bytes, GetPath(fileName));
		}

		public static byte[] LoadFile(string fileName)
		{
			return BinaryFileHelper.LoadFile(GetPath(fileName));
		}

		public static bool FileExist(string fileName)
		{
			return File.Exists(GetPath(fileName));
		}

		private static string GetPath(string fileName)
		{
			return Application.persistentDataPath + "/" + fileName;
		}
	}
}
