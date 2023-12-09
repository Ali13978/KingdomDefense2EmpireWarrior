using System;
using UnityEngine;

namespace Services.PlatformSpecific
{
	public class PlatformSpecificServicesProvider : MonoBehaviour
	{
		private static IPlatformSpecificServices services;

		public static IPlatformSpecificServices Services
		{
			get
			{
				return services;
			}
			set
			{
				if (services != null)
				{
					throw new InvalidOperationException("Services can not be set twice");
				}
				services = value;
			}
		}
	}
}
