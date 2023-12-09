using System.Collections.Generic;

namespace Parameter
{
	public class PowerUpItemParameter : Singleton<PowerUpItemParameter>
	{
		private List<PowerUpItem> listWeapon = new List<PowerUpItem>();

		private bool CheckId(int puItemId)
		{
			return puItemId >= 0 && puItemId < listWeapon.Count;
		}

		public void SetWeaponParameter(PowerUpItem powerUpItem)
		{
			int count = listWeapon.Count;
			if (count <= powerUpItem.id)
			{
				listWeapon.Add(powerUpItem);
			}
		}

		public int GetPrice(int puItemId)
		{
			if (puItemId < listWeapon.Count && puItemId >= 0)
			{
				PowerUpItem powerUpItem = listWeapon[puItemId];
				return powerUpItem.priceToBuy;
			}
			return 0;
		}

		public int GetCooldownTime(int puItemId)
		{
			if (puItemId < listWeapon.Count && puItemId >= 0)
			{
				PowerUpItem powerUpItem = listWeapon[puItemId];
				return powerUpItem.cooldownTime;
			}
			return 0;
		}

		public int GetWeaponActivationTime(int puItemId)
		{
			if (puItemId < listWeapon.Count && puItemId >= 0)
			{
				PowerUpItem powerUpItem = listWeapon[puItemId];
				return powerUpItem.activationTime;
			}
			return 0;
		}

		public int[] GetCustomValue(int puItemId)
		{
			if (puItemId < listWeapon.Count && puItemId >= 0)
			{
				PowerUpItem powerUpItem = listWeapon[puItemId];
				return powerUpItem.customValues;
			}
			return null;
		}

		public PowerUpItem GetWeapon(int puItemId)
		{
			return listWeapon[puItemId];
		}
	}
}
