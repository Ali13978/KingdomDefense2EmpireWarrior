public interface INewFSMController
{
	INewEntityState GetCurrentState();

	void SetCurrentState(INewEntityState state);
}
