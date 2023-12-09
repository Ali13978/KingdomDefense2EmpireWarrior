using System;

namespace SSR.Core.Architecture
{
	public interface ICollabHubRegister
	{
		bool IsOpening
		{
			get;
		}

		event Action Finish;

		void Register(ICollabMember member);
	}
}
