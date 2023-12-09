namespace Services.PlatformSpecific
{
	public interface IAd
	{
		bool IsOfferVideoAvailable
		{
			get;
		}

		void RequestAds();

		void ShowInterstitial();

		void ShowOfferVideo(OfferVideoCallback offerVideoCallback);
	}
}
