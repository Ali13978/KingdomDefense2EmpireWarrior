using SSR.Core.Architecture.Pool;
using UnityEngine;

namespace Gameplay
{
	public class GeneralObjectPool : MonoBehaviour
	{
		public class Pool : MultiPrototypesPool<GeneralPoolMember>
		{
		}

		private Pool pool;

		public static MultiPrototypesPool<GeneralPoolMember> Current
		{
			get;
			private set;
		}

		public void Awake()
		{
			pool = new Pool();
			Current = pool;
		}
	}
}
