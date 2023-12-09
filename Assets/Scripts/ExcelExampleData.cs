using System;
using UnityEngine;

[Serializable]
public class ExcelExampleData
{
	[SerializeField]
	private int id;

	[SerializeField]
	private string name;

	[SerializeField]
	private float strength;

	[SerializeField]
	private Difficulty difficulty;

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

	public float Strength
	{
		get
		{
			return strength;
		}
		set
		{
			strength = value;
		}
	}

	public Difficulty DIFFICULTY
	{
		get
		{
			return difficulty;
		}
		set
		{
			difficulty = value;
		}
	}
}
