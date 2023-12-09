using UnityEngine;
using UnityEngine.UI;

public class AutoSetVersionName : MonoBehaviour
{
	[SerializeField]
	private Text versionName;

	private void Start()
	{
		versionName.text = "v" + Application.version.ToString();
	}
}
