namespace SSR.Core.Architecture
{
	public interface IMessageCreator : IMessage
	{
		new object Sender
		{
			get;
			set;
		}

		new string Subject
		{
			get;
			set;
		}

		new object Content
		{
			get;
			set;
		}
	}
}
