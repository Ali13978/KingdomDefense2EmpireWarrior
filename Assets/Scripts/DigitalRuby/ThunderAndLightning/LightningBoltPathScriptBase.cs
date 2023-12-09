using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public abstract class LightningBoltPathScriptBase : LightningBoltPrefabScriptBase
	{
		[Header("Lightning Path Properties")]
		[ReorderableList("The game objects to follow for the lightning path")]
		public ReorderableList_GameObject LightningPath;

		private readonly List<GameObject> currentPathObjects = new List<GameObject>();

		protected List<GameObject> GetCurrentPathObjects()
		{
			currentPathObjects.Clear();
			if (LightningPath != null && LightningPath.List != null)
			{
				foreach (GameObject item in LightningPath.List)
				{
					if (item != null && item.activeInHierarchy)
					{
						currentPathObjects.Add(item);
					}
				}
			}
			return currentPathObjects;
		}

		protected override LightningBoltParameters OnCreateParameters()
		{
			LightningBoltParameters lightningBoltParameters = base.OnCreateParameters();
			lightningBoltParameters.Generator = LightningGenerator.GeneratorInstance;
			return lightningBoltParameters;
		}
	}
}
