using DG.Tweening.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DG.Tweening
{
	[AddComponentMenu("DOTween/DOTween Animation")]
	public class DOTweenAnimation : ABSAnimationComponent
	{
		public float delay;

		public float duration = 1f;

		public Ease easeType = Ease.OutQuad;

		public AnimationCurve easeCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

		public LoopType loopType;

		public int loops = 1;

		public string id = string.Empty;

		public bool isRelative;

		public bool isFrom;

		public bool isIndependentUpdate;

		public bool autoKill = true;

		public bool isActive = true;

		public bool isValid;

		public Component target;

		public DOTweenAnimationType animationType;

		public TargetType targetType;

		public TargetType forcedTargetType;

		public bool autoPlay = true;

		public bool useTargetAsV3;

		public float endValueFloat;

		public Vector3 endValueV3;

		public Vector2 endValueV2;

		public Color endValueColor = new Color(1f, 1f, 1f, 1f);

		public string endValueString = string.Empty;

		public Rect endValueRect = new Rect(0f, 0f, 0f, 0f);

		public Transform endValueTransform;

		public bool optionalBool0;

		public float optionalFloat0;

		public int optionalInt0;

		public RotateMode optionalRotationMode;

		public ScrambleMode optionalScrambleMode;

		public string optionalString;

		private bool _tweenCreated;

		private int _playCount = -1;

		private void Awake()
		{
			if (isActive && isValid && (animationType != DOTweenAnimationType.Move || !useTargetAsV3))
			{
				CreateTween();
				_tweenCreated = true;
			}
		}

		private void Start()
		{
			if (!_tweenCreated)
			{
				CreateTween();
				_tweenCreated = true;
			}
		}

		private void OnDestroy()
		{
			if (tween != null && tween.IsActive())
			{
				tween.Kill();
			}
			tween = null;
		}

		public void CreateTween()
		{
			if (target == null)
			{
				UnityEngine.Debug.LogWarning($"{base.gameObject.name} :: This tween's target is NULL, because the animation was created with a DOTween Pro version older than 0.9.255. To fix this, exit Play mode then simply select this object, and it will update automatically", base.gameObject);
				return;
			}
			if (forcedTargetType != 0)
			{
				this.targetType = forcedTargetType;
			}
			if (this.targetType == TargetType.Unset)
			{
				this.targetType = TypeToDOTargetType(target.GetType());
			}
			switch (animationType)
			{
			case DOTweenAnimationType.Move:
				if (useTargetAsV3)
				{
					isRelative = false;
					if (endValueTransform == null)
					{
						UnityEngine.Debug.LogWarning($"{base.gameObject.name} :: This tween's TO target is NULL, a Vector3 of (0,0,0) will be used instead", base.gameObject);
						endValueV3 = Vector3.zero;
					}
					else if (this.targetType == TargetType.RectTransform)
					{
						RectTransform rectTransform = endValueTransform as RectTransform;
						if (rectTransform == null)
						{
							UnityEngine.Debug.LogWarning($"{base.gameObject.name} :: This tween's TO target should be a RectTransform, a Vector3 of (0,0,0) will be used instead", base.gameObject);
							endValueV3 = Vector3.zero;
						}
						else
						{
							RectTransform rectTransform2 = target as RectTransform;
							if (rectTransform2 == null)
							{
								UnityEngine.Debug.LogWarning($"{base.gameObject.name} :: This tween's target and TO target are not of the same type. Please reassign the values", base.gameObject);
							}
							else
							{
								endValueV3 = DOTweenUtils46.SwitchToRectTransform(rectTransform, rectTransform2);
							}
						}
					}
					else
					{
						endValueV3 = endValueTransform.position;
					}
				}
				switch (this.targetType)
				{
				case TargetType.RectTransform:
					tween = ((RectTransform)target).DOAnchorPos3D(endValueV3, duration, optionalBool0);
					break;
				case TargetType.Transform:
					tween = ((Transform)target).DOMove(endValueV3, duration, optionalBool0);
					break;
				case TargetType.Rigidbody2D:
					tween = ((Rigidbody2D)target).DOMove(endValueV3, duration, optionalBool0);
					break;
				case TargetType.Rigidbody:
					tween = ((Rigidbody)target).DOMove(endValueV3, duration, optionalBool0);
					break;
				}
				break;
			case DOTweenAnimationType.LocalMove:
				tween = base.transform.DOLocalMove(endValueV3, duration, optionalBool0);
				break;
			case DOTweenAnimationType.Rotate:
				switch (this.targetType)
				{
				case TargetType.Transform:
					tween = ((Transform)target).DORotate(endValueV3, duration, optionalRotationMode);
					break;
				case TargetType.Rigidbody2D:
					tween = ((Rigidbody2D)target).DORotate(endValueFloat, duration);
					break;
				case TargetType.Rigidbody:
					tween = ((Rigidbody)target).DORotate(endValueV3, duration, optionalRotationMode);
					break;
				}
				break;
			case DOTweenAnimationType.LocalRotate:
				tween = base.transform.DOLocalRotate(endValueV3, duration, optionalRotationMode);
				break;
			case DOTweenAnimationType.Scale:
				tween = base.transform.DOScale((!optionalBool0) ? endValueV3 : new Vector3(endValueFloat, endValueFloat, endValueFloat), duration);
				break;
			case DOTweenAnimationType.UIWidthHeight:
				tween = ((RectTransform)target).DOSizeDelta((!optionalBool0) ? endValueV2 : new Vector2(endValueFloat, endValueFloat), duration);
				break;
			case DOTweenAnimationType.Color:
				isRelative = false;
				switch (this.targetType)
				{
				case TargetType.SpriteRenderer:
					tween = ((SpriteRenderer)target).DOColor(endValueColor, duration);
					break;
				case TargetType.Renderer:
					tween = ((Renderer)target).material.DOColor(endValueColor, duration);
					break;
				case TargetType.Image:
					tween = ((Image)target).DOColor(endValueColor, duration);
					break;
				case TargetType.Text:
					tween = ((Text)target).DOColor(endValueColor, duration);
					break;
				case TargetType.Light:
					tween = ((Light)target).DOColor(endValueColor, duration);
					break;
				}
				break;
			case DOTweenAnimationType.Fade:
				isRelative = false;
				switch (this.targetType)
				{
				case TargetType.SpriteRenderer:
					tween = ((SpriteRenderer)target).DOFade(endValueFloat, duration);
					break;
				case TargetType.Renderer:
					tween = ((Renderer)target).material.DOFade(endValueFloat, duration);
					break;
				case TargetType.Image:
					tween = ((Image)target).DOFade(endValueFloat, duration);
					break;
				case TargetType.Text:
					tween = ((Text)target).DOFade(endValueFloat, duration);
					break;
				case TargetType.Light:
					tween = ((Light)target).DOIntensity(endValueFloat, duration);
					break;
				case TargetType.CanvasGroup:
					tween = ((CanvasGroup)target).DOFade(endValueFloat, duration);
					break;
				}
				break;
			case DOTweenAnimationType.Text:
			{
				TargetType targetType = this.targetType;
				if (targetType == TargetType.Text)
				{
					tween = ((Text)target).DOText(endValueString, duration, optionalBool0, optionalScrambleMode, optionalString);
				}
				break;
			}
			case DOTweenAnimationType.PunchPosition:
				switch (this.targetType)
				{
				case TargetType.RectTransform:
					tween = ((RectTransform)target).DOPunchAnchorPos(endValueV3, duration, optionalInt0, optionalFloat0, optionalBool0);
					break;
				case TargetType.Transform:
					tween = ((Transform)target).DOPunchPosition(endValueV3, duration, optionalInt0, optionalFloat0, optionalBool0);
					break;
				}
				break;
			case DOTweenAnimationType.PunchScale:
				tween = base.transform.DOPunchScale(endValueV3, duration, optionalInt0, optionalFloat0);
				break;
			case DOTweenAnimationType.PunchRotation:
				tween = base.transform.DOPunchRotation(endValueV3, duration, optionalInt0, optionalFloat0);
				break;
			case DOTweenAnimationType.ShakePosition:
				switch (this.targetType)
				{
				case TargetType.RectTransform:
					tween = ((RectTransform)target).DOShakeAnchorPos(duration, endValueV3, optionalInt0, optionalFloat0, optionalBool0);
					break;
				case TargetType.Transform:
					tween = ((Transform)target).DOShakePosition(duration, endValueV3, optionalInt0, optionalFloat0, optionalBool0);
					break;
				}
				break;
			case DOTweenAnimationType.ShakeScale:
				tween = base.transform.DOShakeScale(duration, endValueV3, optionalInt0, optionalFloat0);
				break;
			case DOTweenAnimationType.ShakeRotation:
				tween = base.transform.DOShakeRotation(duration, endValueV3, optionalInt0, optionalFloat0);
				break;
			case DOTweenAnimationType.CameraAspect:
				tween = ((Camera)target).DOAspect(endValueFloat, duration);
				break;
			case DOTweenAnimationType.CameraBackgroundColor:
				tween = ((Camera)target).DOColor(endValueColor, duration);
				break;
			case DOTweenAnimationType.CameraFieldOfView:
				tween = ((Camera)target).DOFieldOfView(endValueFloat, duration);
				break;
			case DOTweenAnimationType.CameraOrthoSize:
				tween = ((Camera)target).DOOrthoSize(endValueFloat, duration);
				break;
			case DOTweenAnimationType.CameraPixelRect:
				tween = ((Camera)target).DOPixelRect(endValueRect, duration);
				break;
			case DOTweenAnimationType.CameraRect:
				tween = ((Camera)target).DORect(endValueRect, duration);
				break;
			}
			if (tween == null)
			{
				return;
			}
			if (isFrom)
			{
				((Tweener)tween).From(isRelative);
			}
			else
			{
				tween.SetRelative(isRelative);
			}
			tween.SetTarget(base.gameObject).SetDelay(delay).SetLoops(loops, loopType)
				.SetAutoKill(autoKill)
				.OnKill(delegate
				{
					tween = null;
				});
			if (isSpeedBased)
			{
				tween.SetSpeedBased();
			}
			if (easeType == Ease.INTERNAL_Custom)
			{
				tween.SetEase(easeCurve);
			}
			else
			{
				tween.SetEase(easeType);
			}
			if (!string.IsNullOrEmpty(id))
			{
				tween.SetId(id);
			}
			tween.SetUpdate(isIndependentUpdate);
			if (hasOnStart)
			{
				if (onStart != null)
				{
					tween.OnStart(onStart.Invoke);
				}
			}
			else
			{
				onStart = null;
			}
			if (hasOnPlay)
			{
				if (onPlay != null)
				{
					tween.OnPlay(onPlay.Invoke);
				}
			}
			else
			{
				onPlay = null;
			}
			if (hasOnUpdate)
			{
				if (onUpdate != null)
				{
					tween.OnUpdate(onUpdate.Invoke);
				}
			}
			else
			{
				onUpdate = null;
			}
			if (hasOnStepComplete)
			{
				if (onStepComplete != null)
				{
					tween.OnStepComplete(onStepComplete.Invoke);
				}
			}
			else
			{
				onStepComplete = null;
			}
			if (hasOnComplete)
			{
				if (onComplete != null)
				{
					tween.OnComplete(onComplete.Invoke);
				}
			}
			else
			{
				onComplete = null;
			}
			if (autoPlay)
			{
				tween.Play();
			}
			else
			{
				tween.Pause();
			}
			if (hasOnTweenCreated && onTweenCreated != null)
			{
				onTweenCreated.Invoke();
			}
		}

		public override void DOPlay()
		{
			DOTween.Play(base.gameObject);
		}

		public override void DOPlayBackwards()
		{
			DOTween.PlayBackwards(base.gameObject);
		}

		public override void DOPlayForward()
		{
			DOTween.PlayForward(base.gameObject);
		}

		public override void DOPause()
		{
			DOTween.Pause(base.gameObject);
		}

		public override void DOTogglePause()
		{
			DOTween.TogglePause(base.gameObject);
		}

		public override void DORewind()
		{
			_playCount = -1;
			DOTweenAnimation[] components = base.gameObject.GetComponents<DOTweenAnimation>();
			for (int num = components.Length - 1; num > -1; num--)
			{
				Tween tween = components[num].tween;
				if (tween != null && tween.IsInitialized())
				{
					components[num].tween.Rewind();
				}
			}
		}

		public override void DORestart(bool fromHere = false)
		{
			_playCount = -1;
			if (tween == null)
			{
				if (Debugger.logPriority > 1)
				{
					Debugger.LogNullTween(tween);
				}
				return;
			}
			if (fromHere && isRelative)
			{
				ReEvaluateRelativeTween();
			}
			DOTween.Restart(base.gameObject);
		}

		public override void DOComplete()
		{
			DOTween.Complete(base.gameObject);
		}

		public override void DOKill()
		{
			DOTween.Kill(base.gameObject);
			tween = null;
		}

		public void DOPlayById(string id)
		{
			DOTween.Play(base.gameObject, id);
		}

		public void DOPlayAllById(string id)
		{
			DOTween.Play(id);
		}

		public void DOPlayBackwardsById(string id)
		{
			DOTween.PlayBackwards(base.gameObject, id);
		}

		public void DOPlayBackwardsAllById(string id)
		{
			DOTween.PlayBackwards(id);
		}

		public void DOPlayForwardById(string id)
		{
			DOTween.PlayForward(base.gameObject, id);
		}

		public void DOPlayForwardAllById(string id)
		{
			DOTween.PlayForward(id);
		}

		public void DOPlayNext()
		{
			DOTweenAnimation[] components = GetComponents<DOTweenAnimation>();
			DOTweenAnimation dOTweenAnimation;
			do
			{
				if (_playCount < components.Length - 1)
				{
					_playCount++;
					dOTweenAnimation = components[_playCount];
					continue;
				}
				return;
			}
			while (!(dOTweenAnimation != null) || dOTweenAnimation.tween == null || dOTweenAnimation.tween.IsPlaying() || dOTweenAnimation.tween.IsComplete());
			dOTweenAnimation.tween.Play();
		}

		public void DORewindAndPlayNext()
		{
			_playCount = -1;
			DOTween.Rewind(base.gameObject);
			DOPlayNext();
		}

		public void DORestartById(string id)
		{
			_playCount = -1;
			DOTween.Restart(base.gameObject, id);
		}

		public void DORestartAllById(string id)
		{
			_playCount = -1;
			DOTween.Restart(id);
		}

		public List<Tween> GetTweens()
		{
			List<Tween> list = new List<Tween>();
			DOTweenAnimation[] components = GetComponents<DOTweenAnimation>();
			DOTweenAnimation[] array = components;
			foreach (DOTweenAnimation dOTweenAnimation in array)
			{
				list.Add(dOTweenAnimation.tween);
			}
			return list;
		}

		public static TargetType TypeToDOTargetType(Type t)
		{
			string text = t.ToString();
			int num = text.LastIndexOf(".");
			if (num != -1)
			{
				text = text.Substring(num + 1);
			}
			if (text.IndexOf("Renderer") != -1 && text != "SpriteRenderer")
			{
				text = "Renderer";
			}
			return (TargetType)Enum.Parse(typeof(TargetType), text);
		}

		private void ReEvaluateRelativeTween()
		{
			if (animationType == DOTweenAnimationType.Move)
			{
				((Tweener)tween).ChangeEndValue(base.transform.position + endValueV3, snapStartValue: true);
			}
			else if (animationType == DOTweenAnimationType.LocalMove)
			{
				((Tweener)tween).ChangeEndValue(base.transform.localPosition + endValueV3, snapStartValue: true);
			}
		}
	}
}
