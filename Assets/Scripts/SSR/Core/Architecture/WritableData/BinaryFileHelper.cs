using System.IO;

namespace SSR.Core.Architecture.WritableData
{
	public static class BinaryFileHelper
	{
		public static void SaveFile(byte[] bytes, string path)
		{
			FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			fileStream.Write(bytes, 0, bytes.Length);
			fileStream.Flush();
			fileStream.Close();
		}

		public static byte[] LoadFile(string path)
		{
			FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			byte[] array = new byte[fileStream.Length];
			fileStream.Read(array, 0, array.Length);
			fileStream.Close();
			return array;
		}
	}
}
