using System;
using UnityEngine;

[Serializable]
public class PetConfigData
{
	[SerializeField]
	private int id;

	[SerializeField]
	private string petname;

	[SerializeField]
	private int atk_cooldown;

	[SerializeField]
	private int atk_physics_min;

	[SerializeField]
	private int atk_physics_max;

	[SerializeField]
	private int atk_magic_min;

	[SerializeField]
	private int atk_magic_max;

	[SerializeField]
	private int atk_range_min;

	[SerializeField]
	private int atk_range_avg;

	[SerializeField]
	private int atk_range_max;

	[SerializeField]
	private int can_attack_air;

	[SerializeField]
	private int respawn_time;

	[SerializeField]
	private int health_regen;

	[SerializeField]
	private int health_regen_cooldown;

	[SerializeField]
	private int health;

	[SerializeField]
	private int armor_physics;

	[SerializeField]
	private int armor_magic;

	[SerializeField]
	private int[] skillvalues = new int[0];

	[SerializeField]
	private int speed;

	[SerializeField]
	private string desc;

	[SerializeField]
	private int[] desc_values = new int[0];

	[SerializeField]
	private int is_available;

	[SerializeField]
	private int price;

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

	public string Petname
	{
		get
		{
			return petname;
		}
		set
		{
			petname = value;
		}
	}

	public int Atk_cooldown
	{
		get
		{
			return atk_cooldown;
		}
		set
		{
			atk_cooldown = value;
		}
	}

	public int Atk_physics_min
	{
		get
		{
			return atk_physics_min;
		}
		set
		{
			atk_physics_min = value;
		}
	}

	public int Atk_physics_max
	{
		get
		{
			return atk_physics_max;
		}
		set
		{
			atk_physics_max = value;
		}
	}

	public int Atk_magic_min
	{
		get
		{
			return atk_magic_min;
		}
		set
		{
			atk_magic_min = value;
		}
	}

	public int Atk_magic_max
	{
		get
		{
			return atk_magic_max;
		}
		set
		{
			atk_magic_max = value;
		}
	}

	public int Atk_range_min
	{
		get
		{
			return atk_range_min;
		}
		set
		{
			atk_range_min = value;
		}
	}

	public int Atk_range_avg
	{
		get
		{
			return atk_range_avg;
		}
		set
		{
			atk_range_avg = value;
		}
	}

	public int Atk_range_max
	{
		get
		{
			return atk_range_max;
		}
		set
		{
			atk_range_max = value;
		}
	}

	public int Can_attack_air
	{
		get
		{
			return can_attack_air;
		}
		set
		{
			can_attack_air = value;
		}
	}

	public int Respawn_time
	{
		get
		{
			return respawn_time;
		}
		set
		{
			respawn_time = value;
		}
	}

	public int Health_regen
	{
		get
		{
			return health_regen;
		}
		set
		{
			health_regen = value;
		}
	}

	public int Health_regen_cooldown
	{
		get
		{
			return health_regen_cooldown;
		}
		set
		{
			health_regen_cooldown = value;
		}
	}

	public int Health
	{
		get
		{
			return health;
		}
		set
		{
			health = value;
		}
	}

	public int Armor_physics
	{
		get
		{
			return armor_physics;
		}
		set
		{
			armor_physics = value;
		}
	}

	public int Armor_magic
	{
		get
		{
			return armor_magic;
		}
		set
		{
			armor_magic = value;
		}
	}

	public int[] Skillvalues
	{
		get
		{
			return skillvalues;
		}
		set
		{
			skillvalues = value;
		}
	}

	public int Speed
	{
		get
		{
			return speed;
		}
		set
		{
			speed = value;
		}
	}

	public string Desc
	{
		get
		{
			return desc;
		}
		set
		{
			desc = value;
		}
	}

	public int[] Desc_values
	{
		get
		{
			return desc_values;
		}
		set
		{
			desc_values = value;
		}
	}

	public int Is_available
	{
		get
		{
			return is_available;
		}
		set
		{
			is_available = value;
		}
	}

	public int Price
	{
		get
		{
			return price;
		}
		set
		{
			price = value;
		}
	}
}
