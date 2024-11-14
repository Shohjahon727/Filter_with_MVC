using Microsoft.AspNetCore.Mvc;
using WebApplication4.Data;
using WebApplication4.Entities;
using WebApplication4.Enums;
using WebApplication4.Models;
using WebApplication4.Models.RequestModels;

namespace WebApplication4.Controllers
{
	public class CarWithComponentsController : Controller
	{
		private readonly AppDbContext _context;
        public CarWithComponentsController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
		{
			return View();
		}
		[HttpGet]
		public IActionResult Filter([FromQuery] string? filterbycolor, 
			[FromQuery] string? filterbymanufacturer, 
			[FromQuery] decimal? minPrice, 
			[FromQuery] decimal? maxPrice, 
			[FromQuery] int page=1,
			[FromQuery] int pageSize=10)
		{
			var query = _context.Cars.AsQueryable();
			var filterColors = filterbycolor?.Split(',');
			var filterManufacturers = filterbymanufacturer?.Split(',');

			List<Colors> colors = new List<Colors>();
			if(filterColors != null && filterColors.Length > 0)
			{
				colors = filterColors.Select(c => Enum.Parse<Colors>(c)).ToList();
				query = query.Where(c => colors.Contains(c.Color));
			}
			List<Manufacturers> manufacturers = new List<Manufacturers>();
			if(filterManufacturers != null && filterManufacturers.Length > 0)
			{
				manufacturers = filterManufacturers.Select(c => Enum.Parse<Manufacturers>(c)).ToList();
				query = query.Where(c => manufacturers.Contains(c.Manufacturer));
			}
			if(minPrice.HasValue)
			{
				query = query.Where(car => car.Price >= minPrice.Value);
			}
			if(maxPrice.HasValue)
			{
				query = query.Where(car => car.Price <= maxPrice.Value);
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
					page = page,
					pageSize = pageSize,
                });
			return Json(new
			{
				data = filteredCars,
				totalItems = totalCount,
			});
		}
		
	}
}
