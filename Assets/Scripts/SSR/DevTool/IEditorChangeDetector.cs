namespace SSR.DevTool
{
	public interface IEditorChangeDetector
	{
		void AddListenerWithDulicationCheck(ChangeDetectedEventHandler listener);

		void Reset();
	}
}
