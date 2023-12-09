using Gameplay;
using System.Collections.Generic;

namespace Parameter
{
	public class EnemyParameterManager
	{
		public List<List<Enemy>> listEnemy = new List<List<Enemy>>();

		private static EnemyParameterManager instance;

		public static EnemyParameterManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new EnemyParameterManager();
				}
				return instance;
			}
		}

		public void SetEnemyParameter(Enemy enemy)
		{
			int count = listEnemy.Count;
			if (count <= enemy.id)
			{
				List<Enemy> list = new List<Enemy>();
				list.Insert(enemy.level, enemy);
				listEnemy.Insert(enemy.id, list);
			}
			else
			{
				List<Enemy> list2 = listEnemy[enemy.id];
				list2.Insert(enemy.level, enemy);
			}
		}

		public List<int> GetAllEnemyIds()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < listEnemy.Count; i++)
			{
				List<int> list2 = list;
				Enemy enemy = listEnemy[i][0];
				list2.Add(enemy.id);
			}
			return list;
		}

		public Enemy GetEnemyParameter(int id, int level)
		{
			if (CheckParameter(id))
			{
				return listEnemy[id][level];
			}
			return default(Enemy);
		}

		public Enemy GetEnemyParameterForEndlessMode(int id, int level, int loopAmount)
		{
			Enemy enemyParameter = GetEnemyParameter(id, level);
			float healthIncreasePercentage = GameplayManager.Instance.endlessModeManager.healthIncreasePercentage;
			float damageIncreasePercentage = GameplayManager.Instance.endlessModeManager.damageIncreasePercentage;
			enemyParameter.health += (int)((float)loopAmount * (healthIncreasePercentage / 100f) * (float)enemyParameter.health);
			enemyParameter.attack_physics_min += (int)((float)loopAmount * (healthIncreasePercentage / 100f) * (float)enemyParameter.attack_physics_min);
			enemyParameter.attack_physics_max += (int)((float)loopAmount * (healthIncreasePercentage / 100f) * (float)enemyParameter.attack_physics_max);
			enemyParameter.attack_magic_min += (int)((float)loopAmount * (healthIncreasePercentage / 100f) * (float)enemyParameter.attack_magic_min);
			enemyParameter.attack_magic_max += (int)((float)loopAmount * (healthIncreasePercentage / 100f) * (float)enemyParameter.attack_magic_max);
			return enemyParameter;
		}

		public int GetHealth(int enemyID, int level)
		{
			if (CheckParameter(enemyID))
			{
				Enemy enemy = listEnemy[enemyID][level];
				return enemy.health;
			}
			return -1;
		}

		public int GetMinDamage(int enemyID, int level)
		{
			int num = 0;
			if (CheckParameter(enemyID))
			{
				Enemy enemy = listEnemy[enemyID][level];
				if (enemy.attack_physics_min == 0)
				{
					Enemy enemy2 = listEnemy[enemyID][level];
					return enemy2.attack_magic_min;
				}
				Enemy enemy3 = listEnemy[enemyID][level];
				return enemy3.attack_physics_min;
			}
			return -1;
		}

		public int GetMaxDamage(int enemyID, int level)
		{
			int num = 0;
			if (CheckParameter(enemyID))
			{
				Enemy enemy = listEnemy[enemyID][level];
				if (enemy.attack_physics_max == 0)
				{
					Enemy enemy2 = listEnemy[enemyID][level];
					return enemy2.attack_magic_max;
				}
				Enemy enemy3 = listEnemy[enemyID][level];
				return enemy3.attack_physics_max;
			}
			return -1;
		}

		public bool isPhysicsAttack(int enemyID)
		{
			Enemy enemy = listEnemy[enemyID][0];
			return enemy.attack_physics_max > 0;
		}

		public int GetPhysicsArmor(int enemyID, int level)
		{
			if (CheckParameter(enemyID))
			{
				Enemy enemy = listEnemy[enemyID][level];
				return enemy.armor_physics;
			}
			return -1;
		}

		public int GetMagicArmor(int enemyID, int level)
		{
			if (CheckParameter(enemyID))
			{
				Enemy enemy = listEnemy[enemyID][level];
				return enemy.armor_magic;
			}
			return -1;
		}

		public int GetSpeed(int enemyID, int level)
		{
			if (CheckParameter(enemyID))
			{
				Enemy enemy = listEnemy[enemyID][level];
				return enemy.speed;
			}
			return -1;
		}

		public int GetLifeTaken(int enemyID, int level)
		{
			if (CheckParameter(enemyID))
			{
				Enemy enemy = listEnemy[enemyID][level];
				return enemy.lifeTaken;
			}
			return -1;
		}

		public int GetValue(int enemyID)
		{
			if (CheckParameter(enemyID))
			{
				Enemy enemy = listEnemy[enemyID][0];
				return enemy.value;
			}
			return -1;
		}

		public bool IsBoss(int enemyID)
		{
			if (CheckParameter(enemyID))
			{
				Enemy enemy = listEnemy[enemyID][0];
				return enemy.isBoss;
			}
			return false;
		}

		public bool IsWaveHaveBoss(List<int> listEnemyID)
		{
			bool result = false;
			foreach (int item in listEnemyID)
			{
				if (IsBoss(item))
				{
					result = true;
				}
			}
			return result;
		}

		public int GetNumberOfEnemy()
		{
			return listEnemy.Count;
		}

		public bool IsEnemyHasMoreThanOneLife(int enemyID)
		{
			bool result = false;
			if (enemyID == 10 || enemyID == 12 || enemyID == 24)
			{
				result = true;
			}
			return result;
		}

		private bool CheckParameter(int id)
		{
			if (id >= GetNumberOfEnemy())
			{
				return false;
			}
			return true;
		}

		public bool IsFlyEnemyInGate(int wave, int gate)
		{
			bool result = false;
			List<EnemyData> listEnemyWithWave = SingletonMonoBehaviour<GameData>.Instance.GetListEnemyWithWave(wave);
			foreach (EnemyData item in listEnemyWithWave)
			{
				EnemyData current = item;
				if (current.gate == gate)
				{
					int id = current.id;
					Enemy enemyParameter = Instance.GetEnemyParameter(id, 0);
					if (enemyParameter.isAir)
					{
						result = true;
					}
				}
			}
			return result;
		}

		public List<int> getListEnemyGate(int wave)
		{
			List<int> list = new List<int>();
			if (wave >= SingletonMonoBehaviour<GameData>.Instance.TotalWave)
			{
				return null;
			}
			List<EnemyData> listEnemyWithWave = SingletonMonoBehaviour<GameData>.Instance.GetListEnemyWithWave(wave);
			foreach (EnemyData item in listEnemyWithWave)
			{
				EnemyData current = item;
				if (!list.Contains(current.gate))
				{
					list.Add(current.gate);
				}
			}
			return list;
		}
	}
}
