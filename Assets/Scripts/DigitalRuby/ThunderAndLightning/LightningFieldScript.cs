using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningFieldScript : LightningBoltPrefabScriptBase
	{
		[Header("Lightning Field Properties")]
		[Tooltip("The minimum length for a field segment")]
		public float MinimumLength = 0.01f;

		private float minimumLengthSquared;

		[Tooltip("The bounds to put the field in.")]
		public Bounds FieldBounds;

		[Tooltip("Optional light for the lightning field to emit")]
		public Light Light;

		private Vector3 RandomPointInBounds()
		{
			Vector3 min = FieldBounds.min;
			float x = min.x;
			Vector3 max = FieldBounds.max;
			float x2 = Random.Range(x, max.x);
			Vector3 min2 = FieldBounds.min;
			float y = min2.y;
			Vector3 max2 = FieldBounds.max;
			float y2 = Random.Range(y, max2.y);
			Vector3 min3 = FieldBounds.min;
			float z = min3.z;
			Vector3 max3 = FieldBounds.max;
			float z2 = Random.Range(z, max3.z);
			return new Vector3(x2, y2, z2);
		}

		protected override void Start()
		{
			base.Start();
			if (Light != null)
			{
				Light.enabled = false;
			}
		}

		protected override void Update()
		{
			base.Update();
			if (Light != null)
			{
				Light.transform.position = FieldBounds.center;
				Light.intensity = Random.Range(2.8f, 3.2f);
			}
		}

		public override void CreateLightningBolt(LightningBoltParameters parameters)
		{
			minimumLengthSquared = MinimumLength * MinimumLength;
			for (int i = 0; i < 16; i++)
			{
				parameters.Start = RandomPointInBounds();
				parameters.End = RandomPointInBounds();
				if ((parameters.End - parameters.Start).sqrMagnitude >= minimumLengthSquared)
				{
					break;
				}
			}
			if (Light != null)
			{
				Light.enabled = true;
			}
			base.CreateLightningBolt(parameters);
		}
	}
}
