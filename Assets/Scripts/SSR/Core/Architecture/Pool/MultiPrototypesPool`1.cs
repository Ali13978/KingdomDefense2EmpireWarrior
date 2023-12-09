using System;
using System.Collections.Generic;

namespace SSR.Core.Architecture.Pool
{
	public abstract class MultiPrototypesPool<T> : IMultiPrototypesPool<T>, IPoolBase<T>
	{
		private Dictionary<int, T> prototypesDictionary = new Dictionary<int, T>();

		private Dictionary<int, List<T>> instancesDictionary = new Dictionary<int, List<T>>();

		public bool ContainsPrototype(int prototypeId)
		{
			return prototypesDictionary.ContainsKey(prototypeId);
		}

		public void PushInstance<U>(U memberInstance) where U : IMultiPrototypesPoolMember<T>, T
		{
			int prototypeId = memberInstance.PrototypeId;
			memberInstance.Pool = this;
			memberInstance.InPool = true;
			instancesDictionary[prototypeId].Add((T)(object)memberInstance);
		}

		public void PushPrototype<U>(U memberPrototype) where U : IMultiPrototypesPoolMember<T>, T
		{
			int prototypeId = memberPrototype.PrototypeId;
			prototypesDictionary.Add(prototypeId, (T)(object)memberPrototype);
			instancesDictionary.Add(prototypeId, new List<T>());
		}

		public T TakeInstance(int prototypeId, bool forceCloning)
		{
			if (instancesDictionary[prototypeId].Count > 0)
			{
				return TakeInstanceAvailableInDictionary(prototypeId);
			}
			if (forceCloning)
			{
				return TakeInstanceByClonning(prototypeId);
			}
			throw new Exception("Pool memebers are not available to take");
		}

		public T TakeInstanceByPrototype<U>(U prototype) where U : IMultiPrototypesPoolMember<T>, T
		{
			int prototypeId = prototype.PrototypeId;
			if (!ContainsPrototype(prototypeId))
			{
				PushPrototype(prototype);
			}
			return TakeInstance(prototypeId, forceCloning: true);
		}

		private T TakeInstanceAvailableInDictionary(int prototypeId)
		{
			List<T> list = instancesDictionary[prototypeId];
			int index = list.Count - 1;
			T val = list[index];
			(val as IMultiPrototypesPoolMember<T>).InPool = false;
			list.RemoveAt(index);
			return val;
		}

		private T TakeInstanceByClonning(int prototypeId)
		{
			T val = (prototypesDictionary[prototypeId] as ICloneable<T>).Clone();
			(val as IMultiPrototypesPoolMember<T>).Pool = this;
			(val as IMultiPrototypesPoolMember<T>).InPool = false;
			return val;
		}
	}
}
