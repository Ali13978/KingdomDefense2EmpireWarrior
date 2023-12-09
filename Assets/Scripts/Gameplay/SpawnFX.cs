using UnityEngine;

namespace Gameplay
{
	public class SpawnFX : SingletonMonoBehaviour<SpawnFX>
	{
		[Space]
		[Header("Start Wave Button n Bonus Money")]
		[SerializeField]
		private BonusMoneyAnimController bonusMoneyAnimControllerPrefab;

		[Space]
		[Header("Gem n Gold")]
		[SerializeField]
		private GemController gemControllerPrefab;

		[SerializeField]
		private DroppedGoldController goldControllerPrefab;

		private Vector3 PoolPosition = new Vector3(1000f, 100f, 0f);

		public static string EFFECT_MISS = "Miss";

		public static string EFFECT_CRITICAL = "Critical";

		public static string EFFECT_SLOW = "Slow";

		public static string EFFECT_STUN = "Stun";

		public static string EFFECT_POISON0 = "Poison0";

		public static string EFFECT_POISON1 = "Poison1";

		public static string EFFECT_BURNING = "Burning";

		public static string EFFECT_ROOT = "Root";

		public static string EFFECT_ELECTRIC = "Electric";

		public static string EFFECT_THUNDER = "Thunder";

		public static string EFFECT_BLEED = "Bleed";

		public static string EFFECT_BLOOD_SPREAD = "BloodSpread";

		public static string EFFECT_DEF_DOWN = "DefDown";

		private static string EXPLOSION_CANNONBALL = "Explosion-CannonBall";

		private static string BLOODY_ARROW = "BloodyArrow";

		private static string FIREBALL = "FireBall";

		public static string ARROW_ON_THE_GROUND = "ArrowOnTheGround";

		public static string GROUND_BREAK = "GroundBreak";

		public static string EFFECT_BUILD_TOWER = "BuildTower";

		private static string EFFECT_PRE_BUILD_TOWER = "PreBuildTower";

		public static string EFFECT_SELL_TOWER_ON_ALLY = "TowerSellOnAlly";

		public static string EFFECT_UPGRADE_TOWER_ON_ALLY = "TowerUpgradeOnAlly";

		public static string ICON_MOVEABLE_AllY = "Moveable_Ally";

		public static string ICON_MOVEABLE_HERO = "Moveable_Hero";

		public static string ICON_UNMOVEABLE = "UnMoveable";

		private static string DROPPED_GEM_NAME = "GemDropped";

		private static string DROPPED_GOLD_NAME = "GoldDropped";

		public static string EFFECT_ITEM_FREEZE = "Item-Freeze";

		public static string LIGHTNING_PROJECTILE_RANGE = "LightningProjectileRange";

		public static string LIGHTNING_PROJECTILE = "LightningProjectile";

		public static string LIGHTNING_EXPLOSION = "LightningExplosion";

		public static string LIGHTNING_PROJECTILE_SHADOW = "LightningProjectileShadow";

		public static string GROUND_STOMP = "GroundStomp";

		public static string EFFECT_FADE_SCREEN = "FadeScreen";

		public static string METEOR_EXPLOSION2 = "MeteorExplosion2";

		public static string METEOR_SELF_EXPLOSION = "MeteorSelfExplosion";

		public static string INFERNO_GOLEM_AURA = "InfernoGolemAura";

		public static string ATTACK_UP_FX = "AttackUpFx";

		public static string LIGHTNING_EXPLOSION_2 = "LightningExplosion2";

		public static string THOR_MASSIVE_THUNDER = "MassiveThunder";

		public static string THOR_LANDING_THUNDER = "LandingThunder";

		public static string THOR_AIR_THUNDER = "AirThunder";

		public static string TIMER_BOMB_EXPLOSION = "TimerBombExplosion";

		public static string GROUND_AIMING_1 = "GroundAiming1";

		public static string GOLD_CHEST = "GoldChest";

		public static string FLYING_COIN = "FlyingCoin";

		public static string METEOR_EXPLOSION = "MeteorExplosion";

		public static string HEALING_WAND = "HealingWand";

		public static string BUFF_SPEED_AURA = "BuffSpeedAura";

		public static string BUFF_SPEED_ON_TOWER = "BuffSpeedOnTower";

		public static string POISON_AREA = "PoisonArea";

		public static string STORM = "Storm";

		public static string EFFECT_HEAL_0 = "HealFX0";

		public static string EFFECT_HEAL_1 = "HealFX1";

		public static string EFFECT_HEAL_2 = "HealFX2";

		private void Awake()
		{
			InitGemnGoldDropped();
		}

		public void InitExtendObject(GameObject sourceObject, int alloCateNumber)
		{
			GameObject gameObject = null;
			gameObject = Object.Instantiate(sourceObject);
			gameObject.gameObject.SetActive(value: false);
			TrashManRecycleBin trashManRecycleBin = new TrashManRecycleBin();
			trashManRecycleBin.prefab = gameObject.gameObject;
			trashManRecycleBin.instancesToPreallocate = alloCateNumber;
			TrashManRecycleBin recycleBin = trashManRecycleBin;
			TrashMan.manageRecycleBin(recycleBin);
			TrashMan.despawn(gameObject.gameObject);
		}

