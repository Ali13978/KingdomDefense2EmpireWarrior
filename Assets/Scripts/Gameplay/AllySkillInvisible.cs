using UnityEngine;

namespace Gameplay
{
	public class AllySkillInvisible : AllyController
	{
		[SerializeField]
		private bool changeAlphaWhenInvisible;

		[SerializeField]
		private SpriteRenderer spriteRenderer;

		private Color tmpColor;

		private bool isInvisible;

		public override void Update()
		{
			base.Update();
			isInvisible = (base.AllyModel.curState == EntityStateEnum.HeroIdle);
			base.AllyModel.IsInvisible = isInvisible;
			if (changeAlphaWhenInvisible)
			{
				SetAlpha(isInvisible);
			}
		}

		private void SetAlpha(bool isInvisible)
		{
			tmpColor = spriteRenderer.color;
			if (isInvisible)
			{
				tmpColor.a = 0.5f;
			}
			else
			{
				tmpColor.a = 1f;
			}
			spriteRenderer.color = tmpColor;
		}
	}
}
