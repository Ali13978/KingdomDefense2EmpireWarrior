using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	public class ReadWriteDataHeroPrepare : MonoBehaviour
	{
		private static string DB_NAME = "/heroPrepareInfor.dat";

		private HeroPrepareSerializeData data = new HeroPrepareSerializeData();

		private static ReadWriteDataHeroPrepare instance;

		public static ReadWriteDataHeroPrepare Instance => instance;

		private void Awake()
		{
			instance = this;
			SaveDefaultData();
			Load();
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				data.listHeroIDSaved = new int[3]
				{
					-1,
					-1,
					-1
				};
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		public void Save(int[] listHeroID)
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			for (int i = 0; i < data.listHeroIDSaved.Length; i++)
			{
				data.listHeroIDSaved[i] = listHeroID[i];
			}
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		private void Load()
		{
			FileStream fileStream = File.Open(Application.persistentDataPath + DB_NAME, FileMode.Open);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			data = (HeroPrepareSerializeData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
		}

		public int[] GetListHeroIDSaved()
		{
			Load();
			return data.listHeroIDSaved;
		}
	}
}
