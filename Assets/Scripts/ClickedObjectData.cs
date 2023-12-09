public class ClickedObjectData
{
	public ClickedObjectType clickedObjType;

	public int id;

	public ClickedObjectData(ClickedObjectType clickedObjType)
	{
		this.clickedObjType = clickedObjType;
		id = -1;
	}

	public ClickedObjectData(ClickedObjectType clickedObjType, int id)
	{
		this.clickedObjType = clickedObjType;
		this.id = id;
	}
}
