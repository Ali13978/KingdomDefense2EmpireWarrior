public interface IAnimationController
{
	void ToRunState();

	void ToIdleState();

	void ToMeleeAttackState();

	void ToRangeAttackState();

	void ToDieState();

	void ToAppearState();

	bool ContainAppearAnim();

	void ToPlayState();

	void ToSpecialState(string animationName, float duration);
}
