using Gameplay;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextTips : MonoBehaviour
{
	public Text textTipShow;

	private string textTipContent;

	public void GetContentTextTip()
	{
		int mapID = SingletonMonoBehaviour<GameData>.Instance.MapID;
		textTipContent = Singleton<TextTipParameter>.Instance.GetRandomTextTipContent(mapID);
	}

	public void ShowContentTextTip()
	{
		textTipShow.text = textTipContent;
	}

	public void EnableTextTip()
	{
		textTipShow.enabled = true;
	}

	public void DisableTextTip()
	{
		textTipShow.enabled = false;
	}
}
