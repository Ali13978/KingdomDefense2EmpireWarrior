using System;
using System.Collections.Generic;
using UnityEngine;

namespace SSR.Core.Architecture
{
	public class CollabHub : ClassEncapsulator<ICollabHub>, ICollabHub, ICollabHubRegister
	{
		[Header("Status")]
		[SerializeField]
		[ReadOnly]
		private bool isOpening;

		[SerializeField]
		[ReadOnly]
		private bool isFinished;

		[Header("Event")]
		[SerializeField]
		private OrderedEventDispatcher onFinished = new OrderedEventDispatcher();

		private List<ICollabMember> collabMembers;

		private bool working;

		bool ICollabHub.IsFinished => isFinished;

		public bool IsOpening => isOpening;

		event Action ICollabHubRegister.Finish
		{
			add
			{
				finish += value;
			}
			remove
			{
				finish -= value;
			}
		}

		private event Action finish;

		void ICollabHubRegister.Register(ICollabMember member)
		{
			collabMembers.Add(member);
		}

		protected override ICollabHub GetEncapsulatedClass()
		{
			return this;
		}

		void ICollabHub.CloseRegistration()
		{
			isOpening = false;
		}

		void ICollabHub.OpenRegistration()
		{
			isOpening = true;
			isFinished = false;
			collabMembers = new List<ICollabMember>();
		}

		public void StartWorking()
		{
			working = true;
			base.enabled = true;
			DispatchStartWorkingEvent();
		}

		private void DispatchStartWorkingEvent()
		{
			for (int i = 0; i < collabMembers.Count; i++)
			{
				collabMembers[i].OnStartWorking();
			}
		}

		private void FinishWorking()
		{
			working = false;
			isFinished = true;
			base.enabled = false;
			if (this.finish != null)
			{
				this.finish();
			}
			onFinished.Dispatch();
		}

		public void OnEnable()
		{
			if (!working)
			{
				base.enabled = false;
			}
		}

		private void Update()
		{
			if (IsAllDone())
			{
				FinishWorking();
			}
		}

		private bool IsAllDone()
		{
			for (int i = 0; i < collabMembers.Count; i++)
			{
				if (!collabMembers[i].IsFinished)
				{
					return false;
				}
			}
			return true;
		}
	}
}
