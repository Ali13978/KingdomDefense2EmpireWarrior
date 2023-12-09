using System;
using UnityEngine;

[Serializable]
public class DailyRewardConfigData
{
	[SerializeField]
	private int id;

	[SerializeField]
	private RewardType rewardtype;

	[SerializeField]
	private int rewardid;

	[SerializeField]
	private int rewardquantity;

	[SerializeField]
	private BonusType bonustype;

	[SerializeField]
	private int bonusid;

	[SerializeField]
	private int bonusquantity;

	public int Id
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
		}
	}

	public RewardType REWARDTYPE
	{
		get
		{
			return rewardtype;
		}
		set
		{
			rewardtype = value;
		}
	}

	public int Rewardid
	{
		get
		{
			return rewardid;
		}
		set
		{
			rewardid = value;
		}
	}

	public int Rewardquantity
	{
		get
		{
			return rewardquantity;
		}
		set
		{
			rewardquantity = value;
		}
	}

	public BonusType BONUSTYPE
	{
		get
		{
			return bonustype;
		}
		set
		{
			bonustype = value;
		}
	}

	public int Bonusid
	{
		get
		{
			return bonusid;
		}
		set
		{
			bonusid = value;
		}
	}

	public int Bonusquantity
	{
		get
		{
			return bonusquantity;
		}
		set
		{
			bonusquantity = value;
		}
	}
}
