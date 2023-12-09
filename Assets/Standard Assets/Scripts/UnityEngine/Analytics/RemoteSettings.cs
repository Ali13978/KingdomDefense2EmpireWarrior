namespace UnityEngine.Analytics
{
	[AddComponentMenu("Analytics/RemoteSettings")]
	public class RemoteSettings : MonoBehaviour
	{
		[SerializeField]
		private DriveableProperty m_DriveableProperty = new DriveableProperty();

		internal DriveableProperty driveableProperty;

		internal DriveableProperty DP
		{
			get
			{
				return m_DriveableProperty;
			}
			set
			{
				m_DriveableProperty = value;
			}
		}

		private void Start()
		{
			RemoteSettingsUpdated();
			UnityEngine.RemoteSettings.Updated += RemoteSettingsUpdated;
		}

		private void RemoteSettingsUpdated()
		{
			for (int i = 0; i < m_DriveableProperty.fields.Count; i++)
			{
				DriveableProperty.FieldWithRemoteSettingsKey fieldWithRemoteSettingsKey = m_DriveableProperty.fields[i];
				if (!string.IsNullOrEmpty(fieldWithRemoteSettingsKey.rsKeyName) && UnityEngine.RemoteSettings.HasKey(fieldWithRemoteSettingsKey.rsKeyName) && fieldWithRemoteSettingsKey.target != null && !string.IsNullOrEmpty(fieldWithRemoteSettingsKey.fieldPath))
				{
					if (fieldWithRemoteSettingsKey.type == "bool")
					{
						fieldWithRemoteSettingsKey.SetValue(UnityEngine.RemoteSettings.GetBool(fieldWithRemoteSettingsKey.rsKeyName));
					}
					else if (fieldWithRemoteSettingsKey.type == "float")
					{
						fieldWithRemoteSettingsKey.SetValue(UnityEngine.RemoteSettings.GetFloat(fieldWithRemoteSettingsKey.rsKeyName));
					}
					else if (fieldWithRemoteSettingsKey.type == "int")
					{
						fieldWithRemoteSettingsKey.SetValue(UnityEngine.RemoteSettings.GetInt(fieldWithRemoteSettingsKey.rsKeyName));
					}
					else if (fieldWithRemoteSettingsKey.type == "string")
					{
						fieldWithRemoteSettingsKey.SetValue(UnityEngine.RemoteSettings.GetString(fieldWithRemoteSettingsKey.rsKeyName));
					}
				}
			}
		}
	}
}
