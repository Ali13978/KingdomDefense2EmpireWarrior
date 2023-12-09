using Data;

namespace Tutorial
{
	public class SkipTutorialButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			ReadWriteDataTutorial.Instance.SkipAllTutorials();
			ReadWriteDataTutorial.Instance.SetCurrentTutorialPass();
		}
	}
}
