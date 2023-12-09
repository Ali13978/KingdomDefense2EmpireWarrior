using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class NewEnemyInformationUIManager : MonoBehaviour
	{
		[Space]
		[SerializeField]
		private Transform cardsRoot;

		[Space]
		private List<NewEnemyInformationButton> buttons = new List<NewEnemyInformationButton>();

		[Space]
		[SerializeField]
		private float buttonLifeTime = 60f;

		private static NewEnemyInformationUIManager instance;

		public static NewEnemyInformationUIManager Instance
		{
			get
			{
				return instance;
			}
			private set
			{
				instance = value;
			}
		}

		public void Awake()
		{
			Instance = this;
		}

		private void Start()
		{
			InitializeButtons();
		}

		public void TryActivateButton(int enemyId)
		{
			foreach (NewEnemyInformationButton button in buttons)
			{
				if (button.EnemyId == enemyId)
				{
					button.ShowButton(buttonLifeTime);
				}
			}
		}

		private void InitializeButtons()
		{
			List<int> listEnemyID = SingletonMonoBehaviour<GameData>.Instance.ListEnemyID;
			string empty = string.Empty;
			for (int i = 0; i < listEnemyID.Count; i++)
			{
				empty = $"NewEnemy/ButtonNewEnemy";
				NewEnemyInformationButton newEnemyInformationButton = UnityEngine.Object.Instantiate(Resources.Load<NewEnemyInformationButton>(empty));
				newEnemyInformationButton.Init(listEnemyID[i]);
				newEnemyInformationButton.gameObject.SetActive(value: false);
				newEnemyInformationButton.transform.SetParent(cardsRoot);
				newEnemyInformationButton.transform.localScale = Vector3.one;
				buttons.Add(newEnemyInformationButton);
			}
		}
	}
}
