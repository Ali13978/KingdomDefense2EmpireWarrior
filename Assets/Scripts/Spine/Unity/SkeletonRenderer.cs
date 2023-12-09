using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Spine.Unity
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	[DisallowMultipleComponent]
	[HelpURL("http://esotericsoftware.com/spine-unity-documentation#Rendering")]
	public class SkeletonRenderer : MonoBehaviour, ISkeletonComponent, ISkeletonDataAssetComponent
	{
		public delegate void SkeletonRendererDelegate(SkeletonRenderer skeletonRenderer);

		public delegate void InstructionDelegate(SkeletonRendererInstruction instruction);

		public SkeletonDataAsset skeletonDataAsset;

		public string initialSkinName;

		public bool initialFlipX;

		public bool initialFlipY;

		[FormerlySerializedAs("submeshSeparators")]
		[SpineSlot("", "", false, true, false)]
		public string[] separatorSlotNames = new string[0];

		[NonSerialized]
		public readonly List<Slot> separatorSlots = new List<Slot>();

		[Range(-0.1f, 0f)]
		public float zSpacing;

		public bool useClipping = true;

		public bool immutableTriangles;

		public bool pmaVertexColors = true;

		public bool clearStateOnDisable;

		public bool tintBlack;

		public bool singleSubmesh;

		[FormerlySerializedAs("calculateNormals")]
		public bool addNormals;

		public bool calculateTangents;

		public bool logErrors;

		public bool disableRenderingOnOverride = true;

		[NonSerialized]
		private readonly Dictionary<Material, Material> customMaterialOverride = new Dictionary<Material, Material>();

		[NonSerialized]
		private readonly Dictionary<Slot, Material> customSlotMaterials = new Dictionary<Slot, Material>();

		private MeshRenderer meshRenderer;

		private MeshFilter meshFilter;

		[NonSerialized]
		public bool valid;

		[NonSerialized]
		public Skeleton skeleton;

		[NonSerialized]
		private readonly SkeletonRendererInstruction currentInstructions = new SkeletonRendererInstruction();

		private readonly MeshGenerator meshGenerator = new MeshGenerator();

		[NonSerialized]
		private readonly MeshRendererBuffers rendererBuffers = new MeshRendererBuffers();

		public SkeletonDataAsset SkeletonDataAsset => skeletonDataAsset;

		public Dictionary<Material, Material> CustomMaterialOverride => customMaterialOverride;

		public Dictionary<Slot, Material> CustomSlotMaterials => customSlotMaterials;

		public Skeleton Skeleton
		{
			get
			{
				Initialize(overwrite: false);
				return skeleton;
			}
		}

		public event SkeletonRendererDelegate OnRebuild;

		public event MeshGeneratorDelegate OnPostProcessVertices;

		private event InstructionDelegate generateMeshOverride;

		public event InstructionDelegate GenerateMeshOverride;

		public static T NewSpineGameObject<T>(SkeletonDataAsset skeletonDataAsset) where T : SkeletonRenderer
		{
			return AddSpineComponent<T>(new GameObject("New Spine GameObject"), skeletonDataAsset);
		}

		public static T AddSpineComponent<T>(GameObject gameObject, SkeletonDataAsset skeletonDataAsset) where T : SkeletonRenderer
		{
			T val = gameObject.AddComponent<T>();
			if (skeletonDataAsset != null)
			{
				val.skeletonDataAsset = skeletonDataAsset;
				val.Initialize(overwrite: false);
			}
			return val;
		}

		public void SetMeshSettings(MeshGenerator.Settings settings)
		{
			calculateTangents = settings.calculateTangents;
			immutableTriangles = settings.immutableTriangles;
			pmaVertexColors = settings.pmaVertexColors;
			tintBlack = settings.tintBlack;
			useClipping = settings.useClipping;
			zSpacing = settings.zSpacing;
			meshGenerator.settings = settings;
		}

		public virtual void Awake()
		{
			Initialize(overwrite: false);
		}

		private void OnDisable()
		{
			if (clearStateOnDisable && valid)
			{
				ClearState();
			}
		}

		private void OnDestroy()
		{
			rendererBuffers.Dispose();
			valid = false;
		}

		public virtual void ClearState()
		{
			meshFilter.sharedMesh = null;
			currentInstructions.Clear();
			if (skeleton != null)
			{
				skeleton.SetToSetupPose();
			}
		}

		public virtual void Initialize(bool overwrite)
		{
			if (valid && !overwrite)
			{
				return;
			}
			if (meshFilter != null)
			{
				meshFilter.sharedMesh = null;
			}
			meshRenderer = GetComponent<MeshRenderer>();
			if (meshRenderer != null)
			{
				meshRenderer.sharedMaterial = null;
			}
			currentInstructions.Clear();
			rendererBuffers.Clear();
			meshGenerator.Begin();
			skeleton = null;
			valid = false;
			if (!skeletonDataAsset)
			{
				if (logErrors)
				{
					UnityEngine.Debug.LogError("Missing SkeletonData asset.", this);
				}
				return;
			}
			SkeletonData skeletonData = skeletonDataAsset.GetSkeletonData(quiet: false);
			if (skeletonData != null)
			{
				valid = true;
				meshFilter = GetComponent<MeshFilter>();
				meshRenderer = GetComponent<MeshRenderer>();
				rendererBuffers.Initialize();
				skeleton = new Skeleton(skeletonData)
				{
					flipX = initialFlipX,
					flipY = initialFlipY
				};
				if (!string.IsNullOrEmpty(initialSkinName) && !string.Equals(initialSkinName, "default", StringComparison.Ordinal))
				{
					skeleton.SetSkin(initialSkinName);
				}
				separatorSlots.Clear();
				for (int i = 0; i < separatorSlotNames.Length; i++)
				{
					separatorSlots.Add(skeleton.FindSlot(separatorSlotNames[i]));
				}
				LateUpdate();
				if (this.OnRebuild != null)
				{
					this.OnRebuild(this);
				}
			}
		}

		public virtual void LateUpdate()
		{
			if (!valid)
			{
				return;
			}
			bool flag = this.generateMeshOverride != null;
			if (!meshRenderer.enabled && !flag)
			{
				return;
			}
			SkeletonRendererInstruction skeletonRendererInstruction = currentInstructions;
			ExposedList<SubmeshInstruction> submeshInstructions = skeletonRendererInstruction.submeshInstructions;
			MeshRendererBuffers.SmartMesh nextMesh = rendererBuffers.GetNextMesh();
			bool flag2;
			if (singleSubmesh)
			{
				MeshGenerator.GenerateSingleSubmeshInstruction(skeletonRendererInstruction, skeleton, skeletonDataAsset.atlasAssets[0].materials[0]);
				if (customMaterialOverride.Count > 0)
				{
					MeshGenerator.TryReplaceMaterials(submeshInstructions, customMaterialOverride);
				}
				meshGenerator.settings = new MeshGenerator.Settings
				{
					pmaVertexColors = pmaVertexColors,
					zSpacing = zSpacing,
					useClipping = useClipping,
					tintBlack = tintBlack,
					calculateTangents = calculateTangents,
					addNormals = addNormals
				};
				meshGenerator.Begin();
				flag2 = SkeletonRendererInstruction.GeometryNotEqual(skeletonRendererInstruction, nextMesh.instructionUsed);
				if (skeletonRendererInstruction.hasActiveClipping)
				{
					meshGenerator.AddSubmesh(submeshInstructions.Items[0], flag2);
				}
				else
				{
					meshGenerator.BuildMeshWithArrays(skeletonRendererInstruction, flag2);
				}
			}
			else
			{
				MeshGenerator.GenerateSkeletonRendererInstruction(skeletonRendererInstruction, skeleton, customSlotMaterials, separatorSlots, flag, immutableTriangles);
				if (customMaterialOverride.Count > 0)
				{
					MeshGenerator.TryReplaceMaterials(submeshInstructions, customMaterialOverride);
				}
				if (flag)
				{
					this.generateMeshOverride(skeletonRendererInstruction);
					if (disableRenderingOnOverride)
					{
						return;
					}
				}
				flag2 = SkeletonRendererInstruction.GeometryNotEqual(skeletonRendererInstruction, nextMesh.instructionUsed);
				meshGenerator.settings = new MeshGenerator.Settings
				{
					pmaVertexColors = pmaVertexColors,
					zSpacing = zSpacing,
					useClipping = useClipping,
					tintBlack = tintBlack,
					calculateTangents = calculateTangents,
					addNormals = addNormals
				};
				meshGenerator.Begin();
				if (skeletonRendererInstruction.hasActiveClipping)
				{
					meshGenerator.BuildMesh(skeletonRendererInstruction, flag2);
				}
				else
				{
					meshGenerator.BuildMeshWithArrays(skeletonRendererInstruction, flag2);
				}
			}
			if (this.OnPostProcessVertices != null)
			{
				this.OnPostProcessVertices(meshGenerator.Buffers);
			}
			Mesh mesh = nextMesh.mesh;
			meshGenerator.FillVertexData(mesh);
			rendererBuffers.UpdateSharedMaterials(submeshInstructions);
			if (flag2)
			{
				meshGenerator.FillTriangles(mesh);
				meshRenderer.sharedMaterials = rendererBuffers.GetUpdatedSharedMaterialsArray();
			}
			else if (rendererBuffers.MaterialsChangedInLastUpdate())
			{
				meshRenderer.sharedMaterials = rendererBuffers.GetUpdatedSharedMaterialsArray();
			}
			meshFilter.sharedMesh = mesh;
			nextMesh.instructionUsed.Set(skeletonRendererInstruction);
		}
	}
}
