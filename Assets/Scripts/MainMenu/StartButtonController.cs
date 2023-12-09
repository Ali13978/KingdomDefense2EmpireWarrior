using System.Collections;
using UnityEngine;

namespace MainMenu
{
	public class StartButtonController : ButtonController
	{
		[SerializeField]
		private float timeToOpen;

		public override void OnClick()
		{
			base.OnClick();
			StartCoroutine(StartGame());
		}

		private IEnumerator StartGame()
		{
			yield return new WaitForSeconds(timeToOpen);
			Loading.Instance.ShowLoading();
			yield return new WaitForSeconds(1f);
			VideoPlayerManager.Instance.TryToShowInterstitialAds_Loading();
			GameApplication.Instance.LoadScene(GameApplication.WorldMapSceneName);
		}
	}
}
