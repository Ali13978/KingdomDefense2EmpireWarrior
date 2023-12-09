using System;
using UnityEngine;

[Serializable]
public class TdLocalizationData
{
	[SerializeField]
	private string key;

	[SerializeField]
	private string brazil;

	[SerializeField]
	private string en;

	[SerializeField]
	private string french;

	[SerializeField]
	private string german;

	[SerializeField]
	private string korean;

	[SerializeField]
	private string polish;

	[SerializeField]
	private string russian;

	[SerializeField]
	private string spanish;

	[SerializeField]
	private string vi;

	[SerializeField]
	private string chinese;

	[SerializeField]
	private string japanese;

	public string Key
	{
		get
		{
			return key;
		}
		set
		{
			key = value;
		}
	}

	public string Brazil
	{
		get
		{
			return brazil;
		}
		set
		{
			brazil = value;
		}
	}

	public string En
	{
		get
		{
			return en;
		}
		set
		{
			en = value;
		}
	}

	public string French
	{
		get
		{
			return french;
		}
		set
		{
			french = value;
		}
	}

	public string German
	{
		get
		{
			return german;
		}
		set
		{
			german = value;
		}
	}

	public string Korean
	{
		get
		{
			return korean;
		}
		set
		{
			korean = value;
		}
	}

	public string Polish
	{
		get
		{
			return polish;
		}
		set
		{
			polish = value;
		}
	}

	public string Russian
	{
		get
		{
			return russian;
		}
		set
		{
			russian = value;
		}
	}

	public string Spanish
	{
		get
		{
			return spanish;
		}
		set
		{
			spanish = value;
		}
	}

	public string Vi
	{
		get
		{
			return vi;
		}
		set
		{
			vi = value;
		}
	}

	public string Chinese
	{
		get
		{
			return chinese;
		}
		set
		{
			chinese = value;
		}
	}

	public string Japanese
	{
		get
		{
			return japanese;
		}
		set
		{
			japanese = value;
		}
	}
}
