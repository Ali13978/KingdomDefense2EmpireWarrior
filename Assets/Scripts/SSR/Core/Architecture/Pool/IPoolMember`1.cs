namespace SSR.Core.Architecture.Pool
{
	public interface IPoolMember<T> : IPoolMemberBase<T>, ICloneable<T>
	{
		IPool<T> Pool
		{
			get;
			set;
		}
	}
}
