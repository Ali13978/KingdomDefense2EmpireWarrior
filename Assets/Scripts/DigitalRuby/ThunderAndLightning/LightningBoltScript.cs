using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningBoltScript : MonoBehaviour
	{
		[Header("Lightning General Properties")]
		[Tooltip("The camera the lightning should be shown in. Defaults to the current camera, or the main camera if current camera is null. If you are using a different camera, you may want to put the lightning in it's own layer and cull that layer out of any other cameras.")]
		public Camera Camera;

		[Tooltip("Type of camera mode. Auto detects the camera and creates appropriate lightning. Can be overriden to do something more specific regardless of camera.")]
		public CameraMode CameraMode;

		internal CameraMode calculatedCameraMode = CameraMode.Unknown;

		[Tooltip("True if you are using world space coordinates for the lightning bolt, false if you are using coordinates relative to the parent game object.")]
		public bool UseWorldSpace = true;

		[Tooltip("Whether to compensate for the parent transform. Default is false. If true, rotation, scale and position are altered by the parent transform. Use this to fix scaling, rotation and other offset problems with the lightning.")]
		public bool CompensateForParentTransform;

		[Tooltip("Lightning quality setting. This allows setting limits on generations, lights and shadow casting lights based on the global quality setting.")]
		public LightningBoltQualitySetting QualitySetting;

		[Tooltip("Whether to use multi-threaded generation of lightning. Lightning will be delayed by about 1 frame if this is turned on, but this can significantly improve performance.")]
		public bool MultiThreaded;

		[Tooltip("Soft particles factor. 0.01 to 3.0 are typical, 100.0 to disable.")]
		[Range(0.01f, 100f)]
		public float SoftParticlesFactor = 3f;

		[Header("Lightning Rendering Properties")]
		[Tooltip("The render queue for the lightning. -1 for default.")]
		public int RenderQueue = -1;

		[HideInInspector]
		public string SortLayerName;

		[HideInInspector]
		public int SortOrderInLayer;

		[Tooltip("Lightning material for mesh renderer")]
		public Material LightningMaterialMesh;

		[Tooltip("Lightning material for mesh renderer, without glow")]
		public Material LightningMaterialMeshNoGlow;

		[Tooltip("The texture to use for the lightning bolts, or null for the material default texture.")]
		public Texture2D LightningTexture;

		[Tooltip("The texture to use for the lightning glow, or null for the material default texture.")]
		public Texture2D LightningGlowTexture;

		[Tooltip("Particle system to play at the point of emission (start). 'Emission rate' particles will be emitted all at once.")]
		public ParticleSystem LightningOriginParticleSystem;

		[Tooltip("Particle system to play at the point of impact (end). 'Emission rate' particles will be emitted all at once.")]
		public ParticleSystem LightningDestinationParticleSystem;

		[Tooltip("Tint color for the lightning")]
		public Color LightningTintColor = Color.white;

		[Tooltip("Tint color for the lightning glow")]
		public Color GlowTintColor = new Color(0.1f, 0.2f, 1f, 1f);

		[Tooltip("Source blend mode. Default is SrcAlpha.")]
		public BlendMode SourceBlendMode = BlendMode.SrcAlpha;

		[Tooltip("Destination blend mode. Default is One. For additive blend use One. For alpha blend use OneMinusSrcAlpha.")]
		public BlendMode DestinationBlendMode = BlendMode.One;

		[Header("Lightning Movement Properties")]
		[Tooltip("Jitter multiplier to randomize lightning size. Jitter depends on trunk width and will make the lightning move rapidly and jaggedly, giving a more lively and sometimes cartoony feel. Jitter may be shared with other bolts depending on materials. If you need different jitters for the same material, create a second script object.")]
		public float JitterMultiplier;

		[Tooltip("Built in turbulance based on the direction of each segment. Small values usually work better, like 0.2.")]
		public float Turbulence;

		[Tooltip("Global turbulence velocity for this script")]
		public Vector3 TurbulenceVelocity = Vector3.zero;

		private Texture2D lastLightningTexture;

		private Texture2D lastLightningGlowTexture;

		private readonly List<LightningBolt> activeBolts = new List<LightningBolt>();

		private readonly LightningBoltParameters[] oneParameterArray = new LightningBoltParameters[1];

		private readonly List<LightningBolt> lightningBoltCache = new List<LightningBolt>();

		private readonly List<LightningBoltDependencies> dependenciesCache = new List<LightningBoltDependencies>();

		private LightningThreadState threadState;

		private static int shaderId_MainTex = int.MinValue;

		private static int shaderId_GlowTex;

		private static int shaderId_TintColor;

		private static int shaderId_GlowTintColor;

		private static int shaderId_JitterMultiplier;

		private static int shaderId_Turbulence;

		private static int shaderId_TurbulenceVelocity;

		private static int shaderId_SrcBlendMode;

		private static int shaderId_DstBlendMode;

		private static int shaderId_InvFade;

		public Action<LightningBoltParameters, Vector3, Vector3> LightningStartedCallback
		{
			get;
			set;
		}

		public Action<LightningBoltParameters, Vector3, Vector3> LightningEndedCallback
		{
			get;
			set;
		}

		public Action<Light> LightAddedCallback
		{
			get;
			set;
		}

		public Action<Light> LightRemovedCallback
		{
			get;
			set;
		}

		public bool HasActiveBolts => activeBolts.Count != 0;

		internal Material lightningMaterialMeshInternal
		{
			get;
			private set;
		}

		internal Material lightningMaterialMeshNoGlowInternal
		{
			get;
			private set;
		}

		public virtual void CreateLightningBolt(LightningBoltParameters p)
		{
			if (p != null)
			{
				UpdateTexture();
				oneParameterArray[0] = p;
				LightningBolt orCreateLightningBolt = GetOrCreateLightningBolt();
				LightningBoltDependencies dependencies = CreateLightningBoltDependencies(oneParameterArray);
				orCreateLightningBolt.SetupLightningBolt(dependencies);
			}
		}

		public void CreateLightningBolts(ICollection<LightningBoltParameters> parameters)
		{
			if (parameters != null && parameters.Count != 0)
			{
				UpdateTexture();
				LightningBolt orCreateLightningBolt = GetOrCreateLightningBolt();
				LightningBoltDependencies dependencies = CreateLightningBoltDependencies(parameters);
				orCreateLightningBolt.SetupLightningBolt(dependencies);
			}
		}

		protected virtual void Awake()
		{
			UpdateShaderIds();
		}

		protected virtual void Start()
		{
			UpdateCamera();
			UpdateMaterialsForLastTexture();
			UpdateShaderParameters();
			CheckCompensateForParentTransform();
			SceneManager.sceneLoaded += OnSceneLoaded;
			if (MultiThreaded)
			{
				threadState = new LightningThreadState();
				InvokeRepeating("UpdateMainThreadActions", 0f, 0.004166667f);
			}
		}

		protected virtual void Update()
		{
			if (HasActiveBolts)
			{
				UpdateCamera();
				UpdateShaderParameters();
				CheckCompensateForParentTransform();
				UpdateActiveBolts();
			}
		}

		protected virtual LightningBoltParameters OnCreateParameters()
		{
			return LightningBoltParameters.GetOrCreateParameters();
		}

		protected LightningBoltParameters CreateParameters()
		{
			LightningBoltParameters lightningBoltParameters = OnCreateParameters();
			lightningBoltParameters.quality = QualitySetting;
			PopulateParameters(lightningBoltParameters);
			return lightningBoltParameters;
		}

		protected virtual void PopulateParameters(LightningBoltParameters p)
		{
		}

		private Coroutine StartCoroutineWrapper(IEnumerator routine)
		{
			if (base.isActiveAndEnabled)
			{
				return StartCoroutine(routine);
			}
			return null;
		}

		private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
		{
			LightningBolt.ClearCache();
		}

		private LightningBoltDependencies CreateLightningBoltDependencies(ICollection<LightningBoltParameters> parameters)
		{
			LightningBoltDependencies lightningBoltDependencies;
			if (dependenciesCache.Count == 0)
			{
				lightningBoltDependencies = new LightningBoltDependencies();
				lightningBoltDependencies.AddActiveBolt = AddActiveBolt;
				lightningBoltDependencies.LightAdded = OnLightAdded;
				lightningBoltDependencies.LightRemoved = OnLightRemoved;
				lightningBoltDependencies.ReturnToCache = ReturnLightningDependenciesToCache;
				lightningBoltDependencies.StartCoroutine = StartCoroutineWrapper;
				lightningBoltDependencies.Parent = base.gameObject;
			}
			else
			{
				int index = dependenciesCache.Count - 1;
				lightningBoltDependencies = dependenciesCache[index];
				dependenciesCache.RemoveAt(index);
			}
			lightningBoltDependencies.CameraPos = Camera.transform.position;
			lightningBoltDependencies.CameraIsOrthographic = Camera.orthographic;
			lightningBoltDependencies.CameraMode = calculatedCameraMode;
			lightningBoltDependencies.DestParticleSystem = LightningDestinationParticleSystem;
			lightningBoltDependencies.LightningMaterialMesh = lightningMaterialMeshInternal;
			lightningBoltDependencies.LightningMaterialMeshNoGlow = lightningMaterialMeshNoGlowInternal;
			lightningBoltDependencies.OriginParticleSystem = LightningOriginParticleSystem;
			lightningBoltDependencies.SortLayerName = SortLayerName;
			lightningBoltDependencies.SortOrderInLayer = SortOrderInLayer;
			lightningBoltDependencies.UseWorldSpace = UseWorldSpace;
			lightningBoltDependencies.ThreadState = threadState;
			if (threadState != null)
			{
				lightningBoltDependencies.Parameters = new List<LightningBoltParameters>(parameters);
			}
			else
			{
				lightningBoltDependencies.Parameters = parameters;
			}
			lightningBoltDependencies.LightningBoltStarted = LightningStartedCallback;
			lightningBoltDependencies.LightningBoltEnded = LightningEndedCallback;
			return lightningBoltDependencies;
		}

		private void ReturnLightningDependenciesToCache(LightningBoltDependencies d)
		{
			d.Parameters = null;
			d.OriginParticleSystem = null;
			d.DestParticleSystem = null;
			d.LightningMaterialMesh = null;
			d.LightningMaterialMeshNoGlow = null;
			dependenciesCache.Add(d);
		}

		internal void OnLightAdded(Light l)
		{
			if (LightAddedCallback != null)
			{
				LightAddedCallback(l);
			}
		}

		internal void OnLightRemoved(Light l)
		{
			if (LightRemovedCallback != null)
			{
				LightRemovedCallback(l);
			}
		}

		internal void AddActiveBolt(LightningBolt bolt)
		{
			activeBolts.Add(bolt);
		}

		private void UpdateMainThreadActions()
		{
			threadState.UpdateMainThreadActions();
		}

		private void UpdateShaderIds()
		{
			if (shaderId_MainTex == int.MinValue)
			{
				shaderId_MainTex = Shader.PropertyToID("_MainTex");
				shaderId_GlowTex = Shader.PropertyToID("_GlowTex");
				shaderId_TintColor = Shader.PropertyToID("_TintColor");
				shaderId_GlowTintColor = Shader.PropertyToID("_GlowTintColor");
				shaderId_JitterMultiplier = Shader.PropertyToID("_JitterMultiplier");
				shaderId_Turbulence = Shader.PropertyToID("_Turbulence");
				shaderId_TurbulenceVelocity = Shader.PropertyToID("_TurbulenceVelocity");
				shaderId_SrcBlendMode = Shader.PropertyToID("_SrcBlendMode");
				shaderId_DstBlendMode = Shader.PropertyToID("_DstBlendMode");
				shaderId_InvFade = Shader.PropertyToID("_InvFade");
			}
		}

		private void UpdateMaterialsForLastTexture()
		{
			if (Application.isPlaying)
			{
				calculatedCameraMode = CameraMode.Unknown;
				lightningMaterialMeshInternal = new Material(LightningMaterialMesh);
				lightningMaterialMeshNoGlowInternal = new Material(LightningMaterialMeshNoGlow);
				if (LightningTexture != null)
				{
					lightningMaterialMeshInternal.SetTexture(shaderId_MainTex, LightningTexture);
					lightningMaterialMeshNoGlowInternal.SetTexture(shaderId_MainTex, LightningTexture);
				}
				if (LightningGlowTexture != null)
				{
					lightningMaterialMeshInternal.SetTexture(shaderId_GlowTex, LightningGlowTexture);
				}
				SetupMaterialCamera();
			}
		}

		private void UpdateTexture()
		{
			if (LightningTexture != null && LightningTexture != lastLightningTexture)
			{
				lastLightningTexture = LightningTexture;
				UpdateMaterialsForLastTexture();
			}
			if (LightningGlowTexture != null && LightningGlowTexture != lastLightningGlowTexture)
			{
				lastLightningGlowTexture = LightningGlowTexture;
				UpdateMaterialsForLastTexture();
			}
		}

		private void SetMaterialPerspective()
		{
			if (calculatedCameraMode != CameraMode.Perspective)
			{
				calculatedCameraMode = CameraMode.Perspective;
				lightningMaterialMeshInternal.EnableKeyword("PERSPECTIVE");
				lightningMaterialMeshNoGlowInternal.EnableKeyword("PERSPECTIVE");
				lightningMaterialMeshInternal.DisableKeyword("ORTHOGRAPHIC_XY");
				lightningMaterialMeshNoGlowInternal.DisableKeyword("ORTHOGRAPHIC_XY");
				lightningMaterialMeshInternal.DisableKeyword("ORTHOGRAPHIC_XZ");
				lightningMaterialMeshNoGlowInternal.DisableKeyword("ORTHOGRAPHIC_XZ");
			}
		}

		private void SetMaterialOrthographicXY()
		{
			if (calculatedCameraMode != CameraMode.OrthographicXY)
			{
				calculatedCameraMode = CameraMode.OrthographicXY;
				lightningMaterialMeshInternal.EnableKeyword("ORTHOGRAPHIC_XY");
				lightningMaterialMeshNoGlowInternal.EnableKeyword("ORTHOGRAPHIC_XY");
				lightningMaterialMeshInternal.DisableKeyword("ORTHOGRAPHIC_XZ");
				lightningMaterialMeshNoGlowInternal.DisableKeyword("ORTHOGRAPHIC_XZ");
				lightningMaterialMeshInternal.DisableKeyword("PERSPECTIVE");
				lightningMaterialMeshNoGlowInternal.DisableKeyword("PERSPECTIVE");
			}
		}

		private void SetMaterialOrthographicXZ()
		{
			if (calculatedCameraMode != CameraMode.OrthographicXZ)
			{
				calculatedCameraMode = CameraMode.OrthographicXZ;
				lightningMaterialMeshInternal.EnableKeyword("ORTHOGRAPHIC_XZ");
				lightningMaterialMeshNoGlowInternal.EnableKeyword("ORTHOGRAPHIC_XZ");
				lightningMaterialMeshInternal.DisableKeyword("ORTHOGRAPHIC_XY");
				lightningMaterialMeshNoGlowInternal.DisableKeyword("ORTHOGRAPHIC_XY");
				lightningMaterialMeshInternal.DisableKeyword("PERSPECTIVE");
				lightningMaterialMeshNoGlowInternal.DisableKeyword("PERSPECTIVE");
			}
		}

		private void SetupMaterialCamera()
		{
			if (Camera == null && CameraMode == CameraMode.Auto)
			{
				SetMaterialPerspective();
			}
			else if (CameraMode == CameraMode.Auto)
			{
				if (Camera.orthographic)
				{
					SetMaterialOrthographicXY();
				}
				else
				{
					SetMaterialPerspective();
				}
			}
			else if (CameraMode == CameraMode.Perspective)
			{
				SetMaterialPerspective();
			}
			else if (CameraMode == CameraMode.OrthographicXY)
			{
				SetMaterialOrthographicXY();
			}
			else
			{
				SetMaterialOrthographicXZ();
			}
		}

		private void EnableKeyword(string keyword, bool enable, Material m)
		{
			if (enable)
			{
				m.EnableKeyword(keyword);
			}
			else
			{
				m.DisableKeyword(keyword);
			}
		}

		private void UpdateShaderParameters()
		{
			lightningMaterialMeshInternal.SetColor(shaderId_TintColor, LightningTintColor);
			lightningMaterialMeshInternal.SetColor(shaderId_GlowTintColor, GlowTintColor);
			lightningMaterialMeshInternal.SetFloat(shaderId_JitterMultiplier, JitterMultiplier);
			lightningMaterialMeshInternal.SetFloat(shaderId_Turbulence, Turbulence * LightningBoltParameters.Scale);
			lightningMaterialMeshInternal.SetVector(shaderId_TurbulenceVelocity, TurbulenceVelocity * LightningBoltParameters.Scale);
			lightningMaterialMeshInternal.SetInt(shaderId_SrcBlendMode, (int)SourceBlendMode);
			lightningMaterialMeshInternal.SetInt(shaderId_DstBlendMode, (int)DestinationBlendMode);
			lightningMaterialMeshInternal.renderQueue = RenderQueue;
			lightningMaterialMeshInternal.SetFloat(shaderId_InvFade, SoftParticlesFactor);
			lightningMaterialMeshNoGlowInternal.SetColor(shaderId_TintColor, LightningTintColor);
			lightningMaterialMeshNoGlowInternal.SetFloat(shaderId_JitterMultiplier, JitterMultiplier);
			lightningMaterialMeshNoGlowInternal.SetFloat(shaderId_Turbulence, Turbulence * LightningBoltParameters.Scale);
			lightningMaterialMeshNoGlowInternal.SetVector(shaderId_TurbulenceVelocity, TurbulenceVelocity * LightningBoltParameters.Scale);
			lightningMaterialMeshNoGlowInternal.SetInt(shaderId_SrcBlendMode, (int)SourceBlendMode);
			lightningMaterialMeshNoGlowInternal.SetInt(shaderId_DstBlendMode, (int)DestinationBlendMode);
			lightningMaterialMeshNoGlowInternal.renderQueue = RenderQueue;
			lightningMaterialMeshNoGlowInternal.SetFloat(shaderId_InvFade, SoftParticlesFactor);
			SetupMaterialCamera();
		}

		private void CheckCompensateForParentTransform()
		{
			if (CompensateForParentTransform)
			{
				Transform parent = base.transform.parent;
				if (parent != null)
				{
					base.transform.position = parent.position;
					Transform transform = base.transform;
					Vector3 localScale = parent.localScale;
					float x = 1f / localScale.x;
					Vector3 localScale2 = parent.localScale;
					float y = 1f / localScale2.y;
					Vector3 localScale3 = parent.localScale;
					transform.localScale = new Vector3(x, y, 1f / localScale3.z);
					base.transform.rotation = parent.rotation;
				}
			}
		}

		private void UpdateCamera()
		{
			Camera = ((!(Camera == null)) ? Camera : ((!(Camera.current == null)) ? Camera.current : Camera.main));
		}

		private LightningBolt GetOrCreateLightningBolt()
		{
			if (lightningBoltCache.Count == 0)
			{
				return new LightningBolt();
			}
			LightningBolt result = lightningBoltCache[lightningBoltCache.Count - 1];
			lightningBoltCache.RemoveAt(lightningBoltCache.Count - 1);
			return result;
		}

		private void UpdateActiveBolts()
		{
			for (int num = activeBolts.Count - 1; num >= 0; num--)
			{
				LightningBolt lightningBolt = activeBolts[num];
				if (!lightningBolt.Update())
				{
					activeBolts.RemoveAt(num);
					lightningBolt.Cleanup();
					lightningBoltCache.Add(lightningBolt);
				}
			}
		}

		private void OnApplicationQuit()
		{
			if (threadState != null)
			{
				threadState.Running = false;
			}
		}

		private void Cleanup()
		{
			foreach (LightningBolt activeBolt in activeBolts)
			{
				activeBolt.Cleanup();
			}
			activeBolts.Clear();
		}

		private void OnDestroy()
		{
			if (threadState != null)
			{
				threadState.TerminateAndWaitForEnd();
			}
			if (lightningMaterialMeshInternal != null)
			{
				UnityEngine.Object.Destroy(lightningMaterialMeshInternal);
			}
			if (lightningMaterialMeshNoGlowInternal != null)
			{
				UnityEngine.Object.Destroy(lightningMaterialMeshNoGlowInternal);
			}
			Cleanup();
		}

		private void OnDisable()
		{
			Cleanup();
		}
	}
}
