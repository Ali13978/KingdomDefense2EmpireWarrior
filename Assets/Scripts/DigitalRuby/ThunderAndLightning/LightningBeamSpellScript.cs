using System;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningBeamSpellScript : LightningSpellScript
	{
		[Header("Beam")]
		[Tooltip("The lightning path script creating the beam of lightning")]
		public LightningBoltPathScriptBase LightningPathScript;

		[Tooltip("Give the end point some randomization")]
		public float EndPointRandomization = 1.5f;

		[HideInInspector]
		public Action<RaycastHit> CollisionCallback;

		private void CheckCollision()
		{
			if (Physics.Raycast(SpellStart.transform.position, Direction, out RaycastHit hitInfo, MaxDistance, CollisionMask))
			{
				SpellEnd.transform.position = hitInfo.point;
				SpellEnd.transform.position += UnityEngine.Random.insideUnitSphere * EndPointRandomization;
				PlayCollisionSound(SpellEnd.transform.position);
				if (CollisionParticleSystem != null)
				{
					CollisionParticleSystem.transform.position = hitInfo.point;
					CollisionParticleSystem.Play();
				}
				ApplyCollisionForce(hitInfo.point);
				if (CollisionCallback != null)
				{
					CollisionCallback(hitInfo);
				}
			}
			else
			{
				if (CollisionParticleSystem != null)
				{
					CollisionParticleSystem.Stop();
				}
				SpellEnd.transform.position = SpellStart.transform.position + Direction * MaxDistance;
				SpellEnd.transform.position += UnityEngine.Random.insideUnitSphere * EndPointRandomization;
			}
		}

		protected override void Start()
		{
			base.Start();
			LightningPathScript.ManualMode = true;
		}

		protected override void LateUpdate()
		{
			base.LateUpdate();
			if (base.Casting)
			{
				CheckCollision();
			}
		}

		protected override void OnCastSpell()
		{
			LightningPathScript.ManualMode = false;
		}

		protected override void OnStopSpell()
		{
			LightningPathScript.ManualMode = true;
		}
	}
}
