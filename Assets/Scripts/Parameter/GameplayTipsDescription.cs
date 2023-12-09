using System.Collections.Generic;

namespace Parameter
{
	public class GameplayTipsDescription : Singleton<GameplayTipsDescription>
	{
		private List<GameplayTip> listTip = new List<GameplayTip>();

		private bool CheckId(int tipID)
		{
			return tipID >= 0 && tipID < listTip.Count;
		}

		public void ClearData()
		{
			listTip.Clear();
		}

		public void SetGameplayTipParameter(GameplayTip tip)
		{
			int count = listTip.Count;
			if (count <= tip.id)
			{
				listTip.Add(tip);
			}
		}

		public string GetName(int tipID)
		{
			if (tipID < listTip.Count && tipID >= 0)
			{
				GameplayTip gameplayTip = listTip[tipID];
				return gameplayTip.name;
			}
			return "--";
		}

		public string GetDescription(int tipID)
		{
			if (tipID < listTip.Count && tipID >= 0)
			{
				GameplayTip gameplayTip = listTip[tipID];
				return gameplayTip.description;
			}
			return "--";
		}
	}
}
