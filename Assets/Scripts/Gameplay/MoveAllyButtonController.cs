namespace Gameplay
{
	public class MoveAllyButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			SingletonMonoBehaviour<GameData>.Instance.RecordingPosition = true;
			InputFilterManager.Instance.SetIsClickingUI();
			SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.Close();
		}
	}
}
