using GeneralVariable;
using Parameter;
using System.Collections;
using UnityEngine;

namespace LifetimePopup
{
	public class LoadingProgressPopupController : MonoBehaviour
	{
		public void Open()
		{
			base.gameObject.SetActive(value: true);
			StartCoroutine(CheckConnectionTimeOut());
		}

		private IEnumerator CheckConnectionTimeOut()
		{
			yield return new WaitForSeconds(GeneralVariable.GeneralVariable.CONNECTION_TIMEOUT);
			if (IsGameObjectActive())
			{
				Close();
				string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(150);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
		}

		public bool IsGameObjectActive()
		{
			return base.gameObject.activeSelf;
		}

		public void Close()
		{
			base.gameObject.SetActive(value: false);
			StopCoroutine(CheckConnectionTimeOut());
		}
	}
}
