using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SSR.Core.Architecture.WritableData
{
	public static class BinarySerializationHelper
	{
		public static T Deserialize<T>(byte[] bytes)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream serializationStream = new MemoryStream(bytes);
			return (T)binaryFormatter.Deserialize(serializationStream);
		}

		public static byte[] Serialize<T>(T dataObject)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			MemoryStream memoryStream = new MemoryStream();
			binaryFormatter.Serialize(memoryStream, dataObject);
			return memoryStream.ToArray();
		}

		public static T Clone<T>(T originalObject)
		{
			return Deserialize<T>(Serialize(originalObject));
		}
	}
}
