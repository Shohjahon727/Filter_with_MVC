namespace WebApplication4.Models.RequestModels
{
	public class CarListRequestModel
	{
		public string[]? FilterByColor { get; set; }

		public string[]? FilterByManufacturer { get; set; }

		public decimal? MinPrice { get; set; }

		public decimal? MaxPrice { get; set; }

		public int Page { get; set; } = 1;

		public int PageSize { get; set; } = 10;

        public CarListRequestModel()
        {
            
        }
        public CarListRequestModel(string[]? filterByColor = null, string[]? filterByManufacturer = null,
								   decimal? minPrice = null, decimal? maxPrice = null,
								   int page = 1, int pageSize = 10)
		{
			FilterByColor = filterByColor;
			FilterByManufacturer = filterByManufacturer;
			MinPrice = minPrice;
			MaxPrice = maxPrice;
			Page = page;
			PageSize = pageSize;
		}
	}
}
