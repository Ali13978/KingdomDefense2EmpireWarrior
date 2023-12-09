using UnityEngine;

namespace SSR.Core.Architecture.Pool
{
	public class MultiPrototypesPoolMemeberMonoBehavior<T> : MonoBehaviour, IMultiPrototypesPoolMember<T>, IPoolMemberBase<T>, ICloneable<T> where T : class
	{
		private bool inPool;

		private IMultiPrototypesPool<T> pool;

		private int prototypeId;

		private bool prototypeIdOverridden;

		public bool InPool
		{
			get
			{
				return inPool;
			}
			set
			{
				inPool = value;
			}
		}

		public IMultiPrototypesPool<T> Pool
		{
			get
			{
				return pool;
			}
			set
			{
				pool = value;
			}
		}

		public int PrototypeId
		{
			get
			{
				if (prototypeIdOverridden)
				{
					return prototypeId;
				}
				return GetInstanceID();
			}
			set
			{
				prototypeIdOverridden = true;
				prototypeId = value;
			}
		}

		public T Clone()
		{
			MultiPrototypesPoolMemeberMonoBehavior<T> multiPrototypesPoolMemeberMonoBehavior = UnityEngine.Object.Instantiate(this);
			multiPrototypesPoolMemeberMonoBehavior.PrototypeId = PrototypeId;
			multiPrototypesPoolMemeberMonoBehavior.OnClone();
			return multiPrototypesPoolMemeberMonoBehavior as T;
		}

		protected virtual void OnClone()
		{
		}
	}
}
