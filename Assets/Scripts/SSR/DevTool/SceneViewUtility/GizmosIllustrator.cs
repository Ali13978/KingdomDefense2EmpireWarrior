using UnityEngine;

namespace SSR.DevTool.SceneViewUtility
{
	[ExecuteInEditMode]
	public abstract class GizmosIllustrator : MonoBehaviour
	{
		[SerializeField]
		private Color color = Color.green;

		[SerializeField]
		private bool alwaysDraw;

		[SerializeField]
		private Transform rootTransform;

		[SerializeField]
		private Transform targetTransform;

		private bool shouldDrawGizmos;

		protected Transform TargetTransform
		{
			get
			{
				return targetTransform;
			}
			set
			{
				targetTransform = value;
			}
		}

		protected abstract void DrawGizmos();

		private void Awake()
		{
		}

		private void Draw()
		{
			Color color = Gizmos.color;
			Gizmos.color = this.color;
			DrawGizmos();
			Gizmos.color = color;
		}

		private void OnSelectionChanged()
		{
			CheckShouldDrawGizmos();
		}

		private void CheckShouldDrawGizmos()
		{
		}

		public void OnDrawGizmos()
		{
			if (shouldDrawGizmos)
			{
				Draw();
			}
		}

		public void Reset()
		{
			TargetTransform = base.transform;
			rootTransform = base.transform;
		}

		protected void ForceDraw()
		{
		}
	}
}
