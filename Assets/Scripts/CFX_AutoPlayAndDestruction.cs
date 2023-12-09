using UnityEngine;

public class CFX_AutoPlayAndDestruction : MonoBehaviour
{
	public Animator animator;

	public string animName;

	public float duration = 1f;

	private float countdown;

	private void OnEnable()
	{
		countdown = duration;
		if (!string.IsNullOrEmpty(animName))
		{
			animator.Play(animName);
		}
	}

	public void SetTimer(float duration)
	{
		countdown = duration;
	}

	private void Update()
	{
		countdown -= Time.deltaTime;
		if (countdown <= 0f)
		{
			base.gameObject.Recycle();
		}
	}
}
