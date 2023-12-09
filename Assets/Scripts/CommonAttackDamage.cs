using Gameplay;

public class CommonAttackDamage
{
	public TargetType targetType;

	public int physicsDamage;

	public int magicDamage;

	public int criticalStrikeChance;

	public bool isCrit;

	public int instantKillChance;

	public bool isInstantKill;

	public int ignoreArmorChance;

	public bool isIgnoreArmor;

	public bool isNotPlayIgnoreArmorEffect;

	public float cooldown;

	public float aoeRange;

	public float maxRange;

	public int bulletDirection;

	public CharacterType damageSource;

	public int sourceId;

	public int targetInstanceId;

	public EnemyModel targetEnemyModel;

	public CommonAttackDamage()
	{
	}

	public CommonAttackDamage(TargetType targetType, float aoeRange = 0f)
	{
		this.targetType = targetType;
		this.aoeRange = aoeRange;
	}

	public CommonAttackDamage(int physicsDamage, int magicDamage, float aoeRange = 0f)
	{
		this.physicsDamage = physicsDamage;
		this.magicDamage = magicDamage;
		this.aoeRange = aoeRange;
	}

	public CommonAttackDamage(int physicsDamage, int magicDamage, float aoeRange, bool isIgnoreArmor, bool isNotPlayIgnoreArmorEffect)
	{
		this.physicsDamage = physicsDamage;
		this.magicDamage = magicDamage;
		this.aoeRange = aoeRange;
		this.isIgnoreArmor = isIgnoreArmor;
		this.isNotPlayIgnoreArmorEffect = isNotPlayIgnoreArmorEffect;
	}

	public CommonAttackDamage(int physicsDamage, int magicDamage, bool isTargetAir, float aoeRange = 0f)
	{
		this.physicsDamage = physicsDamage;
		this.magicDamage = magicDamage;
		targetType.isAir = isTargetAir;
		this.aoeRange = aoeRange;
	}

	public CommonAttackDamage(int physicsDamage, int magicDamage, float aoeRange, int criticalStrikeChance, int ignoreArmorChance)
		: this(physicsDamage, magicDamage, aoeRange)
	{
		this.criticalStrikeChance = criticalStrikeChance;
		this.ignoreArmorChance = ignoreArmorChance;
		isCrit = false;
		isIgnoreArmor = false;
	}

	public CommonAttackDamage(CharacterType damageSource, int sourceId, int physicsDamage, int magicDamage, float aoeRange, int criticalStrikeChance, int ignoreArmorChance)
		: this(physicsDamage, magicDamage, aoeRange, criticalStrikeChance, ignoreArmorChance)
	{
		this.damageSource = damageSource;
		this.sourceId = sourceId;
	}
}
