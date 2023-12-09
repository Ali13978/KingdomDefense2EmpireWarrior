using DG.Tweening;
using SSR.Core.Architecture;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
	[SerializeField]
	private GameObject _target;

	[SerializeField]
	private OrderedEventDispatcher onStart = new OrderedEventDispatcher();

	[SerializeField]
	private OrderedEventDispatcher onFinish = new OrderedEventDispatcher();

	public Vector3 targetLastPos = new Vector3(0f, 0f, 0f);

	public bool isRunning;

	public virtual GameObject target
	{
		get
		{
			return _target;
		}
		set
		{
			_target = value;
		}
	}

	public void ButtonClose()
	{
		float duration = 0.3f;
		base.transform.DOScale(Vector3.zero, duration).SetEase(Ease.InBack);
	}

	public void ButtonOpen()
	{
		float num = 0.3f;
		Sequence s = DOTween.Sequence();
		s.Append(base.transform.DOScale(Vector3.one * 1.2f, num).SetEase(Ease.InOutBack));
		s.Append(base.transform.DOScale(Vector3.one, 0.25f * num).SetEase(Ease.InBack));
	}

	public void SlotOpen()
	{
		float num = 0.5f;
		base.transform.DOScale(Vector3.one * 1.1f, num).SetEase(Ease.InOutBack);
		base.transform.DOScale(Vector3.one, 0.25f * num).SetEase(Ease.InBack).SetDelay(num);
	}

	public virtual void Run()
	{
		isRunning = true;
		onStart.Dispatch();
	}

	public virtual void Stop()
	{
		onFinish.Dispatch();
	}

	public virtual void StopImmediate()
	{
		isRunning = false;
		onFinish.Dispatch();
	}

	public virtual void OnStop()
	{
		isRunning = false;
	}

	public virtual void SetSize(float size)
	{
	}
}
