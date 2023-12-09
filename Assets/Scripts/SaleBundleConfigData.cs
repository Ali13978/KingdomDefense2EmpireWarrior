using System;
using UnityEngine;

[Serializable]
public class SaleBundleConfigData
{
	[SerializeField]
	private string id;

	[SerializeField]
	private string title;

	[SerializeField]
	private string productid;

	[SerializeField]
	private string bundletype;

	[SerializeField]
	private int timecountdown;

	[SerializeField]
	private int condition;

	[SerializeField]
	private int[] heroid = new int[0];

	[SerializeField]
	private int herolevel;

	[SerializeField]
	private bool havepet;

	[SerializeField]
	private int[] itemids = new int[0];

	[SerializeField]
	private int[] itemquatities = new int[0];

	[SerializeField]
	private int gembonus;

	[SerializeField]
	private float originalprice;

	[SerializeField]
	private float defaultprice;

	[SerializeField]
	private int saleoffpercent;

	[SerializeField]
	private int subcribedur;

	public string Id
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

	public string Title
	{
		get
		{
			return title;
		}
		set
		{
			title = value;
		}
	}

	public string Productid
	{
		get
		{
			return productid;
		}
		set
		{
			productid = value;
		}
	}

	public string Bundletype
	{
		get
		{
			return bundletype;
		}
		set
		{
			bundletype = value;
		}
	}

	public int Timecountdown
	{
		get
		{
			return timecountdown;
		}
		set
		{
			timecountdown = value;
		}
	}

	public int Condition
	{
		get
		{
			return condition;
		}
		set
		{
			condition = value;
		}
	}

	public int[] Heroid
	{
		get
		{
			return heroid;
		}
		set
		{
			heroid = value;
		}
	}

	public int Herolevel
	{
		get
		{
			return herolevel;
		}
		set
		{
			herolevel = value;
		}
	}

	public bool Havepet
	{
		get
		{
			return havepet;
		}
		set
		{
			havepet = value;
		}
	}

	public int[] Itemids
	{
		get
		{
			return itemids;
		}
		set
		{
			itemids = value;
		}
	}

	public int[] Itemquatities
	{
		get
		{
			return itemquatities;
		}
		set
		{
			itemquatities = value;
		}
	}

	public int Gembonus
	{
		get
		{
			return gembonus;
		}
		set
		{
			gembonus = value;
		}
	}

	public float Originalprice
	{
		get
		{
			return originalprice;
		}
		set
		{
			originalprice = value;
		}
	}

	public float Defaultprice
	{
		get
		{
			return defaultprice;
		}
		set
		{
			defaultprice = value;
		}
	}

	public int Saleoffpercent
	{
		get
		{
			return saleoffpercent;
		}
		set
		{
			saleoffpercent = value;
		}
	}

	public int Subcribedur
	{
		get
		{
			return subcribedur;
		}
		set
		{
			subcribedur = value;
		}
	}
}
