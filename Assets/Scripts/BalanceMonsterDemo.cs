using UnityEngine;

public class BalanceMonsterDemo : MonoBehaviour
{
	public SpriteRenderer monster;

	public int line;

	public float timeInSec;

	public void Init(int monsterId)
	{
		monster.color = Color.white;
		monster.sprite = Resources.Load<Sprite>($"Preview/Enemies/p_enemy_{monsterId}");
		Vector3 localPosition = base.transform.localPosition;
		timeInSec = localPosition.x / 0.3f;
	}

	public void Init(int monsterId, Color color)
	{
		Init(monsterId);
		monster.color = color;
	}
}
