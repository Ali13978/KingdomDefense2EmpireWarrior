using DG.Tweening;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
	public class TowerSpawnAllyController : TowerController
	{
		public UnityEvent OnSpawnAllies;

		private List<string> silentBuffKeys = new List<string>
		{
			"Silent"
		};

		[SerializeField]
		private int desiredAlliesNumber;

		[SerializeField]
		private Vector3 offsetSpawnPosition;

		[SerializeField]
		private Transform spawnPosition;

		public GameObject door;

		[Header("Movement parameter")]
		[SerializeField]
		private LayerMask avaiableMovingLayerMask;

		private Transform readyPosition;

		private Transform targetPosition;

		private float timeTracking;

		private bool startChecking;

		private int ignoreReloadChance;

		private AllyModel[] listAllyControl = new AllyModel[3];

		[NonSerialized]
		public Vector2 currentTargetPosition = new Vector2(0f, 0f);

		private StartPositionParameter[] startPositionParameters = new StartPositionParameter[3];

		private Tower originalParameter;

		private EffectCaster effectCaster;

		public AllyModel[] ListAllyControl
		{
			get
			{
				return listAllyControl;
			}
			set
			{
				listAllyControl = value;
			}
		}

		public override void Initialize()
		{
			base.Initialize();
			SingletonMonoBehaviour<SpawnAlly>.Instance.PushAlliesToPool(base.TowerModel.Id, base.TowerModel.Level, 2);
			Tower tower = base.TowerModel.OriginalParameter;
			timeTracking = (float)tower.reload / 1000f;
			Tower tower2 = base.TowerModel.OriginalParameter;
			ignoreReloadChance = tower2.ignoreReloadChance;
			base.TowerModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			base.TowerModel.IsSilent = (base.TowerModel.BuffsHolder.GetBuffsValue(silentBuffKeys) > 0f);
			if (base.TowerModel.IsSilent)
			{
				ApplyBuffSilent();
			}
		}

		public override void OnAppear()
		{
			base.OnAppear();
			Array.Clear(ListAllyControl, 0, ListAllyControl.Length);
			startChecking = false;
			CustomInvoke(setStartChecking, 1f);
			readyPosition = SingletonMonoBehaviour<BuildRegionManager>.Instance.listRegions[base.TowerModel.RegionID].spawnAllyPosition;
			if (base.TowerModel.Level == 0)
			{
				CalculateReadyPosition(readyPosition.position);
			}
			else if (currentTargetPosition.Equals(Vector2.zero))
			{
				UnityEngine.Debug.Log(" vị trí chưa đc chỉ định");
				CalculateReadyPosition(readyPosition.position);
			}
			else
			{
				UnityEngine.Debug.Log(" vị trí đã đc chỉ định");
				CalculateReadyPosition(currentTargetPosition);
			}
			CustomInvoke(SpawnAllAllies, 0.2f);
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			Array.Clear(ListAllyControl, 0, ListAllyControl.Length);
			startChecking = false;
			currentTargetPosition = Vector2.zero;
		}

		private void TowerModel_OnUpgrade(TowerModel oldTower, TowerModel newTower)
		{
			UnityEngine.Debug.Log("tower upgrade from " + oldTower.Level + " to " + newTower.Level);
			newTower.towerSpawnAllyController.currentTargetPosition = currentTargetPosition;
			CancelInvoke("SpawnAllAllies");
			CancelInvoke("SpawnAllies");
			if (ListAllyControl.Length <= 0)
			{
				return;
			}
			AllyModel[] array = ListAllyControl;
			foreach (AllyModel allyModel in array)
			{
				if (allyModel != null)
				{
					EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_UPGRADE_TOWER_ON_ALLY);
					effect.transform.position = allyModel.transform.position;
					effect.Init(1f);
					allyModel.ReturnPool(0f);
				}
			}
		}

		private void TowerModel_OnSell(TowerModel towerModel)
		{
			CancelInvoke("SpawnAllAllies");
			CancelInvoke("SpawnAllies");
			if (ListAllyControl.Length <= 0)
			{
				return;
			}
			AllyModel[] array = ListAllyControl;
			foreach (AllyModel allyModel in array)
			{
				if (allyModel != null)
				{
					EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_SELL_TOWER_ON_ALLY);
					effect.transform.position = allyModel.transform.position;
					effect.Init(0.5f);
					allyModel.ReturnPool(0f);
				}
			}
		}

		private void Awake()
		{
			effectCaster = GetComponent<EffectCaster>();
			originalParameter = base.TowerModel.OriginalParameter;
			base.TowerModel.OnSell += TowerModel_OnSell;
			base.TowerModel.OnUpgrade += TowerModel_OnUpgrade;
		}

		private void Start()
		{
			SingletonMonoBehaviour<AlliesManager>.Instance.onAlliesMoveToAssignedPosition += Instance_onAlliesMoveToAssignedPosition;
		}

		private void OnDestroy()
		{
			base.TowerModel.OnSell -= TowerModel_OnSell;
			base.TowerModel.OnUpgrade -= TowerModel_OnUpgrade;
			SingletonMonoBehaviour<AlliesManager>.Instance.onAlliesMoveToAssignedPosition -= Instance_onAlliesMoveToAssignedPosition;
		}

		public override void Update()
		{
			base.Update();
			if (startChecking && !base.TowerModel.IsSilent)
			{
				if (timeTracking == 0f)
				{
					CheckNumberOfAllies();
				}
				if (GetNumberOfLivingAllies() < desiredAlliesNumber)
				{
					timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
				}
			}
		}

		private void Instance_onAlliesMoveToAssignedPosition(TowerModel inputTowerModel, Vector2 targetPosition)
		{
			if (inputTowerModel.GetInstanceID() != base.TowerModel.GetInstanceID())
			{
				return;
			}
			float num = Vector2.Distance(targetPosition, base.transform.position);
			float num2 = num;
			Tower tower = base.TowerModel.OriginalParameter;
			if (num2 < (float)tower.attackRangeMax / GameData.PIXEL_PER_UNIT)
			{
				SetAlliesToAssignedPosition(targetPosition);
				if ((bool)effectCaster)
				{
					effectCaster.CastEffect(SpawnFX.ICON_MOVEABLE_AllY, 2f, targetPosition);
				}
			}
			else if ((bool)effectCaster)
			{
				effectCaster.CastEffect(SpawnFX.ICON_UNMOVEABLE, 1f, targetPosition);
			}
		}

		private void setStartChecking()
		{
			startChecking = true;
		}

		private void CheckNumberOfAllies()
		{
			if (GetNumberOfLivingAllies() < desiredAlliesNumber)
			{
				SpawnAllies(desiredAlliesNumber - GetNumberOfLivingAllies());
			}
			Tower tower = base.TowerModel.OriginalParameter;
			timeTracking = (float)tower.reload / 1000f;
		}

		private int GetNumberOfLivingAllies()
		{
			int num = 0;
			for (int i = 0; i < ListAllyControl.Length; i++)
			{
				if (ListAllyControl[i] != null)
				{
					num++;
				}
			}
			return num;
		}

		private void SpawnAllies(int amount)
		{
			DoSpawn(amount);
			MoveAllyToReadyPosition(amount);
		}

		private void SpawnAllAllies()
		{
			DoSpawn(desiredAlliesNumber);
			MoveAllyToReadyPositionImmediately(desiredAlliesNumber);
		}

		private void DoSpawn(int amount)
		{
			if (door != null)
			{
				door.transform.localPosition = Vector3.zero;
				Sequence sequence = DOTween.Sequence();
				sequence.Append(door.transform.DOLocalMoveY(0.11f, 0.2f));
				sequence.AppendInterval(1f);
				sequence.Append(door.transform.DOLocalMoveY(0f, 0.6f));
				sequence.Play();
			}
			for (int i = 0; i < amount; i++)
			{
				AllyModel allyModel = null;
				allyModel = SingletonMonoBehaviour<SpawnAlly>.Instance.Get(base.TowerModel.Id, base.TowerModel.Level);
				allyModel.InitFromTower(originalParameter, this);
				allyModel.transform.position = spawnPosition.position;
				allyModel.gameObject.SetActive(value: true);
				AddAllyToListControl(allyModel);
			}
			OnSpawnAllies.Invoke();
		}

		private void AddAllyToListControl(AllyModel ally)
		{
			ListAllyControl[GetFreePositionInListControl()] = ally;
		}

		public void RemoveAllyFromListControl(AllyModel ally)
		{
			int allyIndexOfList = GetAllyIndexOfList(ally);
			startPositionParameters[allyIndexOfList].isUsing = false;
			ListAllyControl[allyIndexOfList] = null;
			CalculateIgnoreReloadChance();
		}

		public int GetAllyIndexOfList(AllyModel ally)
		{
			return Array.IndexOf(ListAllyControl, ally);
		}

		private int GetFreePositionInListPosition()
		{
			int result = -1;
			for (int i = 0; i < startPositionParameters.Length; i++)
			{
				if (!startPositionParameters[i].isUsing)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		private int GetFreePositionInListControl()
		{
			int result = -1;
			for (int i = 0; i < ListAllyControl.Length; i++)
			{
				if (ListAllyControl[i] == null)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		private void MoveAllyToReadyPosition(int amount)
		{
			int num = 0;
			for (int i = 0; i < amount; i++)
			{
				num = GetFreePositionInListPosition();
				ListAllyControl[num].AllyMovementController.MoveToReadyPos(startPositionParameters[num].position);
				startPositionParameters[num].isUsing = true;
			}
		}

		private void MoveAllyToReadyPositionImmediately(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				ListAllyControl[i].AllyMovementController.MoveToReadyPosImmediately(startPositionParameters[i].position, 0.75f);
				startPositionParameters[i].isUsing = true;
			}
		}

		private void CalculateReadyPosition(Vector2 readyPos)
		{
			Vector2 vector = readyPos;
			Vector3 position = new Vector3(vector.x, vector.y + offsetSpawnPosition.y, 0f);
			Vector3 position2 = new Vector3(vector.x + offsetSpawnPosition.x, vector.y - offsetSpawnPosition.y, 0f);
			Vector3 position3 = new Vector3(vector.x - offsetSpawnPosition.x, vector.y - offsetSpawnPosition.y, 0f);
			startPositionParameters[0].position = position;
			startPositionParameters[1].position = position2;
			startPositionParameters[2].position = position3;
		}

		public void SetAlliesToAssignedPosition(Vector2 targetPosition)
		{
			currentTargetPosition = targetPosition;
			CalculateReadyPosition(targetPosition);
			for (int i = 0; i < ListAllyControl.Length; i++)
			{
				if (ListAllyControl[i] != null)
				{
					ListAllyControl[i].AllyMovementController.MoveToReadyPos(startPositionParameters[i].position);
					startPositionParameters[i].isUsing = true;
				}
			}
		}

		private void ApplyBuffSilent()
		{
		}

		public void AddPassiveArmor(float bonusPhysicsArmor, float bonusMagicArmor)
		{
			UnityEngine.Debug.Log("add " + bonusPhysicsArmor + " " + bonusMagicArmor + " to all allies!");
			AllyModel[] array = ListAllyControl;
			foreach (AllyModel allyModel in array)
			{
				if ((bool)allyModel)
				{
					allyModel.AllyHealthController.CurrentPhysicsArmor = allyModel.AllyHealthController.OriginPhysicsArmor + bonusPhysicsArmor;
					allyModel.AllyHealthController.CurrentMagicArmor = allyModel.AllyHealthController.OriginMagicArmor + bonusMagicArmor;
				}
			}
		}

		private void CalculateIgnoreReloadChance()
		{
			if (ignoreReloadChance > 0 && UnityEngine.Random.Range(0, 100) < ignoreReloadChance)
			{
				UnityEngine.Debug.Log("bo qua reload!");
				timeTracking = 0f;
			}
		}

		public void UnlockRangeAttackAbility(float attackRangeBonus)
		{
			UnityEngine.Debug.Log("add " + attackRangeBonus + " attack range to all allies!");
			AllyModel[] array = ListAllyControl;
			foreach (AllyModel allyModel in array)
			{
				if ((bool)allyModel)
				{
					allyModel.AllyAttackController.rangeAttack = true;
					allyModel.CurrentAttackRangeMax = allyModel.AttackRangeMax + attackRangeBonus;
				}
			}
		}

		public void UnlockDodgeAbility(int dodgeRateBonus)
		{
			AllyModel[] array = ListAllyControl;
			foreach (AllyModel allyModel in array)
			{
				if ((bool)allyModel)
				{
					allyModel.CurrentDodgeChance = allyModel.DodgeChance + dodgeRateBonus;
				}
			}
		}

		public void UnlockIgnoreArmorAbility(int ignoreArmorRateBonus)
		{
			AllyModel[] array = ListAllyControl;
			foreach (AllyModel allyModel in array)
			{
				if ((bool)allyModel)
				{
					allyModel.CurrentIgnoreArmorChance = allyModel.IgnoreArmorChance + ignoreArmorRateBonus;
				}
			}
		}

		public void UnlockSkillSlash(float duration, float cooldown, string description, bool isActiveAtStart)
		{
			AllyModel[] array = ListAllyControl;
			foreach (AllyModel allyModel in array)
			{
				if ((bool)allyModel)
				{
					AllySkillSlash componentInChildren = allyModel.GetComponentInChildren<AllySkillSlash>();
					if ((bool)componentInChildren)
					{
						componentInChildren.Init(duration, cooldown, description, isActiveAtStart);
					}
				}
			}
		}
	}
}
