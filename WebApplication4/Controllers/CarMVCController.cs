using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication4.Data;
using WebApplication4.Entities;
using WebApplication4.Enums;
using WebApplication4.Models;

namespace WebApplication4.Controllers;

public class CarMVCController : Controller
{
	private readonly AppDbContext _context;
	public CarMVCController(AppDbContext context)
	{
		_context = context;
	}

	public IActionResult Index(
		[FromQuery]string[]? filterbycolor,
		[FromQuery] string[]? filterbymanufacturer,
		[FromQuery] decimal? minPrice,
		[FromQuery]decimal? maxPrice,
		[FromQuery]int page =1,
		[FromQuery]int pageSize = 10)
	{
		var query = _context.Cars.AsQueryable();
		
		List<Colors> colors = new List<Colors>();
		List<Manufacturers> manufacturers = new List<Manufacturers>();

		if(filterbycolor != null && filterbycolor.Length > 0)
		{
			colors = filterbycolor.Select(c => Enum.Parse<Colors>(c)).ToList();
			query = query.Where(c => colors.Contains(c.Color));
		}

		if(filterbymanufacturer != null && filterbymanufacturer.Length > 0)
		{
			manufacturers = filterbymanufacturer.Select(c => Enum.Parse<Manufacturers>(c)).ToList();
			query = query.Where(c => manufacturers.Contains(c.Manufacturer));
		}

		if(minPrice.HasValue)
		{
			query = query.Where(c =>  c.Price >= minPrice.Value);
		}

		if(maxPrice.HasValue)
		{
			query = query.Where(c => c.Price <= maxPrice.Value);
		}

		if(page < 1)
		{
			page = 1;
		}
		
		var totalCount = query.Count();

		var pager = new PaginationViewModel(totalCount,page,pageSize);

		var filteredCars = query.
			Skip((page - 1) * pageSize).
			Take(pageSize)
			.Select(car => new Car
			{
				Id = car.Id,
				Manufacturer = car.Manufacturer,
				Model = car.Model,
				Color = car.Color,
				Price = car.Price,
			}).ToList();

		var model = (filteredCars,
			filterbymanufacturer?.ToList(),
			filterbycolor?.ToList(),
			pager);

		return View(model);
	}
}

