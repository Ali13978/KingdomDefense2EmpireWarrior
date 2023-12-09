using Data;
using UnityEngine;
using UnityEngine.UI;

public class TotalGem : MonoBehaviour
{
	[SerializeField]
	private Text textGem;

	[SerializeField]
	private Animator gemTextAnimator;

	private void Awake()
	{
		ReadWriteDataPlayerCurrency.Instance.OnGemChangeEvent += Instance_OnGemChangeEvent;
	}

	private void OnDestroy()
	{
		ReadWriteDataPlayerCurrency.Instance.OnGemChangeEvent -= Instance_OnGemChangeEvent;
	}

	private void Instance_OnGemChangeEvent()
	{
		UpdateGemMessage();
	}

	private void Start()
	{
		UpdateGemMessage();
	}

	public void UpdateGemMessage()
	{
		textGem.text = ReadWriteDataPlayerCurrency.Instance.GetCurrentGem().ToString();
	}

	public void PlayAnimationNotEnoughGem()
	{
		gemTextAnimator.SetTrigger("Anim");
	}
}
