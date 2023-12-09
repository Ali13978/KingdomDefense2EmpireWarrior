using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningLightsabreScript : LightningBoltPrefabScript
	{
		[Header("Lightsabre Properties")]
		[Tooltip("Height of the blade")]
		public float BladeHeight = 19f;

		[Tooltip("How long it takes to turn the lightsabre on and off")]
		public float ActivationTime = 0.5f;

		[Tooltip("Sound to play when the lightsabre turns on")]
		public AudioSource StartSound;

		[Tooltip("Sound to play when the lightsabre turns off")]
		public AudioSource StopSound;

		[Tooltip("Sound to play when the lightsabre stays on")]
		public AudioSource ConstantSound;

		private int state;

		private Vector3 bladeStart;

		private Vector3 bladeDir;

		private float bladeTime;

		private float bladeIntensity;

		protected override void Start()
		{
			base.Start();
		}

		protected override void Update()
		{
			if (state == 2 || state == 3)
			{
				bladeTime += Time.deltaTime;
				float num = Mathf.Lerp(0.01f, 1f, bladeTime / ActivationTime);
				Vector3 position = bladeStart + bladeDir * num * BladeHeight;
				Destination.transform.position = position;
				GlowIntensity = bladeIntensity * ((state != 3) ? (1f - num) : num);
				if (bladeTime >= ActivationTime)
				{
					GlowIntensity = bladeIntensity;
					bladeTime = 0f;
					if (state == 2)
					{
						ManualMode = true;
						state = 0;
					}
					else
					{
						state = 1;
					}
				}
			}
			base.Update();
		}

		public bool TurnOn(bool value)
		{
			if (state == 2 || state == 3 || (state == 1 && value) || (state == 0 && !value))
			{
				return false;
			}
			bladeStart = Destination.transform.position;
			ManualMode = false;
			bladeIntensity = GlowIntensity;
			if (value)
			{
				bladeDir = ((!Camera.orthographic) ? base.transform.forward : base.transform.up);
				state = 3;
				StartSound.Play();
				StopSound.Stop();
				ConstantSound.Play();
			}
			else
			{
				bladeDir = -((!Camera.orthographic) ? base.transform.forward : base.transform.up);
				state = 2;
				StartSound.Stop();
				StopSound.Play();
				ConstantSound.Stop();
			}
			return true;
		}

		public void TurnOnGUI(bool value)
		{
			TurnOn(value);
		}
	}
}
