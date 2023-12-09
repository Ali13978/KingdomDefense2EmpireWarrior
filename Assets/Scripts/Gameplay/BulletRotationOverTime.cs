using UnityEngine;

namespace Gameplay
{
	public class BulletRotationOverTime : MonoBehaviour
	{
		[SerializeField]
		private float rotationSpeed;

		private EnemyModel targetEnemy;

		private Vector3 targetPosition;

		private BulletModel bulletModel;

		public void Awake()
		{
			bulletModel = GetComponent<BulletModel>();
			bulletModel.OnInitialized += BulletModel_OnInitialized;
		}

		private void BulletModel_OnInitialized()
		{
			targetEnemy = bulletModel.target;
		}

		private void Update()
		{
			Rotation();
		}

		private int GetRelativePositionX()
		{
			int num = 0;
			if ((bool)targetEnemy)
			{
				Vector3 position = targetEnemy.transform.position;
				float x = position.x;
				Vector3 position2 = base.gameObject.transform.position;
				if (x - position2.x > 0f)
				{
					return 1;
				}
			}
			return -1;
		}

		private void Rotation()
		{
			if (GetRelativePositionX() > 0)
			{
				base.transform.Rotate(rotationSpeed * Vector3.back);
			}
			else
			{
				base.transform.Rotate(rotationSpeed * Vector3.forward);
			}
		}
	}
}
