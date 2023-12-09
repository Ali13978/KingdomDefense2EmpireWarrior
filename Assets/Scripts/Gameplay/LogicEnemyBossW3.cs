using Middle;
using UnityEngine;

namespace Gameplay
{
	public class LogicEnemyBossW3 : EnemyController
	{
		public static string animRunRight = "Run-Right";

		public static string animRunUp = "Run-Up";

		public static string animBirdRunRight = "Bird-Run-Right";

		public static string animAttackRange = "RangeAttack";

		public static string animSpecialAttack = "SpecialAttack";

		public static string animDie = "Die";

		public static string animIdle = "Idle";

		public static string animTurnToBird = "TurnToBird";

		public static string animTurnToBoss = "TurnToBoss";

		public Animator animator;

		public GameObject behitTurnAllyPrefab;

		public GameObject monsterTransformationPrefab;

		public GameObject summonEffectPrefab;

		public float teleSpeed;

		[HideInInspector]
		public BossW3BaseState curState;

		[HideInInspector]
		public int summonRoutineCountdown;

		public override void Initialize()
		{
			base.Initialize();
			summonRoutineCountdown = 10;
			TrashMan.InitPool("Enemies/enemy_18");
			TrashMan.InitPool("Enemies/enemy_20");
			TrashMan.InitPool("Enemies/enemy_21");
		}

		public override void OnAppear()
		{
			base.OnAppear();
			curState = new BossW3_Spawn_P0(this);
		}

		public override void Update()
		{
			base.Update();
			if (GameTools.IsValidEnemy(base.EnemyModel))
			{
				curState.OnUpdate(Time.deltaTime);
			}
		}

		public MonsterPathAnchor GetRandomPosOnRoad(float minLineProp, float maxLineProp)
		{
			int gate = Random.Range(0, LineManager.Current.listGates.Count);
			int line = Random.Range(0, Config.Instance.LineCount);
			LineData line2 = LineManager.Current.GetLine(gate, line);
			float num = Random.Range(minLineProp, maxLineProp);
			float num2 = line2.Length * num;
			int pathSegmentId = 0;
			for (int i = 0; i < line2.segmentLengths.Length; i++)
			{
				if (line2.segmentLengths[i] >= num2)
				{
					pathSegmentId = i;
					break;
				}
				num2 -= line2.segmentLengths[i];
			}
			return new MonsterPathAnchor(LineManager.Current.GetLineIndex(gate, line), pathSegmentId, num2);
		}
	}
}
