using CodeStage.AntiCheat.ObscuredTypes;

public class CustomObscuredInt
{
	private ObscuredInt _value = GameTools.deltaValue;

	public int Value
	{
		get
		{
			return (int)_value - GameTools.deltaValue;
		}
		set
		{
			_value = value + GameTools.deltaValue;
		}
	}
}
