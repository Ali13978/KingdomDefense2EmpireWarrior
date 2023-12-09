namespace Services.PlatformSpecific
{
	public interface INotification
	{
		void PushNotify(string content, int delayTimeBySecond);

		void CancelAllNotify();
	}
}
