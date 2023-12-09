using Data;
using Parameter;
using UnityEngine;

public static class HeroesLevelGemCalculator
{
	private static float expToGemRatio = 0.5f;

	public static int GetGemAmountToLevelUp(int heroID)
	{
		int num = -1;
		return Mathf.RoundToInt((float)ReadWriteDataHero.Instance.GetExpToLevelUp(heroID) * expToGemRatio);
	}

	public static bool IsEnoughGemToUpgrade(int heroID)
	{
		bool flag = false;
		if (ReadWriteDataHero.Instance.IsReachMaxLevel(heroID))
		{
			return false;
		}
		return ReadWriteDataPlayerCurrency.Instance.GetCurrentGem() >= GetGemAmountToLevelUp(heroID);
	}

	public static int GetGemAmountToUnlockPet(int heroID)
	{
		int num = 0;
		int petID = HeroParameter.Instance.GetPetID(heroID);
		return CommonData.Instance.petConfig.dataArray[petID % 1000].Price;
	}

	public static bool IsEnoughGemToUnlockPet(int heroID)
	{
		bool flag = false;
		return ReadWriteDataPlayerCurrency.Instance.GetCurrentGem() >= GetGemAmountToUnlockPet(heroID);
	}
}