		private void InitGemnGoldDropped()
		{
			GemController gemController = null;
			gemController = Object.Instantiate(gemControllerPrefab);
			gemController.gameObject.SetActive(value: false);
			TrashManRecycleBin trashManRecycleBin = new TrashManRecycleBin();
			trashManRecycleBin.prefab = gemController.gameObject;
			trashManRecycleBin.instancesToPreallocate = 0;
			TrashManRecycleBin recycleBin = trashManRecycleBin;
			TrashMan.manageRecycleBin(recycleBin);
			TrashMan.despawn(gemController.gameObject);
			DroppedGoldController droppedGoldController = null;
			droppedGoldController = Object.Instantiate(goldControllerPrefab);
			droppedGoldController.gameObject.SetActive(value: false);
			trashManRecycleBin = new TrashManRecycleBin();
			trashManRecycleBin.prefab = droppedGoldController.gameObject;
			trashManRecycleBin.instancesToPreallocate = 0;
			TrashManRecycleBin recycleBin2 = trashManRecycleBin;
			TrashMan.manageRecycleBin(recycleBin2);
			TrashMan.despawn(droppedGoldController.gameObject);
		}

		public void InitFX(string effectName)
		{
			EffectController effectController = null;
			effectController = Object.Instantiate(Resources.Load<EffectController>($"FXs/{effectName}"));
			effectController.gameObject.SetActive(value: false);
			TrashManRecycleBin trashManRecycleBin = new TrashManRecycleBin();
			trashManRecycleBin.prefab = effectController.gameObject;
			trashManRecycleBin.instancesToPreallocate = 0;
			TrashManRecycleBin recycleBin = trashManRecycleBin;
			TrashMan.manageRecycleBin(recycleBin);
			TrashMan.despawn(effectController.gameObject);
		}

		public void InitFX(string effectName, int numberOfAllocate)
		{
			EffectController effectController = null;
			effectController = Object.Instantiate(Resources.Load<EffectController>($"FXs/{effectName}"));
			effectController.gameObject.SetActive(value: false);
			TrashManRecycleBin trashManRecycleBin = new TrashManRecycleBin();
			trashManRecycleBin.prefab = effectController.gameObject;
			trashManRecycleBin.instancesToPreallocate = numberOfAllocate;
			TrashManRecycleBin recycleBin = trashManRecycleBin;
			TrashMan.manageRecycleBin(recycleBin);
			TrashMan.despawn(effectController.gameObject);
		}

		public void Push(EffectController _effectcontroller)
		{
			_effectcontroller.transform.position = PoolPosition;
			_effectcontroller.gameObject.SetActive(value: false);
			TrashMan.despawn(_effectcontroller.gameObject);
		}

		public void Push(GameObject _gameObject)
		{
			_gameObject.transform.position = PoolPosition;
			_gameObject.SetActive(value: false);
			TrashMan.despawn(_gameObject);
		}

		public EffectController GetExplosion(string explosionName)
		{
			EffectController effectController = null;
			GameObject gameObject = TrashMan.spawn($"{explosionName}(Clone)");
			effectController = gameObject.GetComponent<EffectController>();
			effectController.gameObject.SetActive(value: false);
			effectController.transform.parent = base.transform;
			effectController.transform.localPosition = Vector3.zero;
			return effectController;
		}

		public EffectController GetEffect(string effectName)
		{
			EffectController effectController = null;
			GameObject gameObject = TrashMan.spawn($"{effectName}(Clone)");
			effectController = gameObject.GetComponent<EffectController>();
			effectController.gameObject.SetActive(value: false);
			effectController.transform.parent = base.transform;
			effectController.transform.localPosition = Vector3.zero;
			return effectController;
		}

		public FrostEffect GetFrostEffectOnCamera()
		{
			return Camera.main.GetComponent<FrostEffect>();
		}

		public GameObject GetObjectByName(string name)
		{
			GameObject gameObject = null;
			gameObject = TrashMan.spawn(name + "(Clone)");
			gameObject.SetActive(value: false);
			return gameObject;
		}

		public GemController GetDroppedGem()
		{
			GemController gemController = null;
			GameObject gameObject = TrashMan.spawn(DROPPED_GEM_NAME + "(Clone)");
			gemController = gameObject.GetComponent<GemController>();
			gemController.gameObject.SetActive(value: false);
			gemController.transform.parent = base.transform;
			return gemController;
		}

		public DroppedGoldController GetDroppedGold()
		{
			DroppedGoldController droppedGoldController = null;
			GameObject gameObject = TrashMan.spawn(DROPPED_GOLD_NAME + "(Clone)");
			droppedGoldController = gameObject.GetComponent<DroppedGoldController>();
			droppedGoldController.gameObject.SetActive(value: false);
			droppedGoldController.transform.parent = base.transform;
			return droppedGoldController;
		}
	}
}
