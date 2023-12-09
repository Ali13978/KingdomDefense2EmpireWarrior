using Firebase;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Services.PlatformSpecific.Android
{
	public class FirebaseConstructor : MonoBehaviour
	{
		[SerializeField]
		private DataCloudSaverAndroid dataCloudSaverAndroid;

		[SerializeField]
		private UserProfileAndroid userProfileAndroid;

		[SerializeField]
		private FirebaseAnalyticsAndroid firebaseAnalyticsAndroid;

		[SerializeField]
		private NotificationAndroid notificationAndroid;

		private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

		private void Start()
		{
			StartCoroutine(_InitializeFirebase());
		}

		private void InitializeFirebase()
		{
			dataCloudSaverAndroid.FirebaseInit();
			userProfileAndroid.FirebaseInit();
			firebaseAnalyticsAndroid.FirebaseInit();
			notificationAndroid.FirebaseInit();
		}

		private IEnumerator _InitializeFirebase()
		{
			yield return null;
			yield return null;
			FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(delegate(Task<DependencyStatus> task)
			{
				dependencyStatus = task.Result;
				if (dependencyStatus == DependencyStatus.Available)
				{
					InitializeFirebase();
					UnityEngine.Debug.Log("Inits Firebase services!");
				}
				else
				{
					UnityEngine.Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
				}
			});
		}
	}
}
