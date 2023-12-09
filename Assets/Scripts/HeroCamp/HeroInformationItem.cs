using MyCustom;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class HeroInformationItem : CustomMonoBehaviour
	{
		[SerializeField]
		private Text currentLevelValue;

		[SerializeField]
		private Text nextLevelValue;

		private int heroMaxLevel = 9;

		public Text CurrentLevelValue
		{
			get
			{
				return currentLevelValue;
			}
			set
			{
				currentLevelValue = value;
			}
		}

		public Text NextLevelValue
		{
			get
			{
				return nextLevelValue;
			}
			set
			{
				nextLevelValue = value;
			}
		}

		public int HeroMaxLevel
		{
			get
			{
				return heroMaxLevel;
			}
			set
			{
				heroMaxLevel = value;
			}
		}

		public virtual void Init(int heroID, int heroLevel)
		{
		}
	}
}
