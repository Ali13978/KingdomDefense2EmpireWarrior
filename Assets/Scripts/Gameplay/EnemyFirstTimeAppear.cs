using Middle;
using UnityEngine;

namespace Gameplay
{
	[DisallowMultipleComponent]
	public class EnemyFirstTimeAppear : EnemyController
	{
		public override void OnAppear()
		{
			base.OnAppear();
			if (UnlockedEnemies.Instance.IsEnemyFirstTime(base.EnemyModel.Id))
			{
				NewEnemyInformationUIManager.Instance.TryActivateButton(base.EnemyModel.Id);
			}
		}
	}
}
