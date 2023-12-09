using UnityEngine;
using UnityEngine.UI;

public class FPSShow : MonoBehaviour
{
	public Text currentFPS;

	private const float fpsMeasurePeriod = 0.1f;

	private int m_FpsAccumulator;

	private float m_FpsNextPeriod;

	private int m_CurrentFps;

	private float lastUpdateTime;

	private void Awake()
	{
		lastUpdateTime = Time.realtimeSinceStartup;
		m_FpsNextPeriod = lastUpdateTime + 0.1f;
	}

	private void Update()
	{
		m_FpsAccumulator++;
		if (Time.realtimeSinceStartup > m_FpsNextPeriod)
		{
			m_CurrentFps = (int)((float)m_FpsAccumulator / (Time.realtimeSinceStartup - lastUpdateTime));
			m_FpsAccumulator = 0;
			currentFPS.text = m_CurrentFps.ToFixedString();
			lastUpdateTime = Time.realtimeSinceStartup;
			m_FpsNextPeriod = lastUpdateTime + 0.1f;
		}
	}
}
