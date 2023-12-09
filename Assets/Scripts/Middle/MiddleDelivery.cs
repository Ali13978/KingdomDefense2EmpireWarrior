using System.Collections.Generic;

namespace Middle
{
	public class MiddleDelivery
	{
		private static MiddleDelivery instance;

		private int openSceneCount;

		private int mapIDSelected;

		private List<int> listHeroesIdsSelected = new List<int>();

		public static MiddleDelivery Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MiddleDelivery();
				}
				return instance;
			}
		}

		public int OpenSceneCount
		{
			get
			{
				return openSceneCount;
			}
			private set
			{
				openSceneCount = value;
			}
		}

		public int MapIDSelected
		{
			get
			{
				return mapIDSelected;
			}
			set
			{
				mapIDSelected = value;
			}
		}

		public BattleLevel BattleLevel
		{
			get;
			set;
		}

		public List<int> ListHeroesIdsSelected
		{
			get
			{
				return listHeroesIdsSelected;
			}
			private set
			{
				listHeroesIdsSelected = value;
			}
		}

		public void IncreaseOpenSceneCount()
		{
			OpenSceneCount++;
		}

		public void AddHeroIDToList(int heroID)
		{
			ListHeroesIdsSelected.Add(heroID);
		}

		public void ClearListHeroID()
		{
			ListHeroesIdsSelected.Clear();
		}
	}
}
