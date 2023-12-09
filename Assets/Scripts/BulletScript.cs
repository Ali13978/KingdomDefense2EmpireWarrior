using MyCustom;
using UnityEngine;

public class BulletScript : CustomMonoBehaviour
{
	[SerializeField]
	private float offsetTime;

	private float duration;

	public void Init(float _duration)
	{
		duration = _duration + offsetTime;
		CustomInvoke(AutoDestroy, duration);
	}

	private void AutoDestroy()
	{
		UnityEngine.Debug.Log("dan den dich, tu huy!");
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
