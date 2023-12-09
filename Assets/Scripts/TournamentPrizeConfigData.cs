using System;
using UnityEngine;

[Serializable]
public class TournamentPrizeConfigData
{
	[SerializeField]
	private int leagueindex;

	[SerializeField]
	private int rankrangelower;

	[SerializeField]
	private int rankrangeupper;

	[SerializeField]
	private string itemtypes;

	[SerializeField]
	private int[] itemquantities = new int[0];

	public int Leagueindex
	{
		get
		{
			return leagueindex;
		}
		set
		{
			leagueindex = value;
		}
	}

	public int Rankrangelower
	{
		get
		{
			return rankrangelower;
		}
		set
		{
			rankrangelower = value;
		}
	}

	public int Rankrangeupper
	{
		get
		{
			return rankrangeupper;
		}
		set
		{
			rankrangeupper = value;
		}
	}

	public string Itemtypes
	{
		get
		{
			return itemtypes;
		}
		set
		{
			itemtypes = value;
		}
	}

	public int[] Itemquantities
	{
		get
		{
			return itemquantities;
		}
		set
		{
			itemquantities = value;
		}
	}
}
