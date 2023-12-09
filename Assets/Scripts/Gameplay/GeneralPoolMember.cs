using SSR.Core.Architecture.Pool;

namespace Gameplay
{
	public class GeneralPoolMember : MultiPrototypesPoolMemeberMonoBehavior<GeneralPoolMember>
	{
		public void TryReturnToPool()
		{
			if (base.Pool != null)
			{
				base.Pool.PushInstance(this);
			}
		}
	}
}
