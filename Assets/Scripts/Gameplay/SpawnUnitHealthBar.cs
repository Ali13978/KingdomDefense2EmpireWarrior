using UnityEngine;

namespace Gameplay
{
	public class SpawnUnitHealthBar : SingletonMonoBehaviour<SpawnUnitHealthBar>
	{
		[SerializeField]
		private GameObject unitHealthBarPrefab;

		private void Awake()
		{
			PushPrototypeToPool();
		}

		private void PushPrototypeToPool()
		{
			GameObject gameObject = Object.Instantiate(unitHealthBarPrefab);
			gameObject.gameObject.transform.position = base.transform.position;
			gameObject.gameObject.SetActive(value: false);
			TrashManRecycleBin trashManRecycleBin = new TrashManRecycleBin();
			trashManRecycleBin.prefab = gameObject.gameObject;
			trashManRecycleBin.instancesToPreallocate = 0;
			TrashManRecycleBin recycleBin = trashManRecycleBin;
			TrashMan.manageRecycleBin(recycleBin);
			TrashMan.despawn(gameObject.gameObject);
		}

		public UnitHealthView Get()
		{
			UnitHealthView unitHealthView = null;
			GameObject gameObject = TrashMan.spawn("Health Bar(Clone)");
			unitHealthView = gameObject.GetComponent<UnitHealthView>();
			gameObject.gameObject.transform.position = base.transform.position;
			unitHealthView.gameObject.SetActive(value: true);
			unitHealthView.gameObject.transform.parent = base.transform;
			return unitHealthView;
		}

		public void Push(UnitHealthView enemyHealthView)
		{
			enemyHealthView.gameObject.SetActive(value: false);
			TrashMan.despawn(enemyHealthView.gameObject);
		}
	}
}
