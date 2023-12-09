using UnityEngine;
using UnityEngine.UI;

namespace FreeResources
{
	public class FreeResourcesButtonController : ButtonController
	{
		[Header("Attribute")]
		public Text title;

		public Text gemAmount;

		public GameObject titleReceived;

		public Text timeCountDown;

		public Text quantity;

		public Text notification;

		[Space]
		public bool oneTimeOnlyReward;

		[Space]
		public bool visualDependOnRemoteSetting;

		[Space]
		public Image icon;

		public Sprite sprite_normal;

		public Sprite sprite_gem_chest;

		public virtual void InitData()
		{
		}

		public virtual void ResetData()
		{
		}
	}
}
