using System;
using UnityEngine;

[Serializable]
public class ItemConfigData
{
	[SerializeField]
	private int id;

	[SerializeField]
	private string name;

	[SerializeField]
	private int price_to_buy;

	[SerializeField]
	private int time_cooldown;

	[SerializeField]
	private int activation_time;

	[SerializeField]
	private int[] customvalues = new int[0];

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

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public int Price_to_buy
	{
		get
		{
			return price_to_buy;
		}
		set
		{
			price_to_buy = value;
		}
	}

	public int Time_cooldown
	{
		get
		{
			return time_cooldown;
		}
		set
		{
			time_cooldown = value;
		}
	}

	public int Activation_time
	{
		get
		{
			return activation_time;
		}
		set
		{
			activation_time = value;
		}
	}

	public int[] Customvalues
	{
		get
		{
			return customvalues;
		}
		set
		{
			customvalues = value;
		}
	}
}
