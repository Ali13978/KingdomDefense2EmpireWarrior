using System;
using System.Collections.Generic;

namespace SSR.Core.Architecture.Pool
{
	public class Pool<T> : IPool<T>, IPoolBase<T>
	{
		private List<T> instances = new List<T>();

		private T prototype;

		public void PushInstance<U>(U memberInstance) where U : IPoolMember<T>, T
		{
			instances.Add((T)(object)memberInstance);
			memberInstance.Pool = this;
			memberInstance.InPool = false;
		}

		public void PushPrototype<U>(U memberPrototype) where U : IPoolMember<T>, T
		{
			prototype = (T)(object)memberPrototype;
		}

		public T TakeInstance(bool forceCloning)
		{
			if (instances.Count > 0)
			{
				return TakeAvailableInstance();
			}
			if (forceCloning)
			{
				return TakeIntanceByClonning();
			}
			throw new Exception("Pool memebers are not available to take");
		}

		private T TakeAvailableInstance()
		{
			int index = instances.Count - 1;
			T val = instances[index];
			(val as IPoolMember<T>).InPool = false;
			instances.RemoveAt(index);
			return val;
		}

		private T TakeIntanceByClonning()
		{
			T val = (prototype as ICloneable<T>).Clone();
			(val as IPoolMember<T>).Pool = this;
			(val as IPoolMember<T>).InPool = false;
			return val;
		}
	}
}
