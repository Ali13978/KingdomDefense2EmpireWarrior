using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class InitPoolFXByName : MonoBehaviour
	{
		[SerializeField]
		private List<string> listEffectsName = new List<string>();

		private void Start()
		{
			InitPoolFXs();
		}

		private void InitPoolFXs()
		{
			foreach (string item in listEffectsName)
			{
				EffectController effectController = null;
				effectController = UnityEngine.Object.Instantiate(Resources.Load<EffectController>($"FXs/{item}"));
				effectController.gameObject.SetActive(value: false);
				TrashManRecycleBin trashManRecycleBin = new TrashManRecycleBin();
				trashManRecycleBin.prefab = effectController.gameObject;
				trashManRecycleBin.instancesToPreallocate = 0;
				TrashManRecycleBin recycleBin = trashManRecycleBin;
				TrashMan.manageRecycleBin(recycleBin);
				TrashMan.despawn(effectController.gameObject);
			}
		}
	}
}
