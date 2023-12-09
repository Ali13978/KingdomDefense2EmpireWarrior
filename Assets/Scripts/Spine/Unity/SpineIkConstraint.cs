namespace Spine.Unity
{
	public class SpineIkConstraint : SpineAttributeBase
	{
		public SpineIkConstraint(string startsWith = "", string dataField = "", bool includeNone = true, bool fallbackToTextField = false)
		{
			base.startsWith = startsWith;
			base.dataField = dataField;
			base.includeNone = includeNone;
			base.fallbackToTextField = fallbackToTextField;
		}
	}
}
