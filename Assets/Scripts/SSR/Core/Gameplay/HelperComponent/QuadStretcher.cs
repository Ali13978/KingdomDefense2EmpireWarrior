using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class QuadStretcher : MonoBehaviour
	{
		[SerializeField]
		private Vector3 originalLocalPosition;

		[SerializeField]
		private float left;

		[SerializeField]
		private float right;

		[SerializeField]
		private float top;

		[SerializeField]
		private float bottom;

		public Vector4 Stretch
		{
			get
			{
				return new Vector4(left, right, top, bottom);
			}
			set
			{
				left = value.x;
				right = value.y;
				top = value.z;
				bottom = value.w;
			}
		}

		public Vector3 OriginalLocalPosition
		{
			get
			{
				return originalLocalPosition;
			}
			set
			{
				originalLocalPosition = value;
			}
		}

		public Vector3 OriginalWorldPosition => base.transform.TransformPoint(originalLocalPosition);

		public float Left
		{
			get
			{
				return left;
			}
			set
			{
				left = value;
			}
		}

		public float Right
		{
			get
			{
				return right;
			}
			set
			{
				right = value;
			}
		}

		public float Top
		{
			get
			{
				return top;
			}
			set
			{
				top = value;
			}
		}

		public float Bottom
		{
			get
			{
				return bottom;
			}
			set
			{
				bottom = value;
			}
		}

		public void UpdateTransform()
		{
			float num = Left + Right;
			float num2 = Top + Bottom;
			Vector3 vector = OriginalLocalPosition;
			float num3 = vector.x - Left + num / 2f;
			Vector3 vector2 = OriginalLocalPosition;
			float num4 = vector2.y - Bottom + num2 / 2f;
			Transform transform = base.transform;
			float x = num3;
			float y = num4;
			Vector3 vector3 = OriginalLocalPosition;
			transform.localPosition = new Vector3(x, y, vector3.z);
			base.transform.localScale = new Vector3(num, num2, 1f);
		}

		public void SetToZero()
		{
			Stretch = Vector4.zero;
		}

		public void LateUpdate()
		{
			UpdateTransform();
		}

		public void Reset()
		{
			originalLocalPosition = base.transform.localPosition;
			Vector3 localScale = base.transform.localScale;
			left = localScale.x / 2f;
			Vector3 localScale2 = base.transform.localScale;
			right = localScale2.x / 2f;
			Vector3 localScale3 = base.transform.localScale;
			top = localScale3.y / 2f;
			Vector3 localScale4 = base.transform.localScale;
			bottom = localScale4.y / 2f;
		}
	}
}
