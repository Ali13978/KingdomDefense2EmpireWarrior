namespace Gameplay
{
	public class LogicEnemySkillTransform : EnemyController
	{
		public void TransformUndergrounToBattleground()
		{
			base.EnemyModel.IsUnderground = false;
		}
	}
}
