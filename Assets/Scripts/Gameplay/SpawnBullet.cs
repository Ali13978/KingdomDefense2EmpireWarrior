using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class SpawnBullet : SingletonMonoBehaviour<SpawnBullet>
	{
		private Vector3 PoolPosition = new Vector3(1000f, 100f, 0f);

		public void InitBulletsFromTower(int towerID, int towerLevel)
		{
			string arg = $"bullet_{towerID}_{towerLevel}";
			BulletModel bulletModel = null;
			bulletModel = Object.Instantiate(Resources.Load<BulletModel>($"Bullets/{arg}"));
			bulletModel.gameObject.SetActive(value: false);
			TrashManRecycleBin trashManRecycleBin = new TrashManRecycleBin();
			trashManRecycleBin.prefab = bulletModel.gameObject;
			trashManRecycleBin.instancesToPreallocate = 0;
			TrashManRecycleBin recycleBin = trashManRecycleBin;
			TrashMan.manageRecycleBin(recycleBin);
			TrashMan.despawn(bulletModel.gameObject);
		}

		public void InitBulletsFromHeroes(int heroID, int bulletIndex)
		{
			string arg = $"hero_{heroID}_bullet_{bulletIndex}";
			BulletModel bulletModel = null;
			bulletModel = Object.Instantiate(Resources.Load<BulletModel>($"Bullets/{arg}"));
			bulletModel.gameObject.SetActive(value: false);
			TrashManRecycleBin trashManRecycleBin = new TrashManRecycleBin();
			trashManRecycleBin.prefab = bulletModel.gameObject;
			trashManRecycleBin.instancesToPreallocate = 0;
			TrashManRecycleBin recycleBin = trashManRecycleBin;
			TrashMan.manageRecycleBin(recycleBin);
			TrashMan.despawn(bulletModel.gameObject);
		}

		public void InitExtendBullet(GameObject bullet)
		{
			GameObject gameObject = null;
			gameObject = Object.Instantiate(bullet);
			gameObject.gameObject.SetActive(value: false);
			TrashManRecycleBin trashManRecycleBin = new TrashManRecycleBin();
			trashManRecycleBin.prefab = gameObject.gameObject;
			trashManRecycleBin.instancesToPreallocate = 0;
			TrashManRecycleBin recycleBin = trashManRecycleBin;
			TrashMan.manageRecycleBin(recycleBin);
			TrashMan.despawn(gameObject);
		}

		public BulletModel GetForTower(int towerID, int towerLevel)
		{
			if (towerID < 0 || towerLevel > TowerParameter.Instance.GetNumberOfLevel())
			{
				return null;
			}
			BulletModel bulletModel = null;
			string gameObjectName = $"bullet_{towerID}_{towerLevel}(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			bulletModel = gameObject.GetComponent<BulletModel>();
			bulletModel.transform.parent = base.transform;
			return bulletModel;
		}

		public BulletModel GetForHero(int heroID, int bulletIndex)
		{
			if (heroID < 0)
			{
				return null;
			}
			BulletModel bulletModel = null;
			string gameObjectName = $"hero_{heroID}_bullet_{bulletIndex}(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			bulletModel = gameObject.GetComponent<BulletModel>();
			bulletModel.transform.parent = base.transform;
			return bulletModel;
		}

		public BulletModel GetBulletByName(string name)
		{
			BulletModel bulletModel = null;
			string gameObjectName = $"{name}(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			bulletModel = gameObject.GetComponent<BulletModel>();
			bulletModel.transform.parent = base.transform;
			return bulletModel;
		}

		public EnemyBulletController GetEnemyBulletByName(string name)
		{
			EnemyBulletController enemyBulletController = null;
			string gameObjectName = $"{name}(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			enemyBulletController = gameObject.GetComponent<EnemyBulletController>();
			enemyBulletController.transform.parent = base.transform;
			return enemyBulletController;
		}

		public void Push(BulletModel bullet)
		{
			bullet.transform.position = PoolPosition;
			bullet.gameObject.SetActive(value: false);
			TrashMan.despawn(bullet.gameObject);
		}

		public void Push(GameObject bullet)
		{
			bullet.transform.position = PoolPosition;
			bullet.SetActive(value: false);
			TrashMan.despawn(bullet);
		}

		public HeroSkillAOECommon GetHeroSkillAOECommon(string name)
		{
			HeroSkillAOECommon heroSkillAOECommon = null;
			string gameObjectName = name + "(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			heroSkillAOECommon = gameObject.GetComponent<HeroSkillAOECommon>();
			heroSkillAOECommon.transform.parent = base.transform;
			return heroSkillAOECommon;
		}

		public Hero1Skill0Projectile GetLightningBullet()
		{
			Hero1Skill0Projectile hero1Skill0Projectile = null;
			string gameObjectName = "LightningProjectile(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			hero1Skill0Projectile = gameObject.GetComponent<Hero1Skill0Projectile>();
			hero1Skill0Projectile.transform.parent = base.transform;
			return hero1Skill0Projectile;
		}

		public Hero3Skill0Meteor GetHero3Skill0Meteor()
		{
			Hero3Skill0Meteor hero3Skill0Meteor = null;
			string gameObjectName = "Hero3Skill0Meteor(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			hero3Skill0Meteor = gameObject.GetComponent<Hero3Skill0Meteor>();
			hero3Skill0Meteor.transform.parent = base.transform;
			return hero3Skill0Meteor;
		}

		public Hero3Skill2IceTrap GetHero3Skill2IceTrap()
		{
			Hero3Skill2IceTrap hero3Skill2IceTrap = null;
			string gameObjectName = "Hero3Skill2IceTrap(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			hero3Skill2IceTrap = gameObject.GetComponent<Hero3Skill2IceTrap>();
			hero3Skill2IceTrap.transform.parent = base.transform;
			return hero3Skill2IceTrap;
		}

		public Hero3Skill3SunStrike GetHero3Skill3SunStrike()
		{
			Hero3Skill3SunStrike hero3Skill3SunStrike = null;
			string gameObjectName = "Hero3Skill3SunStrike(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			hero3Skill3SunStrike = gameObject.GetComponent<Hero3Skill3SunStrike>();
			hero3Skill3SunStrike.transform.parent = base.transform;
			return hero3Skill3SunStrike;
		}

		public Hero4Skill0Breakdown GetHero4Skill0Breakdown()
		{
			Hero4Skill0Breakdown hero4Skill0Breakdown = null;
			string gameObjectName = "Hero4Skill0Breakdown(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			hero4Skill0Breakdown = gameObject.GetComponent<Hero4Skill0Breakdown>();
			hero4Skill0Breakdown.transform.parent = base.transform;
			return hero4Skill0Breakdown;
		}

		public Hero5Skill0HealingBomb GetHero5Skill0HealingBomb()
		{
			Hero5Skill0HealingBomb hero5Skill0HealingBomb = null;
			string gameObjectName = "Hero5Skill0HealingBomb(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			hero5Skill0HealingBomb = gameObject.GetComponent<Hero5Skill0HealingBomb>();
			hero5Skill0HealingBomb.transform.parent = base.transform;
			return hero5Skill0HealingBomb;
		}

		public Hero5Skill3LightningSpear GetHero5Skill3LightningSpear()
		{
			Hero5Skill3LightningSpear hero5Skill3LightningSpear = null;
			string gameObjectName = "Hero5Skill3LightningSpear(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			hero5Skill3LightningSpear = gameObject.GetComponent<Hero5Skill3LightningSpear>();
			hero5Skill3LightningSpear.transform.parent = base.transform;
			return hero5Skill3LightningSpear;
		}

		public MeteorController GetMeteorController()
		{
			MeteorController meteorController = null;
			string gameObjectName = "Meteor(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			meteorController = gameObject.GetComponent<MeteorController>();
			meteorController.transform.parent = base.transform;
			return meteorController;
		}

		public Tower0Ultimate0Bullet GetTower0Ultimate0Bullet()
		{
			Tower0Ultimate0Bullet tower0Ultimate0Bullet = null;
			string gameObjectName = "Tower0Ultimate0Bullet(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			tower0Ultimate0Bullet = gameObject.GetComponent<Tower0Ultimate0Bullet>();
			tower0Ultimate0Bullet.transform.parent = base.transform;
			return tower0Ultimate0Bullet;
		}
	}
}
