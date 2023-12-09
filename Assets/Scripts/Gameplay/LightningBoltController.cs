using DigitalRuby.ThunderAndLightning;
using UnityEngine;

namespace Gameplay
{
	public class LightningBoltController : AttackAnimationController
	{
		[SerializeField]
		private LightningBoltPathScriptBase lightningBoltPathScriptBase;

		public override void Init(GameObject target, float lifeTime)
		{
			base.Init(target, lifeTime);
			lightningBoltPathScriptBase.LightningPath.List.Clear();
			lightningBoltPathScriptBase.LightningPath.List.Add(source);
			lightningBoltPathScriptBase.LightningPath.List.Add(target);
			Run();
		}
	}
}
