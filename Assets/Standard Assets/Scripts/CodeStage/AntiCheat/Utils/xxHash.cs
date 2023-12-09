namespace CodeStage.AntiCheat.Utils
{
	internal class xxHash
	{
		private const uint PRIME32_1 = 2654435761u;

		private const uint PRIME32_2 = 2246822519u;

		private const uint PRIME32_3 = 3266489917u;

		private const uint PRIME32_4 = 668265263u;

		private const uint PRIME32_5 = 374761393u;

		public static uint CalculateHash(byte[] buf, int len, uint seed)
		{
			int i = 0;
			uint num23;
			if (len >= 16)
			{
				int num = len - 16;
				uint num2 = (uint)((int)seed + -1640531535 + -2048144777);
				uint num3 = (uint)((int)seed + -2048144777);
				uint num4 = seed;
				uint num5 = (uint)((int)seed - -1640531535);
				do
				{
					uint num10 = (uint)(buf[i++] | (buf[i++] << 8) | (buf[i++] << 16) | (buf[i++] << 24));
					num2 = (uint)((int)num2 + (int)num10 * -2048144777);
					num2 = ((num2 << 13) | (num2 >> 19));
					num2 = (uint)((int)num2 * -1640531535);
					num10 = (uint)(buf[i++] | (buf[i++] << 8) | (buf[i++] << 16) | (buf[i++] << 24));
					num3 = (uint)((int)num3 + (int)num10 * -2048144777);
					num3 = ((num3 << 13) | (num3 >> 19));
					num3 = (uint)((int)num3 * -1640531535);
					num10 = (uint)(buf[i++] | (buf[i++] << 8) | (buf[i++] << 16) | (buf[i++] << 24));
					num4 = (uint)((int)num4 + (int)num10 * -2048144777);
					num4 = ((num4 << 13) | (num4 >> 19));
					num4 = (uint)((int)num4 * -1640531535);
					num10 = (uint)(buf[i++] | (buf[i++] << 8) | (buf[i++] << 16) | (buf[i++] << 24));
					num5 = (uint)((int)num5 + (int)num10 * -2048144777);
					num5 = ((num5 << 13) | (num5 >> 19));
					num5 = (uint)((int)num5 * -1640531535);
				}
				while (i <= num);
				num23 = ((num2 << 1) | (num2 >> 31)) + ((num3 << 7) | (num3 >> 25)) + ((num4 << 12) | (num4 >> 20)) + ((num5 << 18) | (num5 >> 14));
			}
			else
			{
				num23 = seed + 374761393;
			}
			num23 = (uint)((int)num23 + len);
			while (i <= len - 4)
			{
				num23 = (uint)((int)num23 + (buf[i++] | (buf[i++] << 8) | (buf[i++] << 16) | (buf[i++] << 24)) * -1028477379);
				num23 = ((num23 << 17) | (num23 >> 15)) * 668265263;
			}
			for (; i < len; i++)
			{
				num23 = (uint)((int)num23 + buf[i] * 374761393);
				num23 = (uint)((int)((num23 << 11) | (num23 >> 21)) * -1640531535);
			}
			num23 ^= num23 >> 15;
			num23 = (uint)((int)num23 * -2048144777);
			num23 ^= num23 >> 13;
			num23 = (uint)((int)num23 * -1028477379);
			return num23 ^ (num23 >> 16);
		}
	}
}
