using UnityEngine;

public class F3DShotgun : MonoBehaviour
{
	private ParticleCollisionEvent[] collisionEvents = new ParticleCollisionEvent[16];

	private void OnParticleCollision(GameObject other)
	{
		int safeCollisionEventSize = GetComponent<ParticleSystem>().GetSafeCollisionEventSize();
		if (collisionEvents.Length < safeCollisionEventSize)
		{
			collisionEvents = new ParticleCollisionEvent[safeCollisionEventSize];
		}
		int num = GetComponent<ParticleSystem>().GetCollisionEvents(other, collisionEvents);
		for (int i = 0; i < num; i++)
		{
			F3DAudioController.instance.ShotGunHit(collisionEvents[i].intersection);
			if ((bool)other.GetComponent<Rigidbody>())
			{
				Vector3 intersection = collisionEvents[i].intersection;
				Vector3 force = collisionEvents[i].velocity.normalized * 50f;
				other.GetComponent<Rigidbody>().AddForceAtPosition(force, intersection);
			}
		}
	}
}
