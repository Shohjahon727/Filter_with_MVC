﻿using Microsoft.AspNetCore.Mvc;
using WebApplication4.Data;
using WebApplication4.Entities;
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

		[HttpPost]
		public IActionResult Create(Car car)
		{
			if(ModelState.IsValid)
			{
				_context.Add(car);
				_context.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(car);
		}
		
		[HttpGet]
		public IActionResult Filter([FromQuery] string? filterbycolor,
			[FromQuery]string? filterbymanufacturer,
			[FromQuery] decimal? minPrice,
			[FromQuery] decimal? maxPrice,
			[FromQuery] int page = 1,
			[FromQuery] int pageSize = 10,
			[FromQuery] string? sortBy = null,    
			[FromQuery] string? sortOrder = null) 
		{
			var query = _context.Cars.AsQueryable();
			var filterColors = filterbycolor?.Split(',');
			var filterManufacturers = filterbymanufacturer?.Split(',');

			List<Colors> colors = new List<Colors>();
			List<Manufacturers> manufacturers = new List<Manufacturers>();

			if (filterManufacturers !=null && filterManufacturers.Length > 0)
			{
				manufacturers = filterManufacturers.Select(c => Enum.Parse<Manufacturers>(c)).ToList();
				query = query.Where(car => manufacturers.Contains(car.Manufacturer));
			}
			if (filterColors != null && filterColors.Length > 0)
			{
				colors = filterColors.Select(c => Enum.Parse<Colors>(c)).ToList();
				query = query.Where(car => colors.Contains(car.Color));
			}
			if (minPrice.HasValue)
			{
				query = query.Where(car => car.Price >= minPrice.Value);
			}
			if (maxPrice.HasValue)
			{
				query = query.Where(car  => car.Price <= maxPrice.Value);
			}

			bool isDescending = sortOrder?.ToLower() == "desc";

			query = sortBy?.ToLower() switch
			{
				"price" => isDescending
					? query.OrderByDescending(car => car.Price)
					: query.OrderBy(car => car.Price),
				"manufacturer" => isDescending
					? query.OrderByDescending(car => car.Manufacturer)
					: query.OrderBy(car => car.Manufacturer),
				"color" => isDescending
					? query.OrderByDescending(car => car.Color)
					: query.OrderBy(car => car.Color),
				"model" => isDescending
					? query.OrderByDescending(car => car.Model)
					: query.OrderBy(car => car.Model),
				_ => query 
			};

			//if (!string.IsNullOrEmpty(sortBy))
			//{
			//	if (sortOrder == "asc")
			//	{
			//		query = sortBy switch
			//		{
			//			"id" => query.OrderBy(c => c.Id),
			//			"manufacturer" => query.OrderBy(c => c.Manufacturer),
			//			"model" => query.OrderBy(c => c.Model),
			//			"color" => query.OrderBy(c => c.Color),
			//			"price" => query.OrderBy(c => c.Price),
			//			_ => query
			//		};
			//	}
			//	else if (sortOrder == "desc")
			//	{
			//		query = sortBy switch
			//		{
			//			"id" => query.OrderByDescending(c => c.Id),
			//			"manufacturer" => query.OrderByDescending(c => c.Manufacturer),
			//			"model" => query.OrderByDescending(c => c.Model),
			//			"color" => query.OrderByDescending(c => c.Color),
			//			"price" => query.OrderByDescending(c => c.Price),
			//			_ => query
			//		};
			//	}
			//}

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
