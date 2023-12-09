using System.Collections;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	[RequireComponent(typeof(AudioSource))]
	public class LightningWhipScript : MonoBehaviour
	{
		public AudioClip WhipCrack;

		public AudioClip WhipCrackThunder;

		private AudioSource audioSource;

		private GameObject whipStart;

		private GameObject whipEndStrike;

		private GameObject whipHandle;

		private GameObject whipSpring;

		private Vector2 prevDrag;

		private bool dragging;

		private bool canWhip = true;

		private IEnumerator WhipForward()
		{
			if (!canWhip)
			{
				yield break;
			}
			canWhip = false;
			for (int i = 0; i < whipStart.transform.childCount; i++)
			{
				GameObject gameObject = whipStart.transform.GetChild(i).gameObject;
				Rigidbody2D component = gameObject.GetComponent<Rigidbody2D>();
				if (component != null)
				{
					component.drag = 0f;
				}
			}
			audioSource.PlayOneShot(WhipCrack);
			whipSpring.GetComponent<SpringJoint2D>().enabled = true;
			whipSpring.GetComponent<Rigidbody2D>().position = whipHandle.GetComponent<Rigidbody2D>().position + new Vector2(-15f, 5f);
			yield return new WaitForSeconds(0.2f);
			whipSpring.GetComponent<Rigidbody2D>().position = whipHandle.GetComponent<Rigidbody2D>().position + new Vector2(15f, 2.5f);
			yield return new WaitForSeconds(0.15f);
			audioSource.PlayOneShot(WhipCrackThunder, 0.5f);
			yield return new WaitForSeconds(0.15f);
			whipEndStrike.GetComponent<ParticleSystem>().Play();
			whipSpring.GetComponent<SpringJoint2D>().enabled = false;
			yield return new WaitForSeconds(0.65f);
			for (int j = 0; j < whipStart.transform.childCount; j++)
			{
				GameObject gameObject2 = whipStart.transform.GetChild(j).gameObject;
				Rigidbody2D component2 = gameObject2.GetComponent<Rigidbody2D>();
				if (component2 != null)
				{
					component2.velocity = Vector2.zero;
					component2.drag = 0.5f;
				}
			}
			canWhip = true;
		}

		private void Start()
		{
			whipStart = GameObject.Find("WhipStart");
			whipEndStrike = GameObject.Find("WhipEndStrike");
			whipHandle = GameObject.Find("WhipHandle");
			whipSpring = GameObject.Find("WhipSpring");
			audioSource = GetComponent<AudioSource>();
		}

		private void Update()
		{
			if (!dragging && Input.GetMouseButtonDown(0))
			{
				Vector2 point = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
				Collider2D collider2D = Physics2D.OverlapPoint(point);
				if (collider2D != null && collider2D.gameObject == whipHandle)
				{
					dragging = true;
					prevDrag = point;
				}
			}
			else if (dragging && Input.GetMouseButton(0))
			{
				Vector2 a = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
				Vector2 b = a - prevDrag;
				Rigidbody2D component = whipHandle.GetComponent<Rigidbody2D>();
				component.MovePosition(component.position + b);
				prevDrag = a;
			}
			else
			{
				dragging = false;
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
			{
				StartCoroutine(WhipForward());
			}
		}
	}
}
