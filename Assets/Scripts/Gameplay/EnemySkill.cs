namespace Gameplay
{
	public abstract class EnemySkill : EnemyController
	{
		public EnemySkillActivation EnemySkillActivation
		{
			get;
			set;
		}

		public abstract float CoolDownDuration
		{
			get;
		}

		public abstract float ActiveDuration
		{
			get;
		}

		public abstract bool OnActive();

		public abstract void OnInactive();
	}
}
