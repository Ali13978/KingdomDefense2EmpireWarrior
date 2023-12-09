using UnityEngine;
using UnityEngine.UI;

namespace Spine.Unity
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(CanvasRenderer), typeof(RectTransform))]
	[DisallowMultipleComponent]
	[AddComponentMenu("Spine/SkeletonGraphic (Unity UI Canvas)")]
	public class SkeletonGraphic : MaskableGraphic, ISkeletonComponent, IAnimationStateComponent, ISkeletonAnimation, ISkeletonDataAssetComponent
	{
		public SkeletonDataAsset skeletonDataAsset;

		[SpineSkin("", "skeletonDataAsset", true, false)]
		public string initialSkinName = "default";

		public bool initialFlipX;

		public bool initialFlipY;

		[SpineAnimation("", "skeletonDataAsset", true, false)]
		public string startingAnimation;

		public bool startingLoop;

		public float timeScale = 1f;

		public bool freeze;

		public bool unscaledTime;

		private Texture overrideTexture;

		protected Skeleton skeleton;

		protected AnimationState state;

		[SerializeField]
		protected MeshGenerator meshGenerator = new MeshGenerator();

		private DoubleBuffered<MeshRendererBuffers.SmartMesh> meshBuffers;

		private SkeletonRendererInstruction currentInstructions = new SkeletonRendererInstruction();

		public SkeletonDataAsset SkeletonDataAsset => skeletonDataAsset;

		public Texture OverrideTexture
		{
			get
			{
				return overrideTexture;
			}
			set
			{
				overrideTexture = value;
				base.canvasRenderer.SetTexture(mainTexture);
			}
		}

		public override Texture mainTexture
		{
			get
			{
				if (overrideTexture != null)
				{
					return overrideTexture;
				}
				return (!(skeletonDataAsset == null)) ? skeletonDataAsset.atlasAssets[0].materials[0].mainTexture : null;
			}
		}

		public Skeleton Skeleton => skeleton;

		public SkeletonData SkeletonData => (skeleton != null) ? skeleton.data : null;

		public bool IsValid => skeleton != null;

		public AnimationState AnimationState => state;

		public MeshGenerator MeshGenerator => meshGenerator;

		public event UpdateBonesDelegate UpdateLocal;

		public event UpdateBonesDelegate UpdateWorld;

		public event UpdateBonesDelegate UpdateComplete;

		public event MeshGeneratorDelegate OnPostProcessVertices;

		public static SkeletonGraphic NewSkeletonGraphicGameObject(SkeletonDataAsset skeletonDataAsset, Transform parent)
		{
			SkeletonGraphic skeletonGraphic = AddSkeletonGraphicComponent(new GameObject("New Spine GameObject"), skeletonDataAsset);
			if (parent != null)
			{
				skeletonGraphic.transform.SetParent(parent, worldPositionStays: false);
			}
			return skeletonGraphic;
		}

		public static SkeletonGraphic AddSkeletonGraphicComponent(GameObject gameObject, SkeletonDataAsset skeletonDataAsset)
		{
			SkeletonGraphic skeletonGraphic = gameObject.AddComponent<SkeletonGraphic>();
			if (skeletonDataAsset != null)
			{
				skeletonGraphic.skeletonDataAsset = skeletonDataAsset;
				skeletonGraphic.Initialize(overwrite: false);
			}
			return skeletonGraphic;
		}

		protected override void Awake()
		{
			base.Awake();
			if (!IsValid)
			{
				Initialize(overwrite: false);
				Rebuild(CanvasUpdate.PreRender);
			}
		}

		public override void Rebuild(CanvasUpdate update)
		{
			base.Rebuild(update);
			if (!base.canvasRenderer.cull && update == CanvasUpdate.PreRender)
			{
				UpdateMesh();
			}
		}

		public virtual void Update()
		{
			if (!freeze)
			{
				Update((!unscaledTime) ? Time.deltaTime : Time.unscaledDeltaTime);
			}
		}

		public virtual void Update(float deltaTime)
		{
			if (IsValid)
			{
				deltaTime *= timeScale;
				skeleton.Update(deltaTime);
				state.Update(deltaTime);
				state.Apply(skeleton);
				if (this.UpdateLocal != null)
				{
					this.UpdateLocal(this);
				}
				skeleton.UpdateWorldTransform();
				if (this.UpdateWorld != null)
				{
					this.UpdateWorld(this);
					skeleton.UpdateWorldTransform();
				}
				if (this.UpdateComplete != null)
				{
					this.UpdateComplete(this);
				}
			}
		}

		public void LateUpdate()
		{
			if (!freeze)
			{
				UpdateMesh();
			}
		}

		public Mesh GetLastMesh()
		{
			return meshBuffers.GetCurrent().mesh;
		}

		public void Clear()
		{
			skeleton = null;
			base.canvasRenderer.Clear();
		}

		public void Initialize(bool overwrite)
		{
			if ((IsValid && !overwrite) || skeletonDataAsset == null)
			{
				return;
			}
			SkeletonData skeletonData = skeletonDataAsset.GetSkeletonData(quiet: false);
			if (skeletonData == null || skeletonDataAsset.atlasAssets.Length <= 0 || skeletonDataAsset.atlasAssets[0].materials.Length <= 0)
			{
				return;
			}
			state = new AnimationState(skeletonDataAsset.GetAnimationStateData());
			if (state == null)
			{
				Clear();
				return;
			}
			skeleton = new Skeleton(skeletonData)
			{
				flipX = initialFlipX,
				flipY = initialFlipY
			};
			meshBuffers = new DoubleBuffered<MeshRendererBuffers.SmartMesh>();
			base.canvasRenderer.SetTexture(mainTexture);
			if (!string.IsNullOrEmpty(initialSkinName))
			{
				skeleton.SetSkin(initialSkinName);
			}
			if (!string.IsNullOrEmpty(startingAnimation))
			{
				state.SetAnimation(0, startingAnimation, startingLoop);
				Update(0f);
			}
		}

		public void UpdateMesh()
		{
			if (IsValid)
			{
				skeleton.SetColor(color);
				MeshRendererBuffers.SmartMesh next = meshBuffers.GetNext();
				SkeletonRendererInstruction skeletonRendererInstruction = currentInstructions;
				MeshGenerator.GenerateSingleSubmeshInstruction(skeletonRendererInstruction, skeleton, material);
				bool flag = SkeletonRendererInstruction.GeometryNotEqual(skeletonRendererInstruction, next.instructionUsed);
				meshGenerator.Begin();
				if (skeletonRendererInstruction.hasActiveClipping)
				{
					meshGenerator.AddSubmesh(skeletonRendererInstruction.submeshInstructions.Items[0], flag);
				}
				else
				{
					meshGenerator.BuildMeshWithArrays(skeletonRendererInstruction, flag);
				}
				if (base.canvas != null)
				{
					meshGenerator.ScaleVertexData(base.canvas.referencePixelsPerUnit);
				}
				if (this.OnPostProcessVertices != null)
				{
					this.OnPostProcessVertices(meshGenerator.Buffers);
				}
				Mesh mesh = next.mesh;
				meshGenerator.FillVertexData(mesh);
				if (flag)
				{
					meshGenerator.FillTrianglesSingle(mesh);
				}
				base.canvasRenderer.SetMesh(mesh);
				next.instructionUsed.Set(skeletonRendererInstruction);
			}
		}
	}
}
