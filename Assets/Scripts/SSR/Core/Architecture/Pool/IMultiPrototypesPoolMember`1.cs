namespace SSR.Core.Architecture.Pool
{
	public interface IMultiPrototypesPoolMember<T> : IPoolMemberBase<T>, ICloneable<T>
	{
		IMultiPrototypesPool<T> Pool
		{
			get;
			set;
		}

		int PrototypeId
		{
			get;
			set;
		}
	}
}
