using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class EnemyFindTargetController : EnemyController
	{
		public bool activeFindTarget;

		public CharacterModel Target;

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void OnAppear()
		{
			base.OnAppear();
			SetParameter();
		}

		private void SetParameter()
		{
			Target = null;
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			Target = null;
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Vector3 position = base.transform.position;
			Enemy originalParameter = base.EnemyModel.OriginalParameter;
			Gizmos.DrawWireSphere(position, (float)originalParameter.attack_range_average / GameData.PIXEL_PER_UNIT);
			Gizmos.color = Color.yellow;
			Vector3 position2 = base.transform.position;
			Enemy originalParameter2 = base.EnemyModel.OriginalParameter;
			Gizmos.DrawWireSphere(position2, (float)originalParameter2.attack_range_max / GameData.PIXEL_PER_UNIT);
		}

		public void AddTarget(CharacterModel newtarget)
		{
			if (!activeFindTarget && Target == null)
			{
				Target = newtarget;
			}
		}
	}
}
