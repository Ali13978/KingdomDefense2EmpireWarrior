using UnityEngine;

public class AbilitiesRank : ScriptableObject
{
	[Header("Attack Speed Ranking")]
	public int[] AttackSpeedValue;

	public int[] AttackSpeedDescription;

	[Space]
	[Header("Attack Range Ranking")]
	public int[] AttackRangeValue;

	public int[] AttackRangeDescription;

	[Space]
	[Header("Armor Ranking")]
	public int[] ArmorValue;

	public int[] ArmorDescription;

	[Space]
	[Header("Movement Speed Ranking")]
	public int[] MSpeedValue;

	public int[] MSpeedDescription;
}
