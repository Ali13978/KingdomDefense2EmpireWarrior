using System.Collections.Generic;

namespace Parameter
{
	public class TutorialDescription : Singleton<TutorialDescription>
	{
		private Dictionary<string, string> listTutorialDescription = new Dictionary<string, string>();

		public void ClearData()
		{
			listTutorialDescription.Clear();
		}

		public void SetTutParameter(TutorialDes tut)
		{
			if (!listTutorialDescription.ContainsKey(tut.id))
			{
				listTutorialDescription.Add(tut.id, tut.description);
			}
		}

		public string GetDescription(string tutID)
		{
			string value = string.Empty;
			if (!listTutorialDescription.ContainsKey(tutID) || listTutorialDescription.TryGetValue(tutID, out value))
			{
			}
			return value;
		}
	}
}
