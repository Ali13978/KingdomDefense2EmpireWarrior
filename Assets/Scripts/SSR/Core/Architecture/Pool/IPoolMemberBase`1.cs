namespace SSR.Core.Architecture.Pool
{
	public interface IPoolMemberBase<T> : ICloneable<T>
	{
		bool InPool
		{
			get;
			set;
		}
	}
}
