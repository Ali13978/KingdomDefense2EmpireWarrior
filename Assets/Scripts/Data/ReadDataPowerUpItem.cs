using Parameter;
using UnityEngine;

namespace Data
{
	public class ReadDataPowerUpItem : MonoBehaviour
	{
		private void Start()
		{
			ReadWeaponParameter();
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv không tồn tại hoặc dữ liệu trong file không đúng định dạng.");
		}

		private void ReadWeaponParameter()
		{
			ItemConfig itemConfig = CommonData.Instance.itemConfig;
			for (int i = 0; i < itemConfig.dataArray.Length; i++)
			{
				if (itemConfig.dataArray[i].Price_to_buy > 0)
				{
					ItemConfigData itemConfigData = itemConfig.dataArray[i];
					PowerUpItem weaponParameter = default(PowerUpItem);
					weaponParameter.id = itemConfigData.Id;
					weaponParameter.name = itemConfigData.Name;
					weaponParameter.priceToBuy = itemConfigData.Price_to_buy;
					weaponParameter.cooldownTime = itemConfigData.Time_cooldown;
					weaponParameter.activationTime = itemConfigData.Activation_time;
					weaponParameter.customValues = itemConfigData.Customvalues;
					Singleton<PowerUpItemParameter>.Instance.SetWeaponParameter(weaponParameter);
				}
			}
		}
	}
}
