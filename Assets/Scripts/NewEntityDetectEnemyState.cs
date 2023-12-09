using Gameplay;
using System.Collections.Generic;

public class NewEntityDetectEnemyState : NewEntityState
{
	private float delayCheckNewEnemy = 0.12f;

	private float countdown;

	private Dictionary<int, int> enemyFlags = new Dictionary<int, int>();

	public CharacterModel heroModel;

	private int flag;

	public NewEntityDetectEnemyState(CharacterModel heroModel, INewFSMController fsmController)
		: base(fsmController)
	{
		this.heroModel = heroModel;
		flag = 1;
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		countdown -= dt;
		if (countdown <= 0f)
		{
			countdown += delayCheckNewEnemy;
			DetectNewEnemyInRange();
			if (!heroModel.IsAlive && !(heroModel.GetFSMController().GetCurrentState() is NewHeroDieState))
			{
				heroModel.Dead();
			}
		}
	}

	public void DetectNewEnemyInRange()
	{
		flag++;
		List<EnemyModel> listActiveEnemy = SingletonMonoBehaviour<GameData>.Instance.ListActiveEnemy;
		for (int num = listActiveEnemy.Count - 1; num >= 0; num--)
		{
			if (heroModel.IsInRangerRange(listActiveEnemy[num]) || heroModel.IsInMeleeRange(listActiveEnemy[num]))
			{
				int instanceID = listActiveEnemy[num].GetInstanceID();
				if (!enemyFlags.ContainsKey(instanceID))
				{
					enemyFlags.Add(instanceID, -1);
				}
				if (enemyFlags[instanceID] != flag - 1)
				{
					heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.MonsterInAtkRange, listActiveEnemy[num]);
				}
				enemyFlags[instanceID] = flag;
			}
		}
	}
}
