using Parameter;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

namespace Gameplay
{
	public class HeroMovementController : HeroController
	{
		private List<string> increaseMovementSpeedBuffKeys = new List<string>
		{
			"IncreaseMovementSpeed"
		};

		public Vector3 assignedPosition;

		private float speed;

		public bool isMoving => base.HeroModel.GetFSMController().GetCurrentState() is NewHeroMoveState;

		public float GetSpeed()
		{
			return speed;
		}

		public override void OnAppear()
		{
			base.OnAppear();
			base.HeroModel.SetAssignedPosition(base.transform.position);
			SetParameter();
			base.HeroModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		private void SetParameter()
		{
			Hero originalParameter = base.HeroModel.OriginalParameter;
			speed = originalParameter.speed;
		}

		private void Start()
		{
			HeroesManager.Instance.onHeroMoveToAssignedPosition += Instance_onHeroMoveToAssignedPosition;
		}

		private void OnDestroy()
		{
			HeroesManager.Instance.onHeroMoveToAssignedPosition -= Instance_onHeroMoveToAssignedPosition;
		}

		public override void Update()
		{
			base.Update();
			if (HeroesManager.Instance.HeroIDChoosing == base.HeroModel.HeroID && GameplayTutorialManager.Instance.IsTutorialMap())
			{
				GameplayTutorialManager.Instance.TutorialMoveHero.TryMoveToStep(1);
			}
		}

		private void Instance_onHeroMoveToAssignedPosition(int heroID, Vector2 targetPosition)
		{
			if (base.HeroModel.HeroID == heroID)
			{
				if (!base.HeroModel.IsAlive)
				{
					HeroesManager.Instance.UnChooseHero(base.HeroModel.HeroID);
				}
				else
				{
					base.HeroModel.UnitSoundController.PlayStartMove();
				}
			}
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (increaseMovementSpeedBuffKeys.Contains(buffKey))
			{
				ApplyIncreaseMovementSpeed();
			}
		}

		private void ApplyIncreaseMovementSpeed()
		{
			float buffsValue = base.HeroModel.BuffsHolder.GetBuffsValue(increaseMovementSpeedBuffKeys);
			Hero originalParameter = base.HeroModel.OriginalParameter;
			int num = originalParameter.speed;
			Hero originalParameter2 = base.HeroModel.OriginalParameter;
			speed = num + (int)((float)originalParameter2.speed * buffsValue / 100f);
		}
	}
}
