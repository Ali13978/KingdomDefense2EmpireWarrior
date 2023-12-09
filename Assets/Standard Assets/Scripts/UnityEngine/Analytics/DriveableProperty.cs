using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine.Analytics
{
	[Serializable]
	public class DriveableProperty
	{
		[Serializable]
		public class FieldWithRemoteSettingsKey
		{
			[SerializeField]
			private Object m_Target;

			[SerializeField]
			private string m_FieldPath;

			[SerializeField]
			private string m_RSKeyName;

			[SerializeField]
			private string m_Type;

			public Object target
			{
				get
				{
					return m_Target;
				}
				set
				{
					m_Target = target;
				}
			}

			public string fieldPath
			{
				get
				{
					return m_FieldPath;
				}
				set
				{
					m_FieldPath = value;
				}
			}

			public string rsKeyName
			{
				get
				{
					return m_RSKeyName;
				}
				set
				{
					m_RSKeyName = rsKeyName;
				}
			}

			public string type
			{
				get
				{
					return m_Type;
				}
				set
				{
					m_Type = type;
				}
			}

			public void SetValue(object val)
			{
				object target = m_Target;
				string[] array = m_FieldPath.Split('.');
				foreach (string name in array)
				{
					try
					{
						PropertyInfo property = target.GetType().GetProperty(name);
						property.SetValue(target, val, null);
					}
					catch
					{
						FieldInfo field = target.GetType().GetField(name);
						field.SetValue(target, val);
					}
				}
			}

			public Type GetTypeOfField()
			{
				object obj = m_Target;
				string[] array = m_FieldPath.Split('.');
				foreach (string name in array)
				{
					try
					{
						PropertyInfo property = obj.GetType().GetProperty(name);
						obj = property.GetValue(obj, null);
					}
					catch
					{
						FieldInfo field = obj.GetType().GetField(name);
						obj = field.GetValue(obj);
					}
				}
				return obj.GetType();
			}
		}

		[SerializeField]
		private List<FieldWithRemoteSettingsKey> m_Fields;

		public List<FieldWithRemoteSettingsKey> fields
		{
			get
			{
				return m_Fields;
			}
			set
			{
				m_Fields = fields;
			}
		}
	}
}
