using System;
using UnityEngine;

namespace Spine.Unity.Modules.AttachmentTools
{
	public static class AttachmentCloneExtensions
	{
		public static Attachment GetClone(this Attachment o, bool cloneMeshesAsLinked)
		{
			RegionAttachment regionAttachment = o as RegionAttachment;
			if (regionAttachment != null)
			{
				return regionAttachment.GetClone();
			}
			MeshAttachment meshAttachment = o as MeshAttachment;
			if (meshAttachment != null)
			{
				return (!cloneMeshesAsLinked) ? meshAttachment.GetClone() : meshAttachment.GetLinkedClone();
			}
			BoundingBoxAttachment boundingBoxAttachment = o as BoundingBoxAttachment;
			if (boundingBoxAttachment != null)
			{
				return boundingBoxAttachment.GetClone();
			}
			PathAttachment pathAttachment = o as PathAttachment;
			if (pathAttachment != null)
			{
				return pathAttachment.GetClone();
			}
			PointAttachment pointAttachment = o as PointAttachment;
			if (pointAttachment != null)
			{
				return pointAttachment.GetClone();
			}
			return (o as ClippingAttachment)?.GetClone();
		}

		public static RegionAttachment GetClone(this RegionAttachment o)
		{
			RegionAttachment regionAttachment = new RegionAttachment(o.Name + "clone");
			regionAttachment.x = o.x;
			regionAttachment.y = o.y;
			regionAttachment.rotation = o.rotation;
			regionAttachment.scaleX = o.scaleX;
			regionAttachment.scaleY = o.scaleY;
			regionAttachment.width = o.width;
			regionAttachment.height = o.height;
			regionAttachment.r = o.r;
			regionAttachment.g = o.g;
			regionAttachment.b = o.b;
			regionAttachment.a = o.a;
			regionAttachment.Path = o.Path;
			regionAttachment.RendererObject = o.RendererObject;
			regionAttachment.regionOffsetX = o.regionOffsetX;
			regionAttachment.regionOffsetY = o.regionOffsetY;
			regionAttachment.regionWidth = o.regionWidth;
			regionAttachment.regionHeight = o.regionHeight;
			regionAttachment.regionOriginalWidth = o.regionOriginalWidth;
			regionAttachment.regionOriginalHeight = o.regionOriginalHeight;
			regionAttachment.uvs = (o.uvs.Clone() as float[]);
			regionAttachment.offset = (o.offset.Clone() as float[]);
			return regionAttachment;
		}

		public static ClippingAttachment GetClone(this ClippingAttachment o)
		{
			ClippingAttachment clippingAttachment = new ClippingAttachment(o.Name);
			clippingAttachment.endSlot = o.endSlot;
			ClippingAttachment clippingAttachment2 = clippingAttachment;
			CloneVertexAttachment(o, clippingAttachment2);
			return clippingAttachment2;
		}

		public static PointAttachment GetClone(this PointAttachment o)
		{
			PointAttachment pointAttachment = new PointAttachment(o.Name);
			pointAttachment.rotation = o.rotation;
			pointAttachment.x = o.x;
			pointAttachment.y = o.y;
			return pointAttachment;
		}

		public static BoundingBoxAttachment GetClone(this BoundingBoxAttachment o)
		{
			BoundingBoxAttachment boundingBoxAttachment = new BoundingBoxAttachment(o.Name);
			CloneVertexAttachment(o, boundingBoxAttachment);
			return boundingBoxAttachment;
		}

		public static MeshAttachment GetLinkedClone(this MeshAttachment o, bool inheritDeform = true)
		{
			return o.GetLinkedMesh(o.Name, o.RendererObject as AtlasRegion, inheritDeform);
		}

		public static MeshAttachment GetClone(this MeshAttachment o)
		{
			MeshAttachment meshAttachment = new MeshAttachment(o.Name);
			meshAttachment.r = o.r;
			meshAttachment.g = o.g;
			meshAttachment.b = o.b;
			meshAttachment.a = o.a;
			meshAttachment.inheritDeform = o.inheritDeform;
			meshAttachment.Path = o.Path;
			meshAttachment.RendererObject = o.RendererObject;
			meshAttachment.regionOffsetX = o.regionOffsetX;
			meshAttachment.regionOffsetY = o.regionOffsetY;
			meshAttachment.regionWidth = o.regionWidth;
			meshAttachment.regionHeight = o.regionHeight;
			meshAttachment.regionOriginalWidth = o.regionOriginalWidth;
			meshAttachment.regionOriginalHeight = o.regionOriginalHeight;
			meshAttachment.RegionU = o.RegionU;
			meshAttachment.RegionV = o.RegionV;
			meshAttachment.RegionU2 = o.RegionU2;
			meshAttachment.RegionV2 = o.RegionV2;
			meshAttachment.RegionRotate = o.RegionRotate;
			meshAttachment.uvs = (o.uvs.Clone() as float[]);
			MeshAttachment meshAttachment2 = meshAttachment;
			if (o.ParentMesh != null)
			{
				meshAttachment2.ParentMesh = o.ParentMesh;
			}
			else
			{
				CloneVertexAttachment(o, meshAttachment2);
				meshAttachment2.regionUVs = (o.regionUVs.Clone() as float[]);
				meshAttachment2.triangles = (o.triangles.Clone() as int[]);
				meshAttachment2.hulllength = o.hulllength;
				meshAttachment2.Edges = ((o.Edges != null) ? (o.Edges.Clone() as int[]) : null);
				meshAttachment2.Width = o.Width;
				meshAttachment2.Height = o.Height;
			}
			return meshAttachment2;
		}

