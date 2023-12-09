using System.Runtime.InteropServices;

namespace Gameplay
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct EnumeratorHandle<T>
	{
		public int Count
		{
			get;
			set;
		}

		public int Index
		{
			get;
			set;
		}

		public T Value
		{
			get;
			set;
		}

		public EnumeratorHandle(int count, int index, T value)
		{
			Count = count;
			Index = index;
			Value = value;
		}
	}
}
