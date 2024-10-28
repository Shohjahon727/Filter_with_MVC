using Microsoft.AspNetCore.Mvc;
using WebApplication4.Data;
using WebApplication4.Enums;

namespace WebApplication4.Controllers
{
	public class CarController : Controller
	{
		private readonly AppDbContext _context;
        public CarController(AppDbContext context)
        {
            _context = context;
        }
		public IActionResult Index()
		{
			
			return View();
		}
		
		public IActionResult Create()
		{
			return View();
		}
		
		[HttpGet]
		public IActionResult Filter([FromQuery] string? filterbycolor,
			[FromQuery]string? filterbymanufacturer,
			[FromQuery] decimal? minPrice,
			[FromQuery] decimal? maxPrice,
			[FromQuery] int page = 1,
			[FromQuery] int pageSize = 10)
		{
			var query = _context.Cars.AsQueryable();
			var filterColors = filterbycolor?.Split(',');
			var filterManufacturers = filterbymanufacturer?.Split(',');

			List<Colors> colors = new List<Colors>();
			List<Manufacturers> manufacturers = new List<Manufacturers>();

			if(filterManufacturers !=null && filterManufacturers.Length > 0)
			{
				manufacturers = filterManufacturers.Select(c => Enum.Parse<Manufacturers>(c)).ToList();
				query = query.Where(car => manufacturers.Contains(car.Manufacturer));
			}
			if(filterColors != null && filterColors.Length > 0)
			{
				colors = filterColors.Select(c => Enum.Parse<Colors>(c)).ToList();
				query = query.Where(car => colors.Contains(car.Color));
			}
			if(minPrice.HasValue)
			{
				query = query.Where(car => car.Price >= minPrice.Value);
			}
			if(maxPrice.HasValue)
			{
				query = query.Where(car  => car.Price <= maxPrice.Value);
			}

			var totalCount = query.Count();

			var filteredCars = query
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(car => new
				{
					Id = car.Id,
					Manufacturer = car.Manufacturer,
					Color = car.Color,
					Model = car.Model,
					Price = car.Price,
				});

			return Json(new
			{
				data =filteredCars,
				totalItems = totalCount
			});
		}
	}
}
