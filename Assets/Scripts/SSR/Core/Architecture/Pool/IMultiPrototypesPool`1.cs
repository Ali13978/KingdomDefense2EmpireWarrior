namespace SSR.Core.Architecture.Pool
{
	public interface IMultiPrototypesPool<T> : IPoolBase<T>
	{
		void PushInstance<U>(U memberInstance) where U : IMultiPrototypesPoolMember<T>, T;

		void PushPrototype<U>(U memberPrototype) where U : IMultiPrototypesPoolMember<T>, T;

		T TakeInstance(int prototypeId, bool forceCloning);

		bool ContainsPrototype(int prototypeId);
	}
}
