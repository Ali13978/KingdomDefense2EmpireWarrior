using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.Native.Cwrapper;
using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeEvent : BaseReferenceHolder, IEvent
	{
		public string Id => PInvokeUtilities.OutParamsToString((byte[] out_string, UIntPtr out_size) => Event.Event_Id(SelfPtr(), out_string, out_size));

		public string Name => PInvokeUtilities.OutParamsToString((byte[] out_string, UIntPtr out_size) => Event.Event_Name(SelfPtr(), out_string, out_size));

		public string Description => PInvokeUtilities.OutParamsToString((byte[] out_string, UIntPtr out_size) => Event.Event_Description(SelfPtr(), out_string, out_size));

		public string ImageUrl => PInvokeUtilities.OutParamsToString((byte[] out_string, UIntPtr out_size) => Event.Event_ImageUrl(SelfPtr(), out_string, out_size));

		public ulong CurrentCount => Event.Event_Count(SelfPtr());

		public EventVisibility Visibility
		{
			get
			{
				Types.EventVisibility eventVisibility = Event.Event_Visibility(SelfPtr());
				switch (eventVisibility)
				{
				case Types.EventVisibility.HIDDEN:
					return EventVisibility.Hidden;
				case Types.EventVisibility.REVEALED:
					return EventVisibility.Revealed;
				default:
					throw new InvalidOperationException("Unknown visibility: " + eventVisibility);
				}
			}
		}

		internal NativeEvent(IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			Event.Event_Dispose(selfPointer);
		}

		public override string ToString()
		{
			if (IsDisposed())
			{
				return "[NativeEvent: DELETED]";
			}
			return $"[NativeEvent: Id={Id}, Name={Name}, Description={Description}, ImageUrl={ImageUrl}, CurrentCount={CurrentCount}, Visibility={Visibility}]";
		}
	}
}
