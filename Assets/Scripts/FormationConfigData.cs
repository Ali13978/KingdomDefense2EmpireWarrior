using System.Collections.Generic;

public class FormationConfigData
{
	public List<float> times = new List<float>();

	public FormationConfigData()
	{
	}

	public FormationConfigData(FormationConfigData copyData)
	{
		for (int i = 0; i < copyData.times.Count; i++)
		{
			times.Add(copyData.times[i]);
		}
	}

	public FormationConfigData(List<float> times)
	{
		this.times = times;
	}

	public void AddTime(float timeInSecond)
	{
		times.Add(timeInSecond);
	}

	public float GetDuration()
	{
		return times[times.Count - 1];
	}
}