		public static PathAttachment GetClone(this PathAttachment o)
		{
			PathAttachment pathAttachment = new PathAttachment(o.Name);
			pathAttachment.lengths = (o.lengths.Clone() as float[]);
			pathAttachment.closed = o.closed;
			pathAttachment.constantSpeed = o.constantSpeed;
			PathAttachment pathAttachment2 = pathAttachment;
			CloneVertexAttachment(o, pathAttachment2);
			return pathAttachment2;
		}

		private static void CloneVertexAttachment(VertexAttachment src, VertexAttachment dest)
		{
			dest.worldVerticesLength = src.worldVerticesLength;
			if (src.bones != null)
			{
				dest.bones = (src.bones.Clone() as int[]);
			}
			if (src.vertices != null)
			{
				dest.vertices = (src.vertices.Clone() as float[]);
			}
		}

		public static MeshAttachment GetLinkedMesh(this MeshAttachment o, string newLinkedMeshName, AtlasRegion region, bool inheritDeform = true)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			if (o.ParentMesh != null)
			{
				o = o.ParentMesh;
			}
			MeshAttachment meshAttachment = new MeshAttachment(newLinkedMeshName);
			meshAttachment.SetRegion(region, updateUVs: false);
			meshAttachment.Path = newLinkedMeshName;
			meshAttachment.r = 1f;
			meshAttachment.g = 1f;
			meshAttachment.b = 1f;
			meshAttachment.a = 1f;
			meshAttachment.inheritDeform = inheritDeform;
			meshAttachment.ParentMesh = o;
			meshAttachment.UpdateUVs();
			return meshAttachment;
		}

		public static MeshAttachment GetLinkedMesh(this MeshAttachment o, Sprite sprite, Shader shader, bool inheritDeform = true, Material materialPropertySource = null)
		{
			Material material = new Material(shader);
			if (materialPropertySource != null)
			{
				material.CopyPropertiesFromMaterial(materialPropertySource);
				material.shaderKeywords = materialPropertySource.shaderKeywords;
			}
			return o.GetLinkedMesh(sprite.name, sprite.ToAtlasRegion(), inheritDeform);
		}

		public static MeshAttachment GetLinkedMesh(this MeshAttachment o, Sprite sprite, Material materialPropertySource, bool inheritDeform = true)
		{
			return o.GetLinkedMesh(sprite, materialPropertySource.shader, inheritDeform, materialPropertySource);
		}

		public static Attachment GetRemappedClone(this Attachment o, Sprite sprite, Material sourceMaterial, bool premultiplyAlpha = true, bool cloneMeshAsLinked = true, bool useOriginalRegionSize = false)
		{
			AtlasRegion atlasRegion = (!premultiplyAlpha) ? sprite.ToAtlasRegion() : sprite.ToAtlasRegionPMAClone(sourceMaterial);
			return o.GetRemappedClone(atlasRegion, cloneMeshAsLinked, useOriginalRegionSize, 1f / sprite.pixelsPerUnit);
		}

		public static Attachment GetRemappedClone(this Attachment o, AtlasRegion atlasRegion, bool cloneMeshAsLinked = true, bool useOriginalRegionSize = false, float scale = 0.01f)
		{
			RegionAttachment regionAttachment = o as RegionAttachment;
			if (regionAttachment != null)
			{
				RegionAttachment clone = regionAttachment.GetClone();
				clone.SetRegion(atlasRegion, updateOffset: false);
				if (!useOriginalRegionSize)
				{
					clone.width = (float)atlasRegion.width * scale;
					clone.height = (float)atlasRegion.height * scale;
				}
				clone.UpdateOffset();
				return clone;
			}
			MeshAttachment meshAttachment = o as MeshAttachment;
			if (meshAttachment != null)
			{
				MeshAttachment meshAttachment2 = (!cloneMeshAsLinked) ? meshAttachment.GetClone() : meshAttachment.GetLinkedClone(cloneMeshAsLinked);
				meshAttachment2.SetRegion(atlasRegion);
				return meshAttachment2;
			}
			return o.GetClone(cloneMeshesAsLinked: true);
		}
	}
}
