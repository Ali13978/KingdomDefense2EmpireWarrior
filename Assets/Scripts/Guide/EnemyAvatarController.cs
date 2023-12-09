using UnityEngine;

namespace Guide
{
	public class EnemyAvatarController : MonoBehaviour
	{
		public int enemyID;

		public void Init(int enemyID)
		{
			this.enemyID = enemyID;
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}

		public void Display()
		{
			base.gameObject.SetActive(value: true);
		}
	}
}
