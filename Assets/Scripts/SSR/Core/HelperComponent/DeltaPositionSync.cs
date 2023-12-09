using System.Collections;
using UnityEngine;

namespace SSR.Core.HelperComponent
{
	[ExecuteInEditMode]
	public class DeltaPositionSync : MonoBehaviour
	{
		[Header("Source")]
		[SerializeField]
		private Transform sourceAnchor;

		[SerializeField]
		private Transform sourceObject;

		[Header("Target")]
		[SerializeField]
		private Transform targetAnchor;

		[SerializeField]
		private Transform targetObject;

		[Header("Option")]
		[SerializeField]
		private bool ignoreZ = true;

		private Transform oldSourceAnchor;

		private Transform oldSourceObject;

		private float transitionProgress = 1f;

		private Vector3 oldTargetPosition;

		public Transform SourceAnchor => sourceAnchor;

		public Transform SourceObject => sourceObject;

		public void LateUpdate()
		{
			Vector3 vector = SourceObject.position - SourceAnchor.position + targetAnchor.position;
			if (transitionProgress < 1f)
			{
				oldTargetPosition = oldSourceObject.position - oldSourceAnchor.position + targetAnchor.position;
				vector = Vector3.Lerp(oldTargetPosition, vector, transitionProgress);
			}
			if (ignoreZ)
			{
				Vector3 position = targetObject.position;
				vector.z = position.z;
			}
			targetObject.position = vector;
		}

		public void ChangeSources(Transform newSourceAnchor, Transform newTargetObject, float transitionTime = 0f)
		{
			oldSourceAnchor = sourceAnchor;
			oldSourceObject = sourceObject;
			sourceAnchor = newSourceAnchor;
			sourceObject = newTargetObject;
			if (transitionTime > 0f)
			{
				StartCoroutine(TransitToNewSources(transitionTime));
			}
			else
			{
				transitionProgress = 1f;
			}
		}

		private IEnumerator TransitToNewSources(float transitionDuration)
		{
			float currentTime = 0f;
			while (currentTime < transitionDuration)
			{
				currentTime = Mathf.MoveTowards(currentTime, transitionDuration, Time.deltaTime);
				transitionProgress = currentTime / transitionDuration;
				yield return new WaitForEndOfFrame();
			}
			yield return null;
		}

		public void OnValidate()
		{
		}
	}
}
