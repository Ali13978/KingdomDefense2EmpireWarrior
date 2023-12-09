using MyCustom;
using UnityEngine;

namespace OfferPopup
{
	public class OfferPopupController : CustomMonoBehaviour
	{
		[Header("Controllers")]
		[SerializeField]
		private SingleHeroOfferController singleHeroOfferController;

		[Space]
		[SerializeField]
		private ReadDataOfferBundle readDataOfferBundle;

		public SingleHeroOfferController SingleHeroOfferController
		{
			get
			{
				return singleHeroOfferController;
			}
			set
			{
				singleHeroOfferController = value;
			}
		}

		public ReadDataOfferBundle ReadDataOfferBundle
		{
			get
			{
				return readDataOfferBundle;
			}
			set
			{
				readDataOfferBundle = value;
			}
		}

		public void InitSingleHeroOffer(int heroID, OfferType type)
		{
			SingleHeroOfferController.Init(heroID, type);
		}
	}
}
