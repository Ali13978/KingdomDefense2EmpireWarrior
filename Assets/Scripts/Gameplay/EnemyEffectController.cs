using DG.Tweening;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class EnemyEffectController : EnemyController
	{
		private static Color freezeColor = new Color(137f / 255f, 0.9411765f, 1f, 1f);

		private static Color slowColor = new Color(49f / 85f, 227f / 255f, 197f / 255f, 1f);

		private SpriteRenderer spriteRenderer;

		private Color colorOrigin = new Color(1f, 1f, 1f, 1f);

		private Color currentColor = new Color(1f, 1f, 1f, 1f);

		[Space]
		[Header("General")]
		[SerializeField]
		private Transform pointEffectPosition;

		[SerializeField]
		private Transform centerPosition;

		[Space]
		[Header("Effect mờ dần khi chết")]
		[SerializeField]
		private bool effectFadeOnDieAnim;

		[SerializeField]
		private float totalDieAnimTime;

		[SerializeField]
		private float dieAnimTime;

		private float fadeTime;

		private float alphaValue;

		private Tweener tween;

		[Space]
		[Header("Đổi hình ảnh khi bị đóng băng")]
		[SerializeField]
		private bool changeSpriteWhenFreezing;

		[SerializeField]
		private Sprite freezingSprite;

		[SerializeField]
		private Sprite normalSprite;

		private List<EffectController> listEffectsOnEnemy = new List<EffectController>();

		private void Awake()
		{
			spriteRenderer = base.transform.parent.GetComponent<SpriteRenderer>();
			base.EnemyModel.OnEnemyDied += EnemyModel_OnEnemyDied;
			fadeTime = totalDieAnimTime / 2f - dieAnimTime / 2f;
		}

		private void EnemyModel_OnEnemyDied()
		{
			CustomInvoke(FadeOut, dieAnimTime);
		}

		private void OnDestroy()
		{
			base.EnemyModel.OnEnemyDied -= EnemyModel_OnEnemyDied;
		}

		public override void OnAppear()
		{
			base.OnAppear();
			SetNormalColor();
			RemoveAllFXs();
			Show();
			if (changeSpriteWhenFreezing)
			{
				SetNormalSprite();
			}
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			SetNormalColor();
			RemoveAllFXs();
		}

		public void PlayDamageFX(DamageFXType damageFxType, float activationTime = 1f)
		{
			switch (damageFxType)
			{
			case DamageFXType.Poison:
				break;
			case DamageFXType.Slow:
				PlayFXSlow(activationTime);
				break;
			case DamageFXType.Freezing:
				PlayFXFreezing(activationTime);
				break;
			case DamageFXType.Critical:
				PlayFXCritical();
				break;
			case DamageFXType.Stun:
				PlayFXStun(activationTime);
				break;
			case DamageFXType.Root:
				PlayFXRoot(activationTime);
				break;
			case DamageFXType.Electric:
				PlayFXElectric(activationTime);
				break;
			case DamageFXType.Thunder:
				PlayFXThunder();
				break;
			case DamageFXType.Bleed:
				PlayFXBleed(activationTime);
				break;
			case DamageFXType.DefDown:
				PlayFXDefdown(activationTime);
				break;
			case DamageFXType.Poison1:
				PlayFXPoison1(activationTime);
				break;
			}
		}

		public void Show()
		{
			spriteRenderer.enabled = true;
		}

		public void Hide()
		{
			spriteRenderer.enabled = false;
		}

		public void PlayFXCritical(float activationTime = 2f)
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_CRITICAL);
			effect.transform.position = pointEffectPosition.position;
			effect.Init(activationTime, effect.transform);
		}

		public void PlayFXStun(float activationTime = 2f)
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_STUN);
			effect.transform.position = pointEffectPosition.position;
			effect.Init(activationTime, pointEffectPosition);
		}

		public void PlayFXRoot(float activationTime = 2f)
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_ROOT);
			effect.transform.position = base.transform.position;
			effect.Init(activationTime, base.transform);
		}

		public void PlayFXElectric(float activationTime = 2f)
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_ELECTRIC);
			effect.transform.position = base.transform.position;
			effect.Init(activationTime, base.transform.position);
		}

		public void PlayFXThunder()
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_THUNDER);
			effect.transform.position = pointEffectPosition.position;
			effect.Init(1f, base.transform.position);
		}

		public void PlayFXMiss(float activationTime)
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_MISS);
			effect.transform.position = pointEffectPosition.position;
			effect.Init(activationTime, base.transform);
		}

		public void PlayFXBurning(float activationTime)
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_BURNING);
			effect.transform.position = base.transform.position;
			effect.Init(base.transform);
			listEffectsOnEnemy.Add(effect);
			StartCoroutine(RemoveFX(effect, activationTime));
		}

		public void PlayFXDefdown(float activationTime)
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_DEF_DOWN);
			effect.transform.position = base.transform.position;
			effect.Init(base.transform);
			listEffectsOnEnemy.Add(effect);
			StartCoroutine(RemoveFX(effect, activationTime));
		}

		private void PlayFXBleed(float activationTime)
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_BLEED);
			effect.transform.position = pointEffectPosition.position;
			effect.Init(activationTime, pointEffectPosition);
		}

		private void PlayFXPoison1(float activationTime)
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_POISON1);
			effect.transform.position = centerPosition.transform.position;
			effect.Init(activationTime, centerPosition);
			EffectController effectController = effect;
			Enemy originalParameter = base.EnemyModel.OriginalParameter;
			effectController.SetSize(originalParameter.size);
		}

		private void PlayFXSlow(float activationTime)
		{
			spriteRenderer.color = slowColor;
			CancelInvoke("SetNormalColor");
			CustomInvoke(SetNormalColor, activationTime);
		}

		public void PlayFXFreezing(float activationTime)
		{
			if (changeSpriteWhenFreezing)
			{
				ChangeFreezingSprite(activationTime);
				return;
			}
			ChangeFreezingColor(activationTime);
			FreezeEnemyFoot(activationTime);
		}

		private void FreezeEnemyFoot(float activationTime)
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_ITEM_FREEZE);
			effect.transform.position = base.transform.position;
			effect.Init(base.transform);
			EffectController effectController = effect;
			Enemy originalParameter = base.EnemyModel.OriginalParameter;
			effectController.SetSize(originalParameter.size);
			listEffectsOnEnemy.Add(effect);
			StartCoroutine(RemoveFX(effect, activationTime));
		}

		private void ChangeFreezingColor(float activationTime)
		{
			CancelInvoke("SetNormalColor");
			spriteRenderer.color = freezeColor;
			CustomInvoke(SetNormalColor, activationTime);
		}

		private void ChangeFreezingSprite(float activationTime)
		{
			base.EnemyModel.EnemyAnimationController.TurnOffAnimator();
			CancelInvoke("SetNormalSprite");
			spriteRenderer.sprite = freezingSprite;
			CustomInvoke(SetNormalSprite, activationTime);
		}

		private void FadeOut()
		{
			alphaValue = 0f;
			if (effectFadeOnDieAnim)
			{
				tween = DOTween.To(() => 255f, delegate(float x)
				{
					alphaValue = x;
					currentColor = new Color(1f, 1f, 1f, alphaValue / 255f);
					spriteRenderer.color = currentColor;
				}, 0f, fadeTime).SetEase(Ease.Linear);
			}
		}

		public void SetNormalColor()
		{
			tween.Kill();
			spriteRenderer.color = colorOrigin;
		}

		public void SetNormalSprite()
		{
			base.EnemyModel.EnemyAnimationController.TurnOnAnimator();
			spriteRenderer.sprite = normalSprite;
		}

		public void RemoveAllFXs()
		{
			StopAllCoroutines();
			foreach (EffectController item in listEffectsOnEnemy)
			{
				StartCoroutine(RemoveFX(item, 0f));
			}
		}

		private IEnumerator RemoveFX(EffectController effect, float delayTime)
		{
			yield return new WaitForSeconds(delayTime);
			listEffectsOnEnemy.Remove(effect);
			effect.ReturnPool();
		}
	}
}
