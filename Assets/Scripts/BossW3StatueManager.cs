using UnityEngine;

public class BossW3StatueManager : MonoBehaviour
{
	public static BossW3StatueManager instance;

	public Animator iceAnimator;

	public GameObject bossStatue;

	private void Start()
	{
		instance = this;
	}

	public void BreakIce()
	{
		iceAnimator.Play("IceBreakAnim");
	}
}
