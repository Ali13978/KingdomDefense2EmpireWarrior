using System.Collections.Generic;

namespace Parameter
{
	public class EnemyDescription : Singleton<EnemyDescription>
	{
		public List<List<EnemyDes>> listEnemy = new List<List<EnemyDes>>();

		public void ClearData()
		{
			listEnemy.Clear();
		}

		public void SetEnemyDescription(EnemyDes enemy)
		{
			int count = listEnemy.Count;
			if (count <= enemy.id)
			{
				List<EnemyDes> list = new List<EnemyDes>();
				list.Insert(enemy.level, enemy);
				listEnemy.Insert(enemy.id, list);
			}
			else
			{
				List<EnemyDes> list2 = listEnemy[enemy.id];
				list2.Insert(enemy.level, enemy);
			}
		}

		public EnemyDes GetEnemyParameter(int id, int level)
		{
			if (CheckParameter(id))
			{
				return listEnemy[id][level];
			}
			return default(EnemyDes);
		}

		public int GetNumberOfEnemy()
		{
			return listEnemy.Count;
		}

		public string GetEnemyName(int enemyID)
		{
			if (CheckParameter(enemyID))
			{
				EnemyDes enemyDes = listEnemy[enemyID][0];
				return enemyDes.name;
			}
			return "_";
		}

		public string GetEnemyDescription(int enemyID)
		{
			if (CheckParameter(enemyID))
			{
				EnemyDes enemyDes = listEnemy[enemyID][0];
				return enemyDes.decription;
			}
			return "_";
		}

		public string GetEnemySpecialAbility(int enemyID)
		{
			if (CheckParameter(enemyID))
			{
				EnemyDes enemyDes = listEnemy[enemyID][0];
				return enemyDes.specialAbility;
			}
			return "_";
		}

		private bool CheckParameter(int id)
		{
			if (id >= GetNumberOfEnemy())
			{
				return false;
			}
			return true;
		}
	}
}
