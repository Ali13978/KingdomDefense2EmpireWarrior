using GeneralVariable;
using Parameter;
using System;
using Tutorial;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay
{
	public class InputFilterManager : MonoBehaviour
	{
		[Space]
		[Header("Threshold")]
		[SerializeField]
		private float movementThresHole;

		private const float Vector2DZ = 10f;

		[Space]
		[Header("Mouse click threshold")]
		[SerializeField]
		private float deltaTimeClick;

		[SerializeField]
		private float deltaDistanceClick = 40f;

		private float timeMouseDown;

		private float timeMouseUp;

		private Vector2 distanceMouseDown;

		private Vector2 distanceMouseUp;

		private float timeTracking;

		private bool isClickUI;

		private bool isMovingCamera;

		private RaycastHit2D hit;

		private RaycastHit2D mapHit;

		private int mapLayerIndex;

		private EffectCaster effectCaster;

		private int lastTouchCount;

		public static InputFilterManager Instance
		{
			get;
			set;
		}

		public event Action onMouseStayEvent;

		public event Action onMouseClickEvent;

		public event Action onMouseDownEvent;

		public event Action onMouseUpEvent;

		public event Action onMoveCamera;

		public event Action onZoomCamera;

		public event Action onClickBuildRegion;

		private void Awake()
		{
			Instance = this;
			effectCaster = GetComponent<EffectCaster>();
			mapLayerIndex = 1 << LayerMask.NameToLayer("Map");
		}

		private void Start()
		{
			GameEventCenter.Instance.Subscribe(GameEventType.OnClickButton, new ClickButtonSubscriberData(GameTools.GetUniqueId(), OnAButtonClicked));
		}

		private void Update()
		{
			if (SingletonMonoBehaviour<GameData>.Instance.IsGameOver || SingletonMonoBehaviour<GameData>.Instance.IsAnyPopupOpen || SingletonMonoBehaviour<GameData>.Instance.IsAnyTutorialPopupOpen)
			{
				return;
			}
			if (UnityEngine.Input.touchCount == 1 && UnityEngine.Input.GetTouch(0).phase == TouchPhase.Moved && GameplayTutorialManager.Instance.IsTutorialDone())
			{
				Vector2 deltaWorldPosition = GetDeltaWorldPosition(UnityEngine.Input.GetTouch(0).deltaPosition, Camera.main);
				if (Mathf.Abs(deltaWorldPosition.x) > movementThresHole || Mathf.Abs(deltaWorldPosition.y) > movementThresHole)
				{
					isMovingCamera = true;
					if (this.onMoveCamera != null)
					{
						this.onMoveCamera();
						return;
					}
				}
			}
			else if (UnityEngine.Input.touchCount == 2 && GameplayTutorialManager.Instance.IsTutorialDone() && this.onZoomCamera != null)
			{
				this.onZoomCamera();
				SetIsClickingUI();
				return;
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0) && UnityEngine.Input.touchCount == 1 && EventSystem.current.IsPointerOverGameObject(UnityEngine.Input.GetTouch(0).fingerId))
			{
				SetIsClickingUI();
				return;
			}
			if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse0))
			{
				OnClick();
				if (isClickUI)
				{
					return;
				}
				if (isMovingCamera)
				{
					isMovingCamera = false;
					return;
				}
				if (UnityEngine.Input.touchCount == 1 && EventSystem.current.IsPointerOverGameObject(UnityEngine.Input.GetTouch(0).fingerId))
				{
					SetIsClickingUI();
					return;
				}
				hit = Physics2D.Raycast(getTargetVector(), Vector3.back, 5f);
				mapHit = Physics2D.Raycast(getTargetVector(), Vector3.back, 5f, mapLayerIndex);
				HandleInput(new ClickInputData(ClickInputPhase.Up, mapHit, hit));
			}
			if (timeTracking == 0f)
			{
				ResetTimeout();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		private void ResetTimeout()
		{
			isClickUI = false;
		}

		public void SetIsClickingUI()
		{
			isClickUI = true;
			timeTracking = deltaTimeClick;
		}

		public static bool IsPointerOverGUI()
		{
			bool flag = false;
			for (int i = 0; i < Input.touches.Length; i++)
			{
				flag = EventSystem.current.IsPointerOverGameObject(UnityEngine.Input.GetTouch(i).fingerId);
				if (flag)
				{
					break;
				}
			}
			return flag;
		}

		private Vector2 getTargetVector()
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			return new Vector2(vector.x, vector.y);
		}

		private Vector2 GetDeltaWorldPosition(Vector2 deltaScreen, Camera camera)
		{
			Vector3 position = deltaScreen;
			position.z = 10f;
			Vector3 position2 = new Vector3(0f, 0f, 10f);
			return camera.ScreenToWorldPoint(position) - camera.ScreenToWorldPoint(position2);
		}

		public void OnAButtonClicked(ClickedObjectData clickedObjData)
		{
			SetIsClickingUI();
		}

		private void OnClick()
		{
			GameEventCenter.Instance.Trigger(GameEventType.OnSelectEnemy, -1);
			GameEventCenter.Instance.Trigger(GameEventType.OnSelectPet, -1);
			GameEventCenter.Instance.Trigger(GameEventType.OnSelectAlly, null);
		}

		private void HandleInput(ClickInputData inputData)
		{
			ClickInputPhase clickInputPhase = inputData.clickInputPhase;
			if (clickInputPhase == ClickInputPhase.Up)
			{
				if (HeroesManager.Instance.HeroIDChoosing != -1)
				{
					OnHandleInput_ClickMapToMoveHero(inputData);
				}
				else if (HeroesManager.Instance.HeroSkillIDChoosing != -1)
				{
					OnHandleInput_ClickMapToCastSkill(inputData);
				}
				else if (SingletonMonoBehaviour<PowerUpItemPopupController>.Instance.selectingItemID >= 0)
				{
					OnHandleInput_ClickMapToCastPowerupItem(inputData);
				}
				else if (SingletonMonoBehaviour<GameData>.Instance.RecordingPosition)
				{
					OnHandleInput_AssignAllyPosition(inputData);
				}
				else if (inputData.CompareTag(inputData.entityHit, GeneralVariable.GeneralVariable.DROPPED_GOLD))
				{
					OnHandleInput_CollectDroppedGold(inputData);
				}
				else if (inputData.CompareTag(inputData.entityHit, GeneralVariable.GeneralVariable.HERO_TAG))
				{
					OnHandleInput_ClickSelectHero(inputData);
				}
				else if (inputData.CompareTag(inputData.entityHit, GeneralVariable.GeneralVariable.BUILD_REGION_TAG))
				{
					OnHandleInput_ClickEmptyBuildArea(inputData);
				}
				else if (inputData.CompareTag(inputData.entityHit, GeneralVariable.GeneralVariable.TOWER_TAG))
				{
					OnHandleInput_ClickTower(inputData);
				}
				else if (inputData.CompareTag(inputData.entityHit, GeneralVariable.GeneralVariable.ENEMY_TAG))
				{
					OnHandleInput_ClickSelectEnemy(inputData);
				}
				else if (inputData.CompareTag(inputData.entityHit, GeneralVariable.GeneralVariable.PET_TAG))
				{
					OnHandleInput_ClickSelectPet(inputData);
				}
				else if (inputData.CompareTag(inputData.entityHit, GeneralVariable.GeneralVariable.ALLY_TAG))
				{
					OnHandleInput_ClickSelectAlly(inputData);
				}
			}
		}

		public void OnHandleInput_ClickMapToMoveHero(ClickInputData inputData)
		{
			bool flag = inputData.CompareTag(inputData.mapHit, GeneralVariable.GeneralVariable.UN_MOVEABLE_TAG);
			if (!flag)
			{
				HeroesManager.Instance.MoveHeroToAssignedPosition(HeroesManager.Instance.HeroIDChoosing, getTargetVector());
			}
			if ((bool)effectCaster)
			{
				if (flag)
				{
					effectCaster.CastEffect(SpawnFX.ICON_UNMOVEABLE, 1f, getTargetVector());
				}
				else
				{
					effectCaster.CastEffect(SpawnFX.ICON_MOVEABLE_HERO, 2f, getTargetVector());
				}
			}
			HeroesManager.Instance.UnChooseHero(HeroesManager.Instance.HeroIDChoosing);
		}

		public void OnHandleInput_ClickMapToCastSkill(ClickInputData inputData)
		{
			if (!inputData.CompareTag(inputData.mapHit, GeneralVariable.GeneralVariable.UN_MOVEABLE_TAG))
			{
				HeroesManager.Instance.CastHeroSkillToAssignedPosition(HeroesManager.Instance.HeroSkillIDChoosing, getTargetVector());
			}
			else if ((bool)effectCaster)
			{
				effectCaster.CastEffect(SpawnFX.ICON_UNMOVEABLE, 1f, getTargetVector());
			}
			HeroesManager.Instance.UnChooseHeroSkill(HeroesManager.Instance.HeroSkillIDChoosing);
		}

		public void OnHandleInput_ClickMapToCastPowerupItem(ClickInputData inputData)
		{
			if (!inputData.CompareTag(inputData.mapHit, GeneralVariable.GeneralVariable.UN_MOVEABLE_TAG))
			{
				SingletonMonoBehaviour<PowerUpItemPopupController>.Instance.CastItemSkill();
			}
		}

		public void OnHandleInput_AssignAllyPosition(ClickInputData inputData)
		{
			bool flag = inputData.CompareTag(inputData.mapHit, GeneralVariable.GeneralVariable.ROAD_TAG);
			if (flag)
			{
				SingletonMonoBehaviour<AlliesManager>.Instance.MoveAlliesToAssignedPosition(SingletonMonoBehaviour<GameData>.Instance.CurrentTowerSelected, getTargetVector());
			}
			if ((bool)effectCaster && !flag)
			{
				effectCaster.CastEffect(SpawnFX.ICON_UNMOVEABLE, 1f, getTargetVector());
			}
			SingletonMonoBehaviour<AlliesManager>.Instance.UnChooseTower(SingletonMonoBehaviour<GameData>.Instance.CurrentTowerSelected);
		}

		public void OnHandleInput_CollectDroppedGold(ClickInputData inputData)
		{
			inputData.entityHit.collider.gameObject.GetComponent<ProducedGoldController>().TapOnGold();
		}

		public void OnHandleInput_ClickEmptyBuildArea(ClickInputData inputData)
		{
			inputData.entityHit.transform.gameObject.GetComponent<BuildRegionController>().TryToClick();
			if (this.onClickBuildRegion != null)
			{
				this.onClickBuildRegion();
			}
		}

		public void OnHandleInput_ClickTower(ClickInputData inputData)
		{
			inputData.entityHit.transform.gameObject.GetComponent<TowerModel>().ChooseTower();
		}

		public void OnHandleInput_ClickSelectHero(ClickInputData inputData)
		{
			HeroModel component = inputData.entityHit.collider.GetComponent<HeroModel>();
			if (component != null)
			{
				GameEventCenter.Instance.Trigger(GameEventType.OnSelectHero, component.HeroID);
			}
		}

		public void OnHandleInput_ClickSelectEnemy(ClickInputData inputData)
		{
			EnemyModel component = inputData.entityHit.collider.GetComponent<EnemyModel>();
			component.OnSelected();
			if (component != null)
			{
				GameEventCenter instance = GameEventCenter.Instance;
				Enemy originalParameter = component.OriginalParameter;
				instance.Trigger(GameEventType.OnSelectEnemy, originalParameter.id);
			}
		}

		public void OnHandleInput_ClickSelectPet(ClickInputData inputData)
		{
			HeroModel component = inputData.entityHit.collider.GetComponent<HeroModel>();
			if (component != null)
			{
				GameEventCenter.Instance.Trigger(GameEventType.OnSelectPet, component.HeroID);
			}
		}

		public void OnHandleInput_ClickSelectAlly(ClickInputData inputData)
		{
			AllyModel component = inputData.entityHit.collider.GetComponent<AllyModel>();
			if (component != null)
			{
				GameEventCenter.Instance.Trigger(GameEventType.OnSelectAlly, component);
			}
		}
	}
}
