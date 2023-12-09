using System;
using System.Collections;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class ThunderAndLightningScript : MonoBehaviour
	{
		private class LightningBoltHandler
		{
			private ThunderAndLightningScript script;

			private readonly System.Random random = new System.Random();

			public LightningBoltHandler(ThunderAndLightningScript script)
			{
				this.script = script;
				CalculateNextLightningTime();
			}

			private void UpdateLighting()
			{
				if (script.lightningInProgress)
				{
					return;
				}
				if (script.ModifySkyboxExposure)
				{
					script.skyboxExposureStorm = 0.35f;
					if (script.skyboxMaterial != null && script.skyboxMaterial.HasProperty("_Exposure"))
					{
						script.skyboxMaterial.SetFloat("_Exposure", script.skyboxExposureStorm);
					}
				}
				CheckForLightning();
			}

			private void CalculateNextLightningTime()
			{
				script.nextLightningTime = Time.time + script.LightningIntervalTimeRange.Random(random);
				script.lightningInProgress = false;
				if (script.ModifySkyboxExposure && script.skyboxMaterial.HasProperty("_Exposure"))
				{
					script.skyboxMaterial.SetFloat("_Exposure", script.skyboxExposureStorm);
				}
			}

			public IEnumerator ProcessLightning(Vector3? _start, Vector3? _end, bool intense, bool visible)
			{
				script.lightningInProgress = true;
				float intensity;
				float sleepTime;
				AudioClip[] sounds;
				if (intense)
				{
					float t = UnityEngine.Random.Range(0f, 1f);
					intensity = Mathf.Lerp(2f, 8f, t);
					sleepTime = 5f / intensity;
					sounds = script.ThunderSoundsIntense;
				}
				else
				{
					float t2 = UnityEngine.Random.Range(0f, 1f);
					intensity = Mathf.Lerp(0f, 2f, t2);
					sleepTime = 30f / intensity;
					sounds = script.ThunderSoundsNormal;
				}
				if (script.skyboxMaterial != null && script.ModifySkyboxExposure)
				{
					script.skyboxMaterial.SetFloat("_Exposure", Mathf.Max(intensity * 0.5f, script.skyboxExposureStorm));
				}
				Strike(_start, _end, intense, intensity, script.Camera, (!visible) ? null : script.Camera);
				CalculateNextLightningTime();
				if (intensity >= 1f && sounds != null && sounds.Length != 0)
				{
					yield return new WaitForSeconds(sleepTime);
					AudioClip clip;
					do
					{
						clip = sounds[UnityEngine.Random.Range(0, sounds.Length - 1)];
					}
					while (sounds.Length > 1 && clip == script.lastThunderSound);
					script.lastThunderSound = clip;
					script.audioSourceThunder.PlayOneShot(clip, intensity * 0.5f);
				}
			}

			private void Strike(Vector3? _start, Vector3? _end, bool intense, float intensity, Camera camera, Camera visibleInCamera)
			{
				float min = (!intense) ? (-5000f) : (-1000f);
				float max = (!intense) ? 5000f : 1000f;
				float num = (!intense) ? 2500f : 500f;
				float num2 = (UnityEngine.Random.Range(0, 2) != 0) ? UnityEngine.Random.Range(num, max) : UnityEngine.Random.Range(min, 0f - num);
				float lightningYStart = script.LightningYStart;
				float num3 = (UnityEngine.Random.Range(0, 2) != 0) ? UnityEngine.Random.Range(num, max) : UnityEngine.Random.Range(min, 0f - num);
				Vector3 vector = script.Camera.transform.position;
				vector.x += num2;
				vector.y = lightningYStart;
				vector.z += num3;
				if (visibleInCamera != null)
				{
					Quaternion rotation = visibleInCamera.transform.rotation;
					Transform transform = visibleInCamera.transform;
					Vector3 eulerAngles = rotation.eulerAngles;
					transform.rotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
					float x = UnityEngine.Random.Range((float)visibleInCamera.pixelWidth * 0.1f, (float)visibleInCamera.pixelWidth * 0.9f);
					float z = UnityEngine.Random.Range(visibleInCamera.nearClipPlane + num + num, max);
					Vector3 vector2 = visibleInCamera.ScreenToWorldPoint(new Vector3(x, 0f, z));
					vector = vector2;
					vector.y = lightningYStart;
					visibleInCamera.transform.rotation = rotation;
				}
				Vector3 vector3 = vector;
				num2 = UnityEngine.Random.Range(-100f, 100f);
				lightningYStart = ((UnityEngine.Random.Range(0, 4) != 0) ? (-1f) : UnityEngine.Random.Range(-1f, 600f));
				num3 += UnityEngine.Random.Range(-100f, 100f);
				vector3.x += num2;
				vector3.y = lightningYStart;
				vector3.z += num3;
				float x2 = vector3.x;
				float num4 = num;
				Vector3 forward = camera.transform.forward;
				vector3.x = x2 + num4 * forward.x;
				float z2 = vector3.z;
				float num5 = num;
				Vector3 forward2 = camera.transform.forward;
				vector3.z = z2 + num5 * forward2.z;
				while ((vector - vector3).magnitude < 500f)
				{
					float x3 = vector3.x;
					float num6 = num;
					Vector3 forward3 = camera.transform.forward;
					vector3.x = x3 + num6 * forward3.x;
					float z3 = vector3.z;
					float num7 = num;
					Vector3 forward4 = camera.transform.forward;
					vector3.z = z3 + num7 * forward4.z;
				}
				vector = ((!_start.HasValue) ? vector : _start.Value);
				vector3 = ((!_end.HasValue) ? vector3 : _end.Value);
				if (Physics.Raycast(vector, (vector - vector3).normalized, out RaycastHit hitInfo, float.MaxValue))
				{
					vector3 = hitInfo.point;
				}
				int generations = script.LightningBoltScript.Generations;
				RangeOfFloats trunkWidthRange = script.LightningBoltScript.TrunkWidthRange;
				if (UnityEngine.Random.value < script.CloudLightningChance)
				{
					script.LightningBoltScript.TrunkWidthRange = default(RangeOfFloats);
					script.LightningBoltScript.Generations = 1;
				}
				script.LightningBoltScript.LightParameters.LightIntensity = intensity * 0.5f;
				script.LightningBoltScript.Trigger(vector, vector3);
				script.LightningBoltScript.TrunkWidthRange = trunkWidthRange;
				script.LightningBoltScript.Generations = generations;
			}

			private void CheckForLightning()
			{
				if (Time.time >= script.nextLightningTime)
				{
					bool intense = UnityEngine.Random.value < script.LightningIntenseProbability;
					script.StartCoroutine(ProcessLightning(null, null, intense, script.LightningAlwaysVisible));
				}
			}

			public void Update()
			{
				UpdateLighting();
			}
		}

		[Tooltip("Lightning bolt script - optional, leave null if you don't want lightning bolts")]
		public LightningBoltPrefabScript LightningBoltScript;

		[Tooltip("Camera where the lightning should be centered over. Defaults to main camera.")]
		public Camera Camera;

		[SingleLine("Random interval between strikes.")]
		public RangeOfFloats LightningIntervalTimeRange = new RangeOfFloats
		{
			Minimum = 10f,
			Maximum = 25f
		};

		[Tooltip("Probability (0-1) of an intense lightning bolt that hits really close. Intense lightning has increased brightness and louder thunder compared to normal lightning, and the thunder sounds plays a lot sooner.")]
		[Range(0f, 1f)]
		public float LightningIntenseProbability = 0.2f;

		[Tooltip("Sounds to play for normal thunder. One will be chosen at random for each lightning strike. Depending on intensity, some normal lightning may not play a thunder sound.")]
		public AudioClip[] ThunderSoundsNormal;

		[Tooltip("Sounds to play for intense thunder. One will be chosen at random for each lightning strike.")]
		public AudioClip[] ThunderSoundsIntense;

		[Tooltip("Whether lightning strikes should always try to be in the camera view")]
		public bool LightningAlwaysVisible = true;

		[Tooltip("The chance lightning will simply be in the clouds with no visible bolt")]
		[Range(0f, 1f)]
		public float CloudLightningChance = 0.5f;

		[Tooltip("Whether to modify the skybox exposure when lightning is created")]
		public bool ModifySkyboxExposure;

		[Tooltip("Base point light range for lightning bolts. Increases as intensity increases.")]
		[Range(1f, 10000f)]
		public float BaseLightRange = 2000f;

		[Tooltip("Starting y value for the lightning strikes")]
		[Range(0f, 100000f)]
		public float LightningYStart = 500f;

		private float skyboxExposureOriginal;

		private float skyboxExposureStorm;

		private float nextLightningTime;

		private bool lightningInProgress;

		private AudioSource audioSourceThunder;

		private LightningBoltHandler lightningBoltHandler;

		private Material skyboxMaterial;

		private AudioClip lastThunderSound;

		public float SkyboxExposureOriginal => skyboxExposureOriginal;

		public bool EnableLightning
		{
			get;
			set;
		}

		private void Start()
		{
			EnableLightning = true;
			if (Camera == null)
			{
				Camera = Camera.main;
			}
			if (RenderSettings.skybox != null)
			{
				Material material2 = skyboxMaterial = (RenderSettings.skybox = new Material(RenderSettings.skybox));
			}
			skyboxExposureOriginal = (skyboxExposureStorm = ((!(skyboxMaterial == null) && skyboxMaterial.HasProperty("_Exposure")) ? skyboxMaterial.GetFloat("_Exposure") : 1f));
			audioSourceThunder = base.gameObject.AddComponent<AudioSource>();
			lightningBoltHandler = new LightningBoltHandler(this);
		}

		private void Update()
		{
			if (lightningBoltHandler != null && EnableLightning)
			{
				lightningBoltHandler.Update();
			}
		}

		public void CallNormalLightning()
		{
			CallNormalLightning(null, null);
		}

		public void CallNormalLightning(Vector3? start, Vector3? end)
		{
			StartCoroutine(lightningBoltHandler.ProcessLightning(start, end, intense: false, visible: true));
		}

		public void CallIntenseLightning()
		{
			CallIntenseLightning(null, null);
		}

		public void CallIntenseLightning(Vector3? start, Vector3? end)
		{
			StartCoroutine(lightningBoltHandler.ProcessLightning(start, end, intense: true, visible: true));
		}
	}
}
