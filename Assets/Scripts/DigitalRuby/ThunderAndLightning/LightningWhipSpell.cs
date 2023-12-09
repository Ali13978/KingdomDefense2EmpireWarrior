using System;
using System.Collections;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningWhipSpell : LightningSpellScript
	{
		[Header("Whip")]
		[Tooltip("Attach the whip to what object")]
		public GameObject AttachTo;

		[Tooltip("Rotate the whip with this object")]
		public GameObject RotateWith;

		[Tooltip("Whip handle")]
		public GameObject WhipHandle;

		[Tooltip("Whip start")]
		public GameObject WhipStart;

		[Tooltip("Whip spring")]
		public GameObject WhipSpring;

		[Tooltip("Whip crack audio source")]
		public AudioSource WhipCrackAudioSource;

		[HideInInspector]
		public Action<Vector3> CollisionCallback;

		private IEnumerator WhipForward()
		{
			for (int i = 0; i < WhipStart.transform.childCount; i++)
			{
				GameObject gameObject = WhipStart.transform.GetChild(i).gameObject;
				Rigidbody component = gameObject.GetComponent<Rigidbody>();
				if (component != null)
				{
					component.drag = 0f;
					component.velocity = Vector3.zero;
					component.angularVelocity = Vector3.zero;
				}
			}
			WhipSpring.SetActive(value: true);
			Vector3 anchor = WhipStart.GetComponent<Rigidbody>().position;
			Vector3 whipPositionForwards;
			Vector3 whipPositionBackwards;
			if (Physics.Raycast(anchor, Direction, out RaycastHit hit, MaxDistance, CollisionMask))
			{
				Vector3 normalized = (hit.point - anchor).normalized;
				whipPositionForwards = anchor + normalized * MaxDistance;
				whipPositionBackwards = anchor - normalized * 25f;
			}
			else
			{
				whipPositionForwards = anchor + Direction * MaxDistance;
				whipPositionBackwards = anchor - Direction * 25f;
			}
			WhipSpring.GetComponent<Rigidbody>().position = whipPositionBackwards;
			yield return new WaitForSeconds(0.25f);
			WhipSpring.GetComponent<Rigidbody>().position = whipPositionForwards;
			yield return new WaitForSeconds(0.1f);
			if (WhipCrackAudioSource != null)
			{
				WhipCrackAudioSource.Play();
			}
			yield return new WaitForSeconds(0.1f);
			if (CollisionParticleSystem != null)
			{
				CollisionParticleSystem.Play();
			}
			ApplyCollisionForce(SpellEnd.transform.position);
			WhipSpring.SetActive(value: false);
			if (CollisionCallback != null)
			{
				CollisionCallback(SpellEnd.transform.position);
			}
			yield return new WaitForSeconds(0.1f);
			for (int j = 0; j < WhipStart.transform.childCount; j++)
			{
				GameObject gameObject2 = WhipStart.transform.GetChild(j).gameObject;
				Rigidbody component2 = gameObject2.GetComponent<Rigidbody>();
				if (component2 != null)
				{
					component2.velocity = Vector3.zero;
					component2.angularVelocity = Vector3.zero;
					component2.drag = 0.5f;
				}
			}
		}

		protected override void Start()
		{
			base.Start();
			WhipSpring.SetActive(value: false);
			WhipHandle.SetActive(value: false);
		}

		protected override void Update()
		{
			base.Update();
			base.gameObject.transform.position = AttachTo.transform.position;
			base.gameObject.transform.rotation = RotateWith.transform.rotation;
		}

		protected override void OnCastSpell()
		{
			StartCoroutine(WhipForward());
		}

		protected override void OnStopSpell()
		{
		}

		protected override void OnActivated()
		{
			base.OnActivated();
			WhipHandle.SetActive(value: true);
		}

		protected override void OnDeactivated()
		{
			base.OnDeactivated();
			WhipHandle.SetActive(value: false);
		}
	}
}
