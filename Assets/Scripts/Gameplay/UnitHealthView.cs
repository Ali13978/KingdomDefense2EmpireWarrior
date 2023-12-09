using MyCustom;
using UnityEngine;

namespace Gameplay
{
	public class UnitHealthView : CustomMonoBehaviour
	{
		[SerializeField]
		private Transform healthBarStatus;

		[Space]
		[Header("Reference")]
		[SerializeField]
		private SpriteRenderer healthStatus;

		[SerializeField]
		private SpriteRenderer blackBar;

		[Space]
		[Header("Assets")]
		[SerializeField]
		private Sprite smallBlackBar;

		[Space]
		[Header("Assets")]
		[SerializeField]
		private Sprite bigBlackBar;

		[SerializeField]
		private Sprite redHealthBar;

		[SerializeField]
		private Sprite bigRedHealthBar;

		[SerializeField]
		private Sprite greenHealthBar;

		private GameObject target;

		private Transform healthBarPosition;

		private bool isReadyToUse;

		[Space]
		[SerializeField]
		private float smallOffset;

		[Space]
		[SerializeField]
		private float bigOffset;

		private CharacterType characterType;

		private void Update()
		{
			if (isReadyToUse && (bool)healthBarPosition)
			{
				UpdatePositionFollowUnit();
			}
		}

		public void SetupHealth(CharacterType _characterType, GameObject _target, Transform _healthBarPosition)
		{
			characterType = _characterType;
			switch (characterType)
			{
			case CharacterType.Ally:
				healthStatus.sprite = greenHealthBar;
				blackBar.sprite = smallBlackBar;
				healthBarStatus.localPosition = new Vector3(smallOffset, 0f, 0f);
				Hide();
				break;
			case CharacterType.Enemy:
				healthStatus.sprite = redHealthBar;
				blackBar.sprite = smallBlackBar;
				healthBarStatus.localPosition = new Vector3(smallOffset, 0f, 0f);
				Hide();
				break;
			case CharacterType.Boss:
				healthStatus.sprite = bigRedHealthBar;
				blackBar.sprite = bigBlackBar;
				healthBarStatus.localPosition = new Vector3(bigOffset, -0.003f, 0f);
				Show();
				break;
			}
			target = _target;
			healthBarPosition = _healthBarPosition;
			healthBarStatus.transform.localScale = Vector3.one;
			isReadyToUse = true;
		}

		public void UpdateHealth(int _currentHealth, int _originHealth)
		{
			int num = Mathf.Clamp(_currentHealth, 0, _originHealth);
			if ((bool)healthBarStatus)
			{
				healthBarStatus.localScale = new Vector3((float)num / (float)_originHealth, 1f, 1f);
			}
			if (isReadyToUse && characterType != CharacterType.Boss)
			{
				if (_currentHealth == _originHealth)
				{
					Hide();
				}
				else
				{
					Show();
				}
			}
		}

		private void UpdatePositionFollowUnit()
		{
			base.transform.position = healthBarPosition.position;
		}

		public void OnReturnPool()
		{
			Hide();
			isReadyToUse = false;
			SingletonMonoBehaviour<SpawnUnitHealthBar>.Instance.Push(this);
		}

		private void Show()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
