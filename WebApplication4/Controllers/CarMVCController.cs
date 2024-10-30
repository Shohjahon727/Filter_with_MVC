using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication4.Data;
using WebApplication4.Entities;
using WebApplication4.Enums;
using WebApplication4.Models;
using WebApplication4.Models.RequestModels;
using WebApplication4.Models.ViewModels;

namespace WebApplication4.Controllers;

public class CarMVCController : Controller
{
	private readonly AppDbContext _context;
	public CarMVCController(AppDbContext context)
	{
		_context = context;
	}

	public IActionResult Index([FromQuery]CarListRequestModel requestModel)
	{
		var carListModel = new CarListModel(_context,requestModel);
		return View(carListModel.CarListViewModel);
	}
}

