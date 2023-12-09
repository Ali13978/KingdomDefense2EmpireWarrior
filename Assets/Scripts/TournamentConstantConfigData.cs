using System;
using UnityEngine;

[Serializable]
public class TournamentConstantConfigData
{
	[SerializeField]
	private int leagueindex;

	[SerializeField]
	private float jfactor;

	[SerializeField]
	private int initgemquantity;

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

	public float Jfactor
	{
		get
		{
			return jfactor;
		}
		set
		{
			jfactor = value;
		}
	}

	public int Initgemquantity
	{
		get
		{
			return initgemquantity;
		}
		set
		{
			initgemquantity = value;
		}
	}
}
