public class StoreBundleData
{
	public static SaleBundleConfigData GetDataSaleBundle(string productID)
	{
		SaleBundleConfigData result = null;
		SaleBundleConfigData[] dataArray = CommonData.Instance.saleBundleConfig.dataArray;
		for (int i = 0; i < dataArray.Length; i++)
		{
			if (dataArray[i].Productid.Equals(productID))
			{
				result = dataArray[i];
			}
		}
		return result;
	}
}
