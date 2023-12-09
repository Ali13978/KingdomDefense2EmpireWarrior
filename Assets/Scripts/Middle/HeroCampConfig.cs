namespace Middle
{
	public class HeroCampConfig
	{
		private static HeroCampConfig instance;

		private int health_max;

		private int attack_damage_max;

		private int physics_armor_max;

		private int attack_speed_max;

		private int magic_armor_max;

		private int health_regen_max;

		private int movement_speed_max;

		public static HeroCampConfig Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new HeroCampConfig();
				}
				return instance;
			}
		}

		public int Health_max
		{
			get
			{
				return health_max;
			}
			set
			{
				health_max = value;
			}
		}

		public int Attack_damage_max
		{
			get
			{
				return attack_damage_max;
			}
			set
			{
				attack_damage_max = value;
			}
		}

		public int Physics_armor_max
		{
			get
			{
				return physics_armor_max;
			}
			set
			{
				physics_armor_max = value;
			}
		}

		public int Attack_speed_max
		{
			get
			{
				return attack_speed_max;
			}
			set
			{
				attack_speed_max = value;
			}
		}

		public int Magic_armor_max
		{
			get
			{
				return magic_armor_max;
			}
			set
			{
				magic_armor_max = value;
			}
		}

		public int Health_regen_max
		{
			get
			{
				return health_regen_max;
			}
			set
			{
				health_regen_max = value;
			}
		}

		public int Movement_speed_max
		{
			get
			{
				return movement_speed_max;
			}
			set
			{
				movement_speed_max = value;
			}
		}
	}
}
