namespace SSR.Core.Architecture
{
	public interface IMessage
	{
		object Sender
		{
			get;
		}

		string Subject
		{
			get;
		}

		object Content
		{
			get;
		}
	}
}
