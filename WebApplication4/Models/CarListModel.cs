using WebApplication4.Data;
using WebApplication4.Entities;
using WebApplication4.Enums;
using WebApplication4.Models.RequestModels;
using WebApplication4.Models.ViewModels;

namespace WebApplication4.Models
{
	public class CarListModel
	{
		private readonly AppDbContext _context;

		protected CarListRequestModel RequestModel { get; set; }
		public CarListViewModel CarListViewModel { get; set; }

		public CarListModel(AppDbContext context , CarListRequestModel requestModel)
		{
			_context = context;
			RequestModel = requestModel;
			CarListViewModel = new CarListViewModel();
			FilterCars();
		}

		private void FilterCars()
		{
			var query = _context.Cars.AsQueryable();

			List<Colors> colors = new List<Colors>();
			List<Manufacturers> manufacturers = new List<Manufacturers>();

			if (RequestModel.FilterByColor != null && RequestModel.FilterByColor.Length > 0)
			{
				var color = RequestModel.FilterByColor.Select(c => Enum.Parse<Colors>(c)).ToList();
				query = query.Where(c => color.Contains(c.Color));
			}

			if (RequestModel.FilterByManufacturer != null && RequestModel.FilterByManufacturer.Length > 0)
			{
				var manufacturer = RequestModel.FilterByManufacturer.Select(c => Enum.Parse<Manufacturers>(c)).ToList();
				query = query.Where(c =>  manufacturer.Contains(c.Manufacturer));
			}

			if (RequestModel.MinPrice.HasValue)
			{
				query = query.Where(c => c.Price >= RequestModel.MinPrice.Value);
			}

			if (RequestModel.MaxPrice.HasValue)
			{
				query = query.Where(c => c.Price <= RequestModel.MaxPrice.Value);
			}

			var totalCount = query.Count();

			var pager = new PaginationViewModel(totalCount, RequestModel.Page, RequestModel.PageSize);


			var filteredCars = query
				.Skip((RequestModel.Page - 1) * RequestModel.PageSize)
				.Take(RequestModel.PageSize)
				.Select(car => new Car()
				{
					Id = car.Id,
					Manufacturer = car.Manufacturer,
					Model = car.Model,
					Color = car.Color,
					Price = car.Price,
				}).ToList();

			var filter = new FilterViewModel()
			{
				Manufacturers = RequestModel.FilterByManufacturer,
				Colors = RequestModel.FilterByColor,
				MinPrice = RequestModel.MinPrice,
				MaxPrice = RequestModel.MaxPrice,
			};

			CarListViewModel.Cars = filteredCars;
			CarListViewModel.Pagination = pager;
			CarListViewModel.Filter = filter;

		}
	}
}
