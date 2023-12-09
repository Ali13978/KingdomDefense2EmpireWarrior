using System;

namespace Spine
{
	public class PathConstraint : IConstraint, IUpdatable
	{
		private const int NONE = -1;

		private const int BEFORE = -2;

		private const int AFTER = -3;

		private const float Epsilon = 1E-05f;

		internal PathConstraintData data;

		internal ExposedList<Bone> bones;

		internal Slot target;

		internal float position;

		internal float spacing;

		internal float rotateMix;

		internal float translateMix;

		internal ExposedList<float> spaces = new ExposedList<float>();

		internal ExposedList<float> positions = new ExposedList<float>();

		internal ExposedList<float> world = new ExposedList<float>();

		internal ExposedList<float> curves = new ExposedList<float>();

		internal ExposedList<float> lengths = new ExposedList<float>();

		internal float[] segments = new float[10];

		public int Order => data.order;

		public float Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
			}
		}

		public float Spacing
		{
			get
			{
				return spacing;
			}
			set
			{
				spacing = value;
			}
		}

		public float RotateMix
		{
			get
			{
				return rotateMix;
			}
			set
			{
				rotateMix = value;
			}
		}

		public float TranslateMix
		{
			get
			{
				return translateMix;
			}
			set
			{
				translateMix = value;
			}
		}

		public ExposedList<Bone> Bones => bones;

		public Slot Target
		{
			get
			{
				return target;
			}
			set
			{
				target = value;
			}
		}

		public PathConstraintData Data => data;

		public PathConstraint(PathConstraintData data, Skeleton skeleton)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data", "data cannot be null.");
			}
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton", "skeleton cannot be null.");
			}
			this.data = data;
			bones = new ExposedList<Bone>(data.Bones.Count);
			foreach (BoneData bone in data.bones)
			{
				bones.Add(skeleton.FindBone(bone.name));
			}
			target = skeleton.FindSlot(data.target.name);
			position = data.position;
			spacing = data.spacing;
			rotateMix = data.rotateMix;
			translateMix = data.translateMix;
		}

		public void Apply()
		{
			Update();
		}

		public void Update()
		{
			PathAttachment pathAttachment = target.Attachment as PathAttachment;
			if (pathAttachment == null)
			{
				return;
			}
			float num = rotateMix;
			float num2 = translateMix;
			bool flag = num2 > 0f;
			bool flag2 = num > 0f;
			if (!flag && !flag2)
			{
				return;
			}
			PathConstraintData pathConstraintData = data;
			SpacingMode spacingMode = pathConstraintData.spacingMode;
			bool flag3 = spacingMode == SpacingMode.Length;
			RotateMode rotateMode = pathConstraintData.rotateMode;
			bool flag4 = rotateMode == RotateMode.Tangent;
			bool flag5 = rotateMode == RotateMode.ChainScale;
			int count = bones.Count;
			int num3 = (!flag4) ? (count + 1) : count;
			Bone[] items = bones.Items;
			ExposedList<float> exposedList = spaces.Resize(num3);
			ExposedList<float> exposedList2 = null;
			float num4 = spacing;
			if (flag5 || flag3)
			{
				if (flag5)
				{
					exposedList2 = lengths.Resize(count);
				}
				int num5 = 0;
				int num6 = num3 - 1;
				while (num5 < num6)
				{
					Bone bone = items[num5];
					float length = bone.data.length;
					if (length < 1E-05f)
					{
						if (flag5)
						{
							exposedList2.Items[num5] = 0f;
						}
						exposedList.Items[++num5] = 0f;
						continue;
					}
					float num7 = length * bone.a;
					float num8 = length * bone.c;
					float num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
					if (flag5)
					{
						exposedList2.Items[num5] = num9;
					}
					exposedList.Items[++num5] = ((!flag3) ? num4 : (length + num4)) * num9 / length;
				}
			}
			else
			{
				for (int i = 1; i < num3; i++)
				{
					exposedList.Items[i] = num4;
				}
			}
			float[] array = ComputeWorldPositions(pathAttachment, num3, flag4, pathConstraintData.positionMode == PositionMode.Percent, spacingMode == SpacingMode.Percent);
			float num10 = array[0];
			float num11 = array[1];
			float num12 = pathConstraintData.offsetRotation;
			bool flag6;
			if (num12 == 0f)
			{
				flag6 = (rotateMode == RotateMode.Chain);
			}
			else
			{
				flag6 = false;
				Bone bone2 = target.bone;
				num12 *= ((!(bone2.a * bone2.d - bone2.b * bone2.c > 0f)) ? (-(float)Math.PI / 180f) : ((float)Math.PI / 180f));
			}
			int num13 = 0;
			int num14 = 3;
			while (num13 < count)
			{
				Bone bone3 = items[num13];
				bone3.worldX += (num10 - bone3.worldX) * num2;
				bone3.worldY += (num11 - bone3.worldY) * num2;
				float num15 = array[num14];
				float num16 = array[num14 + 1];
				float num17 = num15 - num10;
				float num18 = num16 - num11;
				if (flag5)
				{
					float num19 = exposedList2.Items[num13];
					if (num19 >= 1E-05f)
					{
						float num20 = ((float)Math.Sqrt(num17 * num17 + num18 * num18) / num19 - 1f) * num + 1f;
						bone3.a *= num20;
						bone3.c *= num20;
					}
				}
				num10 = num15;
				num11 = num16;
				if (flag2)
				{
					float a = bone3.a;
					float b = bone3.b;
					float c = bone3.c;
					float d = bone3.d;
					float num21 = flag4 ? array[num14 - 1] : ((!(exposedList.Items[num13 + 1] < 1E-05f)) ? MathUtils.Atan2(num18, num17) : array[num14 + 2]);
					num21 -= MathUtils.Atan2(c, a);
					float num22;
					float num23;
					if (flag6)
					{
						num22 = MathUtils.Cos(num21);
						num23 = MathUtils.Sin(num21);
						float length2 = bone3.data.length;
						num10 += (length2 * (num22 * a - num23 * c) - num17) * num;
						num11 += (length2 * (num23 * a + num22 * c) - num18) * num;
					}
					else
					{
						num21 += num12;
					}
					if (num21 > (float)Math.PI)
					{
						num21 -= (float)Math.PI * 2f;
					}
					else if (num21 < -(float)Math.PI)
					{
						num21 += (float)Math.PI * 2f;
					}
					num21 *= num;
					num22 = MathUtils.Cos(num21);
					num23 = MathUtils.Sin(num21);
					bone3.a = num22 * a - num23 * c;
					bone3.b = num22 * b - num23 * d;
					bone3.c = num23 * a + num22 * c;
					bone3.d = num23 * b + num22 * d;
				}
				bone3.appliedValid = false;
				num13++;
				num14 += 3;
			}
		}

		private float[] ComputeWorldPositions(PathAttachment path, int spacesCount, bool tangents, bool percentPosition, bool percentSpacing)
		{
			Slot slot = target;
			float num = position;
			float[] items = spaces.Items;
			float[] items2 = positions.Resize(spacesCount * 3 + 2).Items;
			bool closed = path.Closed;
			int worldVerticesLength = path.WorldVerticesLength;
			int num2 = worldVerticesLength / 6;
			int num3 = -1;
			float[] items3;
			float num4;
			if (!path.ConstantSpeed)
			{
				float[] array = path.Lengths;
				num2 -= (closed ? 1 : 2);
				num4 = array[num2];
				if (percentPosition)
				{
					num *= num4;
				}
				if (percentSpacing)
				{
					for (int i = 0; i < spacesCount; i++)
					{
						items[i] *= num4;
					}
				}
				items3 = world.Resize(8).Items;
				int j = 0;
				int k = 0;
				int num5 = 0;
				for (; j < spacesCount; j++, k += 3)
				{
					float num6 = items[j];
					num += num6;
					float num7 = num;
					if (closed)
					{
						num7 %= num4;
						if (num7 < 0f)
						{
							num7 += num4;
						}
						num5 = 0;
					}
					else
					{
						if (num7 < 0f)
						{
							if (num3 != -2)
							{
								num3 = -2;
								path.ComputeWorldVertices(slot, 2, 4, items3, 0);
							}
							AddBeforePosition(num7, items3, 0, items2, k);
							continue;
						}
						if (num7 > num4)
						{
							if (num3 != -3)
							{
								num3 = -3;
								path.ComputeWorldVertices(slot, worldVerticesLength - 6, 4, items3, 0);
							}
							AddAfterPosition(num7 - num4, items3, 0, items2, k);
							continue;
						}
					}
					float num8;
					while (true)
					{
						num8 = array[num5];
						if (!(num7 > num8))
						{
							break;
						}
						num5++;
					}
					if (num5 == 0)
					{
						num7 /= num8;
					}
					else
					{
						float num9 = array[num5 - 1];
						num7 = (num7 - num9) / (num8 - num9);
					}
					if (num5 != num3)
					{
						num3 = num5;
						if (closed && num5 == num2)
						{
							path.ComputeWorldVertices(slot, worldVerticesLength - 4, 4, items3, 0);
							path.ComputeWorldVertices(slot, 0, 4, items3, 4);
						}
						else
						{
							path.ComputeWorldVertices(slot, num5 * 6 + 2, 8, items3, 0);
						}
					}
					AddCurvePosition(num7, items3[0], items3[1], items3[2], items3[3], items3[4], items3[5], items3[6], items3[7], items2, k, tangents || (j > 0 && num6 < 1E-05f));
				}
				return items2;
			}
			if (closed)
			{
				worldVerticesLength += 2;
				items3 = world.Resize(worldVerticesLength).Items;
				path.ComputeWorldVertices(slot, 2, worldVerticesLength - 4, items3, 0);
				path.ComputeWorldVertices(slot, 0, 2, items3, worldVerticesLength - 4);
				items3[worldVerticesLength - 2] = items3[0];
				items3[worldVerticesLength - 1] = items3[1];
			}
			else
			{
				num2--;
				worldVerticesLength -= 4;
				items3 = world.Resize(worldVerticesLength).Items;
				path.ComputeWorldVertices(slot, 2, worldVerticesLength, items3, 0);
			}
			float[] items4 = curves.Resize(num2).Items;
			num4 = 0f;
			float num10 = items3[0];
			float num11 = items3[1];
			float num12 = 0f;
			float num13 = 0f;
			float num14 = 0f;
			float num15 = 0f;
			float num16 = 0f;
			float num17 = 0f;
			int num18 = 0;
			int num19 = 2;
			while (num18 < num2)
			{
				num12 = items3[num19];
				num13 = items3[num19 + 1];
				num14 = items3[num19 + 2];
				num15 = items3[num19 + 3];
				num16 = items3[num19 + 4];
				num17 = items3[num19 + 5];
				float num20 = (num10 - num12 * 2f + num14) * 0.1875f;
				float num21 = (num11 - num13 * 2f + num15) * 0.1875f;
				float num22 = ((num12 - num14) * 3f - num10 + num16) * (3f / 32f);
				float num23 = ((num13 - num15) * 3f - num11 + num17) * (3f / 32f);
				float num24 = num20 * 2f + num22;
				float num25 = num21 * 2f + num23;
				float num26 = (num12 - num10) * 0.75f + num20 + num22 * (355f / (678f * (float)Math.PI));
				float num27 = (num13 - num11) * 0.75f + num21 + num23 * (355f / (678f * (float)Math.PI));
				num4 += (float)Math.Sqrt(num26 * num26 + num27 * num27);
				num26 += num24;
				num27 += num25;
				num24 += num22;
				num25 += num23;
				num4 += (float)Math.Sqrt(num26 * num26 + num27 * num27);
				num26 += num24;
				num27 += num25;
				num4 += (float)Math.Sqrt(num26 * num26 + num27 * num27);
				num26 += num24 + num22;
				num27 += num25 + num23;
				num4 = (items4[num18] = num4 + (float)Math.Sqrt(num26 * num26 + num27 * num27));
				num10 = num16;
				num11 = num17;
				num18++;
				num19 += 6;
			}
			if (percentPosition)
			{
				num *= num4;
			}
			if (percentSpacing)
			{
				for (int l = 0; l < spacesCount; l++)
				{
					items[l] *= num4;
				}
			}
			float[] array2 = segments;
			float num28 = 0f;
			int m = 0;
			int n = 0;
			int num29 = 0;
			int num30 = 0;
			for (; m < spacesCount; m++, n += 3)
			{
				float num31 = items[m];
				num += num31;
				float num32 = num;
				if (closed)
				{
					num32 %= num4;
					if (num32 < 0f)
					{
						num32 += num4;
					}
					num29 = 0;
				}
				else
				{
					if (num32 < 0f)
					{
						AddBeforePosition(num32, items3, 0, items2, n);
						continue;
					}
					if (num32 > num4)
					{
						AddAfterPosition(num32 - num4, items3, worldVerticesLength - 4, items2, n);
						continue;
					}
				}
				float num33;
				while (true)
				{
					num33 = items4[num29];
					if (!(num32 > num33))
					{
						break;
					}
					num29++;
				}
				if (num29 == 0)
				{
					num32 /= num33;
				}
				else
				{
					float num34 = items4[num29 - 1];
					num32 = (num32 - num34) / (num33 - num34);
				}
				if (num29 != num3)
				{
					num3 = num29;
					int num35 = num29 * 6;
					num10 = items3[num35];
					num11 = items3[num35 + 1];
					num12 = items3[num35 + 2];
					num13 = items3[num35 + 3];
					num14 = items3[num35 + 4];
					num15 = items3[num35 + 5];
					num16 = items3[num35 + 6];
					num17 = items3[num35 + 7];
					float num20 = (num10 - num12 * 2f + num14) * 0.03f;
					float num21 = (num11 - num13 * 2f + num15) * 0.03f;
					float num22 = ((num12 - num14) * 3f - num10 + num16) * 0.006f;
					float num23 = ((num13 - num15) * 3f - num11 + num17) * 0.006f;
					float num24 = num20 * 2f + num22;
					float num25 = num21 * 2f + num23;
					float num26 = (num12 - num10) * 0.3f + num20 + num22 * (355f / (678f * (float)Math.PI));
					float num27 = (num13 - num11) * 0.3f + num21 + num23 * (355f / (678f * (float)Math.PI));
					num28 = (array2[0] = (float)Math.Sqrt(num26 * num26 + num27 * num27));
					for (num35 = 1; num35 < 8; num35++)
					{
						num26 += num24;
						num27 += num25;
						num24 += num22;
						num25 += num23;
						num28 = (array2[num35] = num28 + (float)Math.Sqrt(num26 * num26 + num27 * num27));
					}
					num26 += num24;
					num27 += num25;
					num28 = (array2[8] = num28 + (float)Math.Sqrt(num26 * num26 + num27 * num27));
					num26 += num24 + num22;
					num27 += num25 + num23;
					num28 = (array2[9] = num28 + (float)Math.Sqrt(num26 * num26 + num27 * num27));
					num30 = 0;
				}
				num32 *= num28;
				float num36;
				while (true)
				{
					num36 = array2[num30];
					if (!(num32 > num36))
					{
						break;
					}
					num30++;
				}
				if (num30 == 0)
				{
					num32 /= num36;
				}
				else
				{
					float num37 = array2[num30 - 1];
					num32 = (float)num30 + (num32 - num37) / (num36 - num37);
				}
				AddCurvePosition(num32 * 0.1f, num10, num11, num12, num13, num14, num15, num16, num17, items2, n, tangents || (m > 0 && num31 < 1E-05f));
			}
			return items2;
		}

		private static void AddBeforePosition(float p, float[] temp, int i, float[] output, int o)
		{
			float num = temp[i];
			float num2 = temp[i + 1];
			float x = temp[i + 2] - num;
			float y = temp[i + 3] - num2;
			float num3 = MathUtils.Atan2(y, x);
			output[o] = num + p * MathUtils.Cos(num3);
			output[o + 1] = num2 + p * MathUtils.Sin(num3);
			output[o + 2] = num3;
		}

		private static void AddAfterPosition(float p, float[] temp, int i, float[] output, int o)
		{
			float num = temp[i + 2];
			float num2 = temp[i + 3];
			float x = num - temp[i];
			float y = num2 - temp[i + 1];
			float num3 = MathUtils.Atan2(y, x);
			output[o] = num + p * MathUtils.Cos(num3);
			output[o + 1] = num2 + p * MathUtils.Sin(num3);
			output[o + 2] = num3;
		}

		private static void AddCurvePosition(float p, float x1, float y1, float cx1, float cy1, float cx2, float cy2, float x2, float y2, float[] output, int o, bool tangents)
		{
			if (p < 1E-05f || float.IsNaN(p))
			{
				p = 1E-05f;
			}
			float num = p * p;
			float num2 = num * p;
			float num3 = 1f - p;
			float num4 = num3 * num3;
			float num5 = num4 * num3;
			float num6 = num3 * p;
			float num7 = num6 * 3f;
			float num8 = num3 * num7;
			float num9 = num7 * p;
			float num10 = x1 * num5 + cx1 * num8 + cx2 * num9 + x2 * num2;
			float num11 = y1 * num5 + cy1 * num8 + cy2 * num9 + y2 * num2;
			output[o] = num10;
			output[o + 1] = num11;
			if (tangents)
			{
				output[o + 2] = (float)Math.Atan2(num11 - (y1 * num4 + cy1 * num6 * 2f + cy2 * num), num10 - (x1 * num4 + cx1 * num6 * 2f + cx2 * num));
			}
		}
	}
}
