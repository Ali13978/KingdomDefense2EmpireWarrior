namespace SSR.Core.Architecture.Pool
{
	public interface IPool<T> : IPoolBase<T>
	{
		void PushInstance<U>(U memberInstance) where U : IPoolMember<T>, T;

		void PushPrototype<U>(U memberPrototype) where U : IPoolMember<T>, T;

		T TakeInstance(bool forceCloning);
	}
}
