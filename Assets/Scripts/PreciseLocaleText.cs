using UnityEngine;
using UnityEngine.UI;

public class PreciseLocaleText : MonoBehaviour
{
	private void Start()
	{
		GetComponent<Text>().text = $"LANGUAGE ID: {PreciseLocale.GetLanguageID()} \nLANGUAGE: {PreciseLocale.GetLanguage()} \n REGION: {PreciseLocale.GetRegion()} \n CURRENCY CODE: {PreciseLocale.GetCurrencyCode()} \n CURRENCY SYMBOL: {PreciseLocale.GetCurrencySymbol()}";
	}
}
