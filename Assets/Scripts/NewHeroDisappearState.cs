using DG.Tweening;
using Gameplay;
using UnityEngine;

public class NewHeroDisappearState : NewHeroState
{
	private SpriteRenderer spriteRenderer;

	private bool isDiappearedIntermediate;

	public NewHeroDisappearState(CharacterModel heroModel, INewFSMController fSMController, bool isDiappearedIntermediate = false)
		: base(heroModel, fSMController)
	{
		this.isDiappearedIntermediate = isDiappearedIntermediate;
	}

	public override void OnStartState()
	{
		base.OnStartState();
		if (isDiappearedIntermediate)
		{
			OnFadeOutComplete();
			return;
		}
		spriteRenderer = heroModel.GetComponent<SpriteRenderer>();
		DOTween.To(() => 255f, delegate(float x)
		{
			spriteRenderer.color = new Color(1f, 1f, 1f, x / 255f);
		}, 0f, 1f).OnComplete(OnFadeOutComplete);
	}

	private void OnFadeOutComplete()
	{
		heroModel.ReturnPool(0f);
	}
}
