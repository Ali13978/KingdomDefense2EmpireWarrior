using MyCustom;

namespace Gameplay
{
	public class HeroSkillCommon : CustomMonoBehaviour
	{
		private HeroModel heroModel;

		public virtual void Update()
		{
		}

		public virtual void Init(HeroModel heroModel)
		{
			this.heroModel = heroModel;
		}

		public virtual void CastDamage()
		{
		}

		public virtual void CastDamage(EnemyModel enemy)
		{
		}

		public virtual float GetCooldownTime()
		{
			return 0f;
		}

		public virtual string GetUseType()
		{
			return string.Empty;
		}

		public virtual void OnHeroReturnPool()
		{
		}

		public virtual bool IsEmptySpecialState()
		{
			return !(heroModel.GetFSMController().GetCurrentState() is NewHeroSpecialState);
		}
	}
}
