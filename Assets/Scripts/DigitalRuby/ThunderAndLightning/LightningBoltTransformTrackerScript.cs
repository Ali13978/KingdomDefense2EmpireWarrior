using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningBoltTransformTrackerScript : MonoBehaviour
	{
		[Tooltip("The lightning script to track.")]
		public LightningBoltPrefabScript LightningScript;

		[Tooltip("The transform to track which will be where the bolts are emitted from.")]
		public Transform StartTarget;

		[Tooltip("(Optional) The transform to track which will be where the bolts are emitted to. If no end target is specified, lightning will simply move to stay on top of the start target.")]
		public Transform EndTarget;

		[SingleLine("Scaling limits.")]
		public RangeOfFloats ScaleLimit = new RangeOfFloats
		{
			Minimum = 0.1f,
			Maximum = 10f
		};

		private readonly Dictionary<Transform, LightningCustomTransformStateInfo> transformStartPositions = new Dictionary<Transform, LightningCustomTransformStateInfo>();

		private void Start()
		{
			if (LightningScript != null)
			{
				LightningScript.CustomTransformHandler.RemoveAllListeners();
				LightningScript.CustomTransformHandler.AddListener(CustomTransformHandler);
			}
		}

		private static float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
		{
			Vector2 normalized = (vec2 - vec1).normalized;
			return Vector2.Angle(Vector2.right, normalized) * Mathf.Sign(vec2.y - vec1.y);
		}

		private static void UpdateTransform(LightningCustomTransformStateInfo state, LightningBoltPrefabScript script, RangeOfFloats scaleLimit)
		{
			if (state.Transform == null || state.StartTransform == null)
			{
				return;
			}
			if (state.EndTransform == null)
			{
				state.Transform.position = state.StartTransform.position - state.BoltStartPosition;
				return;
			}
			Quaternion quaternion;
			if ((script.CameraMode == CameraMode.Auto && script.Camera.orthographic) || script.CameraMode == CameraMode.OrthographicXY)
			{
				float num = AngleBetweenVector2(state.BoltStartPosition, state.BoltEndPosition);
				float num2 = AngleBetweenVector2(state.StartTransform.position, state.EndTransform.position);
				quaternion = Quaternion.AngleAxis(num2 - num, Vector3.forward);
			}
			if (script.CameraMode == CameraMode.OrthographicXZ)
			{
				float num3 = AngleBetweenVector2(new Vector2(state.BoltStartPosition.x, state.BoltStartPosition.z), new Vector2(state.BoltEndPosition.x, state.BoltEndPosition.z));
				Vector3 position = state.StartTransform.position;
				float x = position.x;
				Vector3 position2 = state.StartTransform.position;
				Vector2 vec = new Vector2(x, position2.z);
				Vector3 position3 = state.EndTransform.position;
				float x2 = position3.x;
				Vector3 position4 = state.EndTransform.position;
				float num4 = AngleBetweenVector2(vec, new Vector2(x2, position4.z));
				quaternion = Quaternion.AngleAxis(num4 - num3, Vector3.up);
			}
			else
			{
				Quaternion rotation = Quaternion.LookRotation((state.BoltEndPosition - state.BoltStartPosition).normalized);
				Quaternion lhs = Quaternion.LookRotation((state.EndTransform.position - state.StartTransform.position).normalized);
				quaternion = lhs * Quaternion.Inverse(rotation);
			}
			state.Transform.rotation = quaternion;
			float num5 = Vector3.Distance(state.BoltStartPosition, state.BoltEndPosition);
			float num6 = Vector3.Distance(state.EndTransform.position, state.StartTransform.position);
			float num7 = Mathf.Clamp((!(num5 < Mathf.Epsilon)) ? (num6 / num5) : 1f, scaleLimit.Minimum, scaleLimit.Maximum);
			state.Transform.localScale = new Vector3(num7, num7, num7);
			Vector3 b = quaternion * (num7 * state.BoltStartPosition);
			state.Transform.position = state.StartTransform.position - b;
		}

		public void CustomTransformHandler(LightningCustomTransformStateInfo state)
		{
			if (base.enabled)
			{
				if (LightningScript == null)
				{
					UnityEngine.Debug.LogError("LightningScript property must be set to non-null.");
				}
				else if (state.State == LightningCustomTransformState.Executing)
				{
					UpdateTransform(state, LightningScript, ScaleLimit);
				}
				else if (state.State == LightningCustomTransformState.Started)
				{
					state.StartTransform = StartTarget;
					state.EndTransform = EndTarget;
					transformStartPositions[base.transform] = state;
				}
				else
				{
					transformStartPositions.Remove(base.transform);
				}
			}
		}
	}
}
