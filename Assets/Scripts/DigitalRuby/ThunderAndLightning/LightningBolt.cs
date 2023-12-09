using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningBolt
	{
		public class LineRendererMesh
		{
			private static readonly Vector2 uv1 = new Vector2(0f, 0f);

			private static readonly Vector2 uv2 = new Vector2(1f, 0f);

			private static readonly Vector2 uv3 = new Vector2(0f, 1f);

			private static readonly Vector2 uv4 = new Vector2(1f, 1f);

			private readonly List<int> indices = new List<int>();

			private readonly List<Vector3> vertices = new List<Vector3>();

			private readonly List<Vector4> lineDirs = new List<Vector4>();

			private readonly List<Color32> colors = new List<Color32>();

			private readonly List<Vector3> ends = new List<Vector3>();

			private readonly List<Vector4> texCoordsAndGlowModifiers = new List<Vector4>();

			private readonly List<Vector4> fadeLifetimes = new List<Vector4>();

			private const int boundsPadder = 1000000000;

			private int currentBoundsMinX = 1147483647;

			private int currentBoundsMinY = 1147483647;

			private int currentBoundsMinZ = 1147483647;

			private int currentBoundsMaxX = -1147483648;

			private int currentBoundsMaxY = -1147483648;

			private int currentBoundsMaxZ = -1147483648;

			private Mesh mesh;

			private MeshFilter meshFilter;

			private MeshRenderer meshRenderer;

			public GameObject GameObject
			{
				get;
				private set;
			}

			public Material Material
			{
				get
				{
					return meshRenderer.sharedMaterial;
				}
				set
				{
					meshRenderer.sharedMaterial = value;
				}
			}

			public MeshRenderer MeshRenderer => meshRenderer;

			public int Tag
			{
				get;
				set;
			}

			public Action<LightningCustomTransformStateInfo> CustomTransform
			{
				get;
				set;
			}

			public Transform Transform
			{
				get;
				private set;
			}

			public bool Empty => vertices.Count == 0;

			public LineRendererMesh()
			{
				GameObject = new GameObject("LightningBoltMeshRenderer");
				GameObject.SetActive(value: false);
				mesh = new Mesh
				{
					name = "ProceduralLightningMesh"
				};
				mesh.MarkDynamic();
				meshFilter = GameObject.AddComponent<MeshFilter>();
				meshFilter.sharedMesh = mesh;
				meshRenderer = GameObject.AddComponent<MeshRenderer>();
				meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
				meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
				meshRenderer.lightProbeUsage = LightProbeUsage.Off;
				meshRenderer.receiveShadows = false;
				Transform = GameObject.GetComponent<Transform>();
			}

			public void PopulateMesh()
			{
				if (vertices.Count == 0)
				{
					mesh.Clear();
				}
				else
				{
					PopulateMeshInternal();
				}
			}

			public bool PrepareForLines(int lineCount)
			{
				int num = lineCount * 4;
				if (vertices.Count + num > 64999)
				{
					return false;
				}
				return true;
			}

			public void BeginLine(Vector3 start, Vector3 end, float radius, Color32 color, float colorIntensity, Vector4 fadeLifeTime, float glowWidthModifier, float glowIntensity)
			{
				Vector4 dir = end - start;
				dir.w = radius;
				AppendLineInternal(ref start, ref end, ref dir, ref dir, ref dir, color, colorIntensity, ref fadeLifeTime, glowWidthModifier, glowIntensity);
			}

			public void AppendLine(Vector3 start, Vector3 end, float radius, Color32 color, float colorIntensity, Vector4 fadeLifeTime, float glowWidthModifier, float glowIntensity)
			{
				Vector4 dir = end - start;
				dir.w = radius;
				Vector4 dirPrev = lineDirs[lineDirs.Count - 3];
				Vector4 dirPrev2 = lineDirs[lineDirs.Count - 1];
				AppendLineInternal(ref start, ref end, ref dir, ref dirPrev, ref dirPrev2, color, colorIntensity, ref fadeLifeTime, glowWidthModifier, glowIntensity);
			}

			public void Reset()
			{
				CustomTransform = null;
				Tag++;
				GameObject.SetActive(value: false);
				mesh.Clear();
				indices.Clear();
				vertices.Clear();
				colors.Clear();
				lineDirs.Clear();
				ends.Clear();
				texCoordsAndGlowModifiers.Clear();
				fadeLifetimes.Clear();
				currentBoundsMaxX = (currentBoundsMaxY = (currentBoundsMaxZ = -1147483648));
				currentBoundsMinX = (currentBoundsMinY = (currentBoundsMinZ = 1147483647));
			}

			private void PopulateMeshInternal()
			{
				GameObject.SetActive(value: true);
				mesh.SetVertices(vertices);
				mesh.SetTangents(lineDirs);
				mesh.SetColors(colors);
				mesh.SetUVs(0, texCoordsAndGlowModifiers);
				mesh.SetUVs(1, fadeLifetimes);
				mesh.SetNormals(ends);
				mesh.SetTriangles(indices, 0);
				Bounds bounds = default(Bounds);
				Vector3 b = new Vector3(currentBoundsMinX - 2, currentBoundsMinY - 2, currentBoundsMinZ - 2);
				Vector3 a = new Vector3(currentBoundsMaxX + 2, currentBoundsMaxY + 2, currentBoundsMaxZ + 2);
				bounds.center = (a + b) * 0.5f;
				bounds.size = (a - b) * 1.2f;
				mesh.bounds = bounds;
			}

			private void UpdateBounds(ref Vector3 point1, ref Vector3 point2)
			{
				int num = (int)point1.x - (int)point2.x;
				num &= num >> 31;
				int num2 = (int)point2.x + num;
				int num3 = (int)point1.x - num;
				num = currentBoundsMinX - num2;
				num &= num >> 31;
				currentBoundsMinX = num2 + num;
				num = currentBoundsMaxX - num3;
				num &= num >> 31;
				currentBoundsMaxX -= num;
				int num4 = (int)point1.y - (int)point2.y;
				num4 &= num4 >> 31;
				int num5 = (int)point2.y + num4;
				int num6 = (int)point1.y - num4;
				num4 = currentBoundsMinY - num5;
				num4 &= num4 >> 31;
				currentBoundsMinY = num5 + num4;
				num4 = currentBoundsMaxY - num6;
				num4 &= num4 >> 31;
				currentBoundsMaxY -= num4;
				int num7 = (int)point1.z - (int)point2.z;
				num7 &= num7 >> 31;
				int num8 = (int)point2.z + num7;
				int num9 = (int)point1.z - num7;
				num7 = currentBoundsMinZ - num8;
				num7 &= num7 >> 31;
				currentBoundsMinZ = num8 + num7;
				num7 = currentBoundsMaxZ - num9;
				num7 &= num7 >> 31;
				currentBoundsMaxZ -= num7;
			}

			private void AddIndices()
			{
				int num = vertices.Count;
				indices.Add(num++);
				indices.Add(num++);
				indices.Add(num);
				indices.Add(num--);
				indices.Add(num);
				indices.Add(num += 2);
			}

			private void AppendLineInternal(ref Vector3 start, ref Vector3 end, ref Vector4 dir, ref Vector4 dirPrev1, ref Vector4 dirPrev2, Color32 color, float colorIntensity, ref Vector4 fadeLifeTime, float glowWidthModifier, float glowIntensity)
			{
				AddIndices();
				color.a = (byte)Mathf.Lerp(0f, 255f, colorIntensity * 0.1f);
				Vector2 vector = uv1;
				float x = vector.x;
				Vector2 vector2 = uv1;
				Vector4 item = new Vector4(x, vector2.y, glowWidthModifier, glowIntensity);
				vertices.Add(start);
				lineDirs.Add(dirPrev1);
				colors.Add(color);
				ends.Add(dir);
				vertices.Add(end);
				lineDirs.Add(dir);
				colors.Add(color);
				ends.Add(dir);
				dir.w = 0f - dir.w;
				vertices.Add(start);
				lineDirs.Add(dirPrev2);
				colors.Add(color);
				ends.Add(dir);
				vertices.Add(end);
				lineDirs.Add(dir);
				colors.Add(color);
				ends.Add(dir);
				texCoordsAndGlowModifiers.Add(item);
				Vector2 vector3 = uv2;
				item.x = vector3.x;
				Vector2 vector4 = uv2;
				item.y = vector4.y;
				texCoordsAndGlowModifiers.Add(item);
				Vector2 vector5 = uv3;
				item.x = vector5.x;
				Vector2 vector6 = uv3;
				item.y = vector6.y;
				texCoordsAndGlowModifiers.Add(item);
				Vector2 vector7 = uv4;
				item.x = vector7.x;
				Vector2 vector8 = uv4;
				item.y = vector8.y;
				texCoordsAndGlowModifiers.Add(item);
				fadeLifetimes.Add(fadeLifeTime);
				fadeLifetimes.Add(fadeLifeTime);
				fadeLifetimes.Add(fadeLifeTime);
				fadeLifetimes.Add(fadeLifeTime);
				UpdateBounds(ref start, ref end);
			}
		}

		public static int MaximumLightCount = 128;

		public static int MaximumLightsPerBatch = 8;

		private DateTime startTimeOffset;

		private LightningBoltDependencies dependencies;

		private float elapsedTime;

		private float lifeTime;

		private float maxLifeTime;

		private bool hasLight;

		private float timeSinceLevelLoad;

		private readonly List<LightningBoltSegmentGroup> segmentGroups = new List<LightningBoltSegmentGroup>();

		private readonly List<LightningBoltSegmentGroup> segmentGroupsWithLight = new List<LightningBoltSegmentGroup>();

		private readonly List<LineRendererMesh> activeLineRenderers = new List<LineRendererMesh>();

		private static int lightCount;

		private static readonly List<LineRendererMesh> lineRendererCache = new List<LineRendererMesh>();

		private static readonly List<LightningBoltSegmentGroup> groupCache = new List<LightningBoltSegmentGroup>();

		private static readonly List<Light> lightCache = new List<Light>();

		public float MinimumDelay
		{
			get;
			private set;
		}

		public bool HasGlow
		{
			get;
			private set;
		}

		public bool IsActive => elapsedTime < lifeTime;

		public CameraMode CameraMode
		{
			get;
			private set;
		}

		public void SetupLightningBolt(LightningBoltDependencies dependencies)
		{
			if (dependencies == null || dependencies.Parameters.Count == 0)
			{
				UnityEngine.Debug.LogError("Lightning bolt dependencies must not be null");
				return;
			}
			if (this.dependencies != null)
			{
				UnityEngine.Debug.LogError("This lightning bolt is already in use!");
				return;
			}
			this.dependencies = dependencies;
			CameraMode = dependencies.CameraMode;
			timeSinceLevelLoad = Time.timeSinceLevelLoad;
			CheckForGlow(dependencies.Parameters);
			MinimumDelay = float.MaxValue;
			startTimeOffset = DateTime.Now;
			if (dependencies.ThreadState != null)
			{
				dependencies.ThreadState.AddActionForBackgroundThread(ProcessAllLightningParameters);
			}
			else
			{
				ProcessAllLightningParameters();
			}
		}

		public bool Update()
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > maxLifeTime)
			{
				return false;
			}
			if (hasLight)
			{
				UpdateLights();
			}
			return true;
		}

		public void Cleanup()
		{
			foreach (LightningBoltSegmentGroup item in segmentGroupsWithLight)
			{
				foreach (Light light in item.Lights)
				{
					CleanupLight(light);
				}
				item.Lights.Clear();
			}
			lock (groupCache)
			{
				foreach (LightningBoltSegmentGroup segmentGroup in segmentGroups)
				{
					groupCache.Add(segmentGroup);
				}
			}
			hasLight = false;
			elapsedTime = 0f;
			lifeTime = 0f;
			maxLifeTime = 0f;
			if (dependencies != null)
			{
				dependencies.ReturnToCache(dependencies);
				dependencies = null;
			}
			foreach (LineRendererMesh activeLineRenderer in activeLineRenderers)
			{
				if (activeLineRenderer != null)
				{
					activeLineRenderer.Reset();
					lineRendererCache.Add(activeLineRenderer);
				}
			}
			segmentGroups.Clear();
			segmentGroupsWithLight.Clear();
			activeLineRenderers.Clear();
		}

		public LightningBoltSegmentGroup AddGroup()
		{
			LightningBoltSegmentGroup lightningBoltSegmentGroup;
			lock (groupCache)
			{
				if (groupCache.Count == 0)
				{
					lightningBoltSegmentGroup = new LightningBoltSegmentGroup();
				}
				else
				{
					int index = groupCache.Count - 1;
					lightningBoltSegmentGroup = groupCache[index];
					lightningBoltSegmentGroup.Reset();
					groupCache.RemoveAt(index);
				}
			}
			segmentGroups.Add(lightningBoltSegmentGroup);
			return lightningBoltSegmentGroup;
		}

		public static void ClearCache()
		{
			foreach (LineRendererMesh item in lineRendererCache)
			{
				if (item != null)
				{
					UnityEngine.Object.Destroy(item.GameObject);
				}
			}
			foreach (Light item2 in lightCache)
			{
				if (item2 != null)
				{
					UnityEngine.Object.Destroy(item2.gameObject);
				}
			}
			lineRendererCache.Clear();
			lightCache.Clear();
			lock (groupCache)
			{
				groupCache.Clear();
			}
		}

		private void CleanupLight(Light l)
		{
			if (l != null)
			{
				dependencies.LightRemoved(l);
				lightCache.Add(l);
				l.gameObject.SetActive(value: false);
				lightCount--;
			}
		}

		private void EnableLineRenderer(LineRendererMesh lineRenderer, int tag)
		{
			if (lineRenderer != null && lineRenderer.GameObject != null && lineRenderer.Tag == tag && IsActive)
			{
				lineRenderer.PopulateMesh();
			}
		}

		private IEnumerator EnableLastRendererCoRoutine()
		{
			LineRendererMesh lineRenderer = activeLineRenderers[activeLineRenderers.Count - 1];
			int tag = ++lineRenderer.Tag;
			yield return new WaitForSeconds(MinimumDelay);
			EnableLineRenderer(lineRenderer, tag);
		}

		private LineRendererMesh GetOrCreateLineRenderer()
		{
			IL_0000:
			LineRendererMesh lineRendererMesh;
			do
			{
				if (lineRendererCache.Count == 0)
				{
					lineRendererMesh = new LineRendererMesh();
					break;
				}
				int index = lineRendererCache.Count - 1;
				lineRendererMesh = lineRendererCache[index];
				lineRendererCache.RemoveAt(index);
			}
			while (lineRendererMesh == null || lineRendererMesh.Transform == null);
			lineRendererMesh.Transform.parent = null;
			lineRendererMesh.Transform.rotation = Quaternion.identity;
			lineRendererMesh.Transform.localScale = Vector3.one;
			lineRendererMesh.Transform.parent = dependencies.Parent.transform;
			lineRendererMesh.GameObject.layer = dependencies.Parent.layer;
			if (dependencies.UseWorldSpace)
			{
				lineRendererMesh.GameObject.transform.position = Vector3.zero;
			}
			else
			{
				lineRendererMesh.GameObject.transform.localPosition = Vector3.zero;
			}
			lineRendererMesh.Material = ((!HasGlow) ? dependencies.LightningMaterialMeshNoGlow : dependencies.LightningMaterialMesh);
			lineRendererMesh.MeshRenderer.sortingLayerName = dependencies.SortLayerName;
			lineRendererMesh.MeshRenderer.sortingOrder = dependencies.SortOrderInLayer;
			activeLineRenderers.Add(lineRendererMesh);
			return lineRendererMesh;
			IL_005f:
			goto IL_0000;
		}

		private void RenderGroup(LightningBoltSegmentGroup group, LightningBoltParameters p)
		{
			if (group.SegmentCount == 0)
			{
				return;
			}
			float num = (float)(DateTime.Now - startTimeOffset).TotalSeconds;
			float num2 = timeSinceLevelLoad + group.Delay + num;
			Vector4 fadeLifeTime = new Vector4(num2, num2 + group.PeakStart, num2 + group.PeakEnd, num2 + group.LifeTime);
			float num3 = group.LineWidth * 0.5f * LightningBoltParameters.Scale;
			int num4 = group.Segments.Count - group.StartIndex;
			float num5 = (num3 - num3 * group.EndWidthMultiplier) / (float)num4;
			float num6;
			if (p.GrowthMultiplier > 0f)
			{
				num6 = group.LifeTime / (float)num4 * p.GrowthMultiplier;
				num = 0f;
			}
			else
			{
				num6 = 0f;
				num = 0f;
			}
			LineRendererMesh currentLineRenderer = (activeLineRenderers.Count != 0) ? activeLineRenderers[activeLineRenderers.Count - 1] : GetOrCreateLineRenderer();
			if (!currentLineRenderer.PrepareForLines(num4))
			{
				if (currentLineRenderer.CustomTransform != null)
				{
					return;
				}
				if (dependencies.ThreadState != null)
				{
					dependencies.ThreadState.AddActionForMainThread(delegate
					{
						EnableCurrentLineRenderer();
						currentLineRenderer = GetOrCreateLineRenderer();
					}, waitForAction: true);
				}
				else
				{
					EnableCurrentLineRenderer();
					currentLineRenderer = GetOrCreateLineRenderer();
				}
			}
			LineRendererMesh lineRendererMesh = currentLineRenderer;
			LightningBoltSegment lightningBoltSegment = group.Segments[group.StartIndex];
			Vector3 start = lightningBoltSegment.Start;
			LightningBoltSegment lightningBoltSegment2 = group.Segments[group.StartIndex];
			lineRendererMesh.BeginLine(start, lightningBoltSegment2.End, num3, group.Color, p.Intensity, fadeLifeTime, p.GlowWidthMultiplier, p.GlowIntensity);
			for (int i = group.StartIndex + 1; i < group.Segments.Count; i++)
			{
				num3 -= num5;
				if (p.GrowthMultiplier < 1f)
				{
					num += num6;
					fadeLifeTime = new Color(num2 + num, num2 + group.PeakStart + num, num2 + group.PeakEnd, num2 + group.LifeTime);
				}
				LineRendererMesh lineRendererMesh2 = currentLineRenderer;
				LightningBoltSegment lightningBoltSegment3 = group.Segments[i];
				Vector3 start2 = lightningBoltSegment3.Start;
				LightningBoltSegment lightningBoltSegment4 = group.Segments[i];
				lineRendererMesh2.AppendLine(start2, lightningBoltSegment4.End, num3, group.Color, p.Intensity, fadeLifeTime, p.GlowWidthMultiplier, p.GlowIntensity);
			}
		}

		private static IEnumerator NotifyBolt(LightningBoltDependencies dependencies, LightningBoltParameters p, Transform transform, Vector3 start, Vector3 end)
		{
			float delay = p.delaySeconds;
			float lifeTime = p.LifeTime;
			yield return new WaitForSeconds(delay);
			if (dependencies.LightningBoltStarted != null)
			{
				dependencies.LightningBoltStarted(p, start, end);
			}
			LightningCustomTransformStateInfo state = (p.CustomTransform != null) ? LightningCustomTransformStateInfo.GetOrCreateStateInfo() : null;
			if (state != null)
			{
				state.BoltStartPosition = start;
				state.BoltEndPosition = end;
				state.State = LightningCustomTransformState.Started;
				state.Transform = transform;
				p.CustomTransform(state);
				state.State = LightningCustomTransformState.Executing;
			}
			if (p.CustomTransform == null)
			{
				yield return new WaitForSeconds(lifeTime);
			}
			else
			{
				while (lifeTime > 0f)
				{
					p.CustomTransform(state);
					lifeTime -= Time.deltaTime;
					yield return null;
				}
			}
			if (p.CustomTransform != null)
			{
				state.State = LightningCustomTransformState.Ended;
				p.CustomTransform(state);
				LightningCustomTransformStateInfo.ReturnStateInfoToCache(state);
			}
			if (dependencies.LightningBoltEnded != null)
			{
				dependencies.LightningBoltEnded(p, start, end);
			}
			LightningBoltParameters.ReturnParametersToCache(p);
		}

		private void ProcessParameters(LightningBoltParameters p, RangeOfFloats delay)
		{
			MinimumDelay = Mathf.Min(delay.Minimum, MinimumDelay);
			p.delaySeconds = delay.Random(p.Random);
			p.generationWhereForksStop = p.Generations - p.GenerationWhereForksStopSubtractor;
			lifeTime = Mathf.Max(p.LifeTime + p.delaySeconds, lifeTime);
			maxLifeTime = Mathf.Max(lifeTime, maxLifeTime);
			p.forkednessCalculated = (int)Mathf.Ceil(p.Forkedness * (float)p.Generations);
			if (p.Generations > 0)
			{
				p.Generator = (p.Generator ?? LightningGenerator.GeneratorInstance);
				p.Generator.GenerateLightningBolt(this, p, out Vector3 start, out Vector3 end);
				p.Start = start;
				p.End = end;
			}
		}

		private void ProcessAllLightningParameters()
		{
			int maxLights = MaximumLightsPerBatch / dependencies.Parameters.Count;
			RangeOfFloats delay = default(RangeOfFloats);
			List<int> list = new List<int>(dependencies.Parameters.Count + 1);
			int num = 0;
			foreach (LightningBoltParameters parameter in dependencies.Parameters)
			{
				delay.Minimum = parameter.DelayRange.Minimum + parameter.Delay;
				delay.Maximum = parameter.DelayRange.Maximum + parameter.Delay;
				parameter.maxLights = maxLights;
				list.Add(segmentGroups.Count);
				ProcessParameters(parameter, delay);
			}
			list.Add(segmentGroups.Count);
			LightningBoltDependencies dependenciesRef = dependencies;
			foreach (LightningBoltParameters parameters in dependenciesRef.Parameters)
			{
				Transform transform = RenderLightningBolt(parameters.quality, parameters.Generations, list[num], list[++num], parameters);
				if (dependenciesRef.ThreadState != null)
				{
					dependenciesRef.ThreadState.AddActionForMainThread(delegate
					{
						dependenciesRef.StartCoroutine(NotifyBolt(dependenciesRef, parameters, transform, parameters.Start, parameters.End));
					});
				}
				else
				{
					dependenciesRef.StartCoroutine(NotifyBolt(dependenciesRef, parameters, transform, parameters.Start, parameters.End));
				}
			}
			if (dependencies.ThreadState != null)
			{
				dependencies.ThreadState.AddActionForMainThread(EnableCurrentLineRendererFromThread);
				return;
			}
			EnableCurrentLineRenderer();
			dependencies.AddActiveBolt(this);
		}

		private void EnableCurrentLineRendererFromThread()
		{
			EnableCurrentLineRenderer();
			dependencies.ThreadState = null;
			dependencies.AddActiveBolt(this);
		}

		private void EnableCurrentLineRenderer()
		{
			if (activeLineRenderers.Count != 0)
			{
				if (MinimumDelay <= 0f)
				{
					EnableLineRenderer(activeLineRenderers[activeLineRenderers.Count - 1], activeLineRenderers[activeLineRenderers.Count - 1].Tag);
				}
				else
				{
					dependencies.StartCoroutine(EnableLastRendererCoRoutine());
				}
			}
		}

		private void RenderParticleSystems(Vector3 start, Vector3 end, float trunkWidth, float lifeTime, float delaySeconds)
		{
			if (trunkWidth > 0f)
			{
				if (dependencies.OriginParticleSystem != null)
				{
					dependencies.StartCoroutine(GenerateParticleCoRoutine(dependencies.OriginParticleSystem, start, delaySeconds));
				}
				if (dependencies.DestParticleSystem != null)
				{
					dependencies.StartCoroutine(GenerateParticleCoRoutine(dependencies.DestParticleSystem, end, delaySeconds + lifeTime * 0.8f));
				}
			}
		}

		private Transform RenderLightningBolt(LightningBoltQualitySetting quality, int generations, int startGroupIndex, int endGroupIndex, LightningBoltParameters parameters)
		{
			if (segmentGroups.Count == 0 || startGroupIndex >= segmentGroups.Count || endGroupIndex > segmentGroups.Count)
			{
				return null;
			}
			Transform result = null;
			LightningLightParameters lp = parameters.LightParameters;
			if (lp != null)
			{
				if (hasLight |= lp.HasLight)
				{
					lp.LightPercent = Mathf.Clamp(lp.LightPercent, Mathf.Epsilon, 1f);
					lp.LightShadowPercent = Mathf.Clamp(lp.LightShadowPercent, 0f, 1f);
				}
				else
				{
					lp = null;
				}
			}
			LightningBoltSegmentGroup lightningBoltSegmentGroup = segmentGroups[startGroupIndex];
			LightningBoltSegment lightningBoltSegment = lightningBoltSegmentGroup.Segments[lightningBoltSegmentGroup.StartIndex];
			Vector3 start = lightningBoltSegment.Start;
			LightningBoltSegment lightningBoltSegment2 = lightningBoltSegmentGroup.Segments[lightningBoltSegmentGroup.StartIndex + lightningBoltSegmentGroup.SegmentCount - 1];
			Vector3 end = lightningBoltSegment2.End;
			parameters.FadePercent = Mathf.Clamp(parameters.FadePercent, 0f, 0.5f);
			if (parameters.CustomTransform != null)
			{
				LineRendererMesh currentLineRenderer = (activeLineRenderers.Count != 0 && activeLineRenderers[activeLineRenderers.Count - 1].Empty) ? activeLineRenderers[activeLineRenderers.Count - 1] : null;
				if (currentLineRenderer == null)
				{
					if (dependencies.ThreadState != null)
					{
						dependencies.ThreadState.AddActionForMainThread(delegate
						{
							EnableCurrentLineRenderer();
							currentLineRenderer = GetOrCreateLineRenderer();
						}, waitForAction: true);
					}
					else
					{
						EnableCurrentLineRenderer();
						currentLineRenderer = GetOrCreateLineRenderer();
					}
				}
				if (currentLineRenderer == null)
				{
					return null;
				}
				currentLineRenderer.CustomTransform = parameters.CustomTransform;
				result = currentLineRenderer.Transform;
			}
			for (int i = startGroupIndex; i < endGroupIndex; i++)
			{
				LightningBoltSegmentGroup lightningBoltSegmentGroup2 = segmentGroups[i];
				lightningBoltSegmentGroup2.Delay = parameters.delaySeconds;
				lightningBoltSegmentGroup2.LifeTime = parameters.LifeTime;
				lightningBoltSegmentGroup2.PeakStart = lightningBoltSegmentGroup2.LifeTime * parameters.FadePercent;
				lightningBoltSegmentGroup2.PeakEnd = lightningBoltSegmentGroup2.LifeTime - lightningBoltSegmentGroup2.PeakStart;
				float num = lightningBoltSegmentGroup2.PeakEnd - lightningBoltSegmentGroup2.PeakStart;
				float num2 = lightningBoltSegmentGroup2.LifeTime - lightningBoltSegmentGroup2.PeakEnd;
				lightningBoltSegmentGroup2.PeakStart *= parameters.FadeInMultiplier;
				lightningBoltSegmentGroup2.PeakEnd = lightningBoltSegmentGroup2.PeakStart + num * parameters.FadeFullyLitMultiplier;
				lightningBoltSegmentGroup2.LifeTime = lightningBoltSegmentGroup2.PeakEnd + num2 * parameters.FadeOutMultiplier;
				lightningBoltSegmentGroup2.LightParameters = lp;
				RenderGroup(lightningBoltSegmentGroup2, parameters);
			}
			if (dependencies.ThreadState != null)
			{
				dependencies.ThreadState.AddActionForMainThread(delegate
				{
					RenderParticleSystems(start, end, parameters.TrunkWidth, parameters.LifeTime, parameters.delaySeconds);
					if (lp != null)
					{
						CreateLightsForGroup(segmentGroups[startGroupIndex], lp, quality, parameters.maxLights);
					}
				});
			}
			else
			{
				RenderParticleSystems(start, end, parameters.TrunkWidth, parameters.LifeTime, parameters.delaySeconds);
				if (lp != null)
				{
					CreateLightsForGroup(segmentGroups[startGroupIndex], lp, quality, parameters.maxLights);
				}
			}
			return result;
		}

		private void CreateLightsForGroup(LightningBoltSegmentGroup group, LightningLightParameters lp, LightningBoltQualitySetting quality, int maxLights)
		{
			if (lightCount == MaximumLightCount || maxLights <= 0)
			{
				return;
			}
			float num = (lifeTime - group.PeakEnd) * lp.FadeOutMultiplier;
			float num2 = (group.PeakEnd - group.PeakStart) * lp.FadeFullyLitMultiplier;
			float num3 = group.PeakStart * lp.FadeInMultiplier;
			float num4 = num3 + num2;
			float num5 = num4 + num;
			maxLifeTime = Mathf.Max(maxLifeTime, group.Delay + num5);
			segmentGroupsWithLight.Add(group);
			int segmentCount = group.SegmentCount;
			float num6;
			float num7;
			if (quality == LightningBoltQualitySetting.LimitToQualitySetting)
			{
				int qualityLevel = QualitySettings.GetQualityLevel();
				if (LightningBoltParameters.QualityMaximums.TryGetValue(qualityLevel, out LightningQualityMaximum value))
				{
					num6 = Mathf.Min(lp.LightPercent, value.MaximumLightPercent);
					num7 = Mathf.Min(lp.LightShadowPercent, value.MaximumShadowPercent);
				}
				else
				{
					UnityEngine.Debug.LogError("Unable to read lightning quality for level " + qualityLevel.ToString());
					num6 = lp.LightPercent;
					num7 = lp.LightShadowPercent;
				}
			}
			else
			{
				num6 = lp.LightPercent;
				num7 = lp.LightShadowPercent;
			}
			maxLights = Mathf.Max(1, Mathf.Min(maxLights, (int)((float)segmentCount * num6)));
			int num8 = Mathf.Max(1, segmentCount / maxLights);
			int num9 = maxLights - (int)((float)maxLights * num7);
			int nthShadowCounter = num9;
			for (int i = group.StartIndex + (int)((float)num8 * 0.5f); i < group.Segments.Count && !AddLightToGroup(group, lp, i, num8, num9, ref maxLights, ref nthShadowCounter); i += num8)
			{
			}
		}

		private bool AddLightToGroup(LightningBoltSegmentGroup group, LightningLightParameters lp, int segmentIndex, int nthLight, int nthShadows, ref int maxLights, ref int nthShadowCounter)
		{
			Light orCreateLight = GetOrCreateLight(lp);
			group.Lights.Add(orCreateLight);
			LightningBoltSegment lightningBoltSegment = group.Segments[segmentIndex];
			Vector3 start = lightningBoltSegment.Start;
			LightningBoltSegment lightningBoltSegment2 = group.Segments[segmentIndex];
			Vector3 vector = (start + lightningBoltSegment2.End) * 0.5f;
			if (dependencies.CameraIsOrthographic)
			{
				if (dependencies.CameraMode == CameraMode.OrthographicXZ)
				{
					vector.y = dependencies.CameraPos.y + lp.OrthographicOffset;
				}
				else
				{
					vector.z = dependencies.CameraPos.z + lp.OrthographicOffset;
				}
			}
			if (dependencies.UseWorldSpace)
			{
				orCreateLight.gameObject.transform.position = vector;
			}
			else
			{
				orCreateLight.gameObject.transform.localPosition = vector;
			}
			if (lp.LightShadowPercent == 0f || ++nthShadowCounter < nthShadows)
			{
				orCreateLight.shadows = LightShadows.None;
			}
			else
			{
				orCreateLight.shadows = LightShadows.Soft;
				nthShadowCounter = 0;
			}
			return ++lightCount == MaximumLightCount || --maxLights == 0;
		}

		private Light GetOrCreateLight(LightningLightParameters lp)
		{
			IL_0000:
			Light light;
			do
			{
				if (lightCache.Count == 0)
				{
					GameObject gameObject = new GameObject("LightningBoltLight");
					light = gameObject.AddComponent<Light>();
					light.type = LightType.Point;
					break;
				}
				light = lightCache[lightCache.Count - 1];
				lightCache.RemoveAt(lightCache.Count - 1);
			}
			while (light == null);
			light.bounceIntensity = lp.BounceIntensity;
			light.shadowNormalBias = lp.ShadowNormalBias;
			light.color = lp.LightColor;
			light.renderMode = lp.RenderMode;
			light.range = lp.LightRange;
			light.shadowStrength = lp.ShadowStrength;
			light.shadowBias = lp.ShadowBias;
			light.intensity = 0f;
			light.gameObject.transform.parent = dependencies.Parent.transform;
			light.gameObject.SetActive(value: true);
			dependencies.LightAdded(light);
			return light;
			IL_0070:
			goto IL_0000;
		}

		private void UpdateLight(LightningLightParameters lp, IEnumerable<Light> lights, float delay, float peakStart, float peakEnd, float lifeTime)
		{
			if (elapsedTime < delay)
			{
				return;
			}
			float num = (lifeTime - peakEnd) * lp.FadeOutMultiplier;
			float num2 = (peakEnd - peakStart) * lp.FadeFullyLitMultiplier;
			peakStart *= lp.FadeInMultiplier;
			peakEnd = peakStart + num2;
			lifeTime = peakEnd + num;
			float num3 = elapsedTime - delay;
			if (num3 >= peakStart)
			{
				if (num3 <= peakEnd)
				{
					foreach (Light light in lights)
					{
						light.intensity = lp.LightIntensity;
					}
					return;
				}
				float t = (num3 - peakEnd) / (lifeTime - peakEnd);
				foreach (Light light2 in lights)
				{
					light2.intensity = Mathf.Lerp(lp.LightIntensity, 0f, t);
				}
			}
			else
			{
				float t2 = num3 / peakStart;
				foreach (Light light3 in lights)
				{
					light3.intensity = Mathf.Lerp(0f, lp.LightIntensity, t2);
				}
			}
		}

		private void UpdateLights()
		{
			foreach (LightningBoltSegmentGroup item in segmentGroupsWithLight)
			{
				UpdateLight(item.LightParameters, item.Lights, item.Delay, item.PeakStart, item.PeakEnd, item.LifeTime);
			}
		}

		private IEnumerator GenerateParticleCoRoutine(ParticleSystem p, Vector3 pos, float delay)
		{
			yield return new WaitForSeconds(delay);
			p.transform.position = pos;
			if (p.emission.burstCount > 0)
			{
				ParticleSystem.Burst[] array = new ParticleSystem.Burst[p.emission.burstCount];
				p.emission.GetBursts(array);
				int count3 = UnityEngine.Random.Range(array[0].minCount, array[0].maxCount + 1);
				p.Emit(count3);
			}
			else
			{
				ParticleSystem.MinMaxCurve rateOverTime = p.emission.rateOverTime;
				int count3 = (int)((rateOverTime.constantMax - rateOverTime.constantMin) * 0.5f);
				count3 = UnityEngine.Random.Range(count3, count3 * 2);
				p.Emit(count3);
			}
		}

		private void CheckForGlow(IEnumerable<LightningBoltParameters> parameters)
		{
			foreach (LightningBoltParameters parameter in parameters)
			{
				HasGlow = (parameter.GlowIntensity >= Mathf.Epsilon && parameter.GlowWidthMultiplier >= Mathf.Epsilon);
				if (HasGlow)
				{
					break;
				}
			}
		}
	}
}
