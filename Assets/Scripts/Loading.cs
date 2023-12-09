using DG.Tweening;
using Middle;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
	[SerializeField]
	private GameObject leftDoor;

	[SerializeField]
	private GameObject rightDoor;

	[SerializeField]
	private GameObject leftLightning;

	[SerializeField]
	private GameObject rightLightning;

	[SerializeField]
	private float timeToClose;

	[SerializeField]
	private float timeToOpen;

	private bool isLoading;

	[SerializeField]
	private ShowTextTips showTextTips;

	private float timeBeforeLoading;

	private float timeAfterLoading;

	public bool IsLoading
	{
		get
		{
			return isLoading;
		}
		set
		{
			isLoading = value;
		}
	}

	public static Loading Instance
	{
		get;
		set;
	}

	private void Awake()
	{
		if ((bool)Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		IsLoading = false;
		SceneManager.sceneLoaded += SceneManager_sceneLoaded;
		showTextTips.DisableTextTip();
		Screen.sleepTimeout = -1;
	}

	public void LoadSceneCompleted()
	{
		if (MiddleDelivery.Instance.OpenSceneCount > 0)
		{
			UnityEngine.Debug.Log("da load scene, giai phong resources");
			Resources.UnloadUnusedAssets();
			StartCoroutine(IEOnReloadComplete());
		}
		MiddleDelivery.Instance.IncreaseOpenSceneCount();
	}

	private void SceneManager_sceneLoaded(Scene loadedScene, LoadSceneMode loadMode)
	{
		if (GameApplication.Instance.GetCurrentSceneName() != GameApplication.GameplaySceneName)
		{
			LoadSceneCompleted();
		}
	}

	public void ShowLoading()
	{
		if (!IsLoading)
		{
			StopAllCoroutines();
			leftDoor.transform.DORestart();
			leftDoor.transform.DOLocalMoveX(-320f, timeToClose).SetEase(Ease.OutQuad);
			rightDoor.transform.DORestart();
			rightDoor.transform.DOLocalMoveX(320f, timeToClose).SetEase(Ease.OutQuad).OnComplete(OnShowLoadComplete);
			IsLoading = true;
			UISoundManager.Instance.PlayOpenLoading();
			timeBeforeLoading = Time.time;
		}
	}

	private void OnShowLoadComplete()
	{
		ShowLight();
		showTextTips.EnableTextTip();
		showTextTips.GetContentTextTip();
		showTextTips.ShowContentTextTip();
	}

	public void HideLoading()
	{
		leftDoor.transform.DOLocalMoveX(-960f, timeToOpen).SetEase(Ease.InCubic);
		rightDoor.transform.DOLocalMoveX(960f, timeToOpen).SetEase(Ease.InCubic).OnComplete(OnHideLoadComplete);
		UISoundManager.Instance.PlayCloseLoading();
	}

	private void OnHideLoadComplete()
	{
		timeAfterLoading = Time.time;
		int num = Mathf.RoundToInt((timeAfterLoading - timeBeforeLoading) * 1000f);
		IsLoading = false;
	}

	private void ShowLight()
	{
		leftLightning.SetActive(value: true);
		rightLightning.SetActive(value: true);
	}

	private void HideLight()
	{
		leftLightning.SetActive(value: false);
		rightLightning.SetActive(value: false);
	}

	private IEnumerator IEOnReloadComplete()
	{
		yield return new WaitForEndOfFrame();
		HideLoading();
		HideLight();
		showTextTips.DisableTextTip();
	}
}
