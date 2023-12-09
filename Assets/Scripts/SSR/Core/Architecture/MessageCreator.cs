namespace SSR.Core.Architecture
{
	public class MessageCreator : IMessageCreator, IMessage
	{
		private object content;

		private object sender;

		private string subject;

		public object Content
		{
			get
			{
				return content;
			}
			set
			{
				content = value;
			}
		}

		public object Sender
		{
			get
			{
				return sender;
			}
			set
			{
				sender = value;
			}
		}

		public string Subject
		{
			get
			{
				return subject;
			}
			set
			{
				subject = value;
			}
		}
	}
}
