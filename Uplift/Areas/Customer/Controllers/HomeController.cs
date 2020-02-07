﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Extensions;
using Uplift.Models;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Controllers
{
 [Area("Customer")]
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private HomeViewModel HomeVM;

    public HomeController(IUnitOfWork unitOfWork, ILogger<HomeController> logger)
    {
      _unitOfWork = unitOfWork;
      _logger = logger;
    }

    public IActionResult Index()
    {
      HomeVM = new HomeViewModel
      {
        CategoryList = _unitOfWork.Category.GetAll(),
        ServiceList = _unitOfWork.Service.GetAll(includeProperties: "Frequency")
      };
      return View(HomeVM);
    }
    public IActionResult AddToCart(int serviceId)
    {
      List<int> sessionList = new List<int>();
      if (string.IsNullOrEmpty(HttpContext.Session.GetString(SD.SessionCart)))
      {
        sessionList.Add(serviceId);
        HttpContext.Session.SetObject(SD.SessionCart, sessionList);
      }
      else
      {
        sessionList = HttpContext.Session.GetObject<List<int>>(SD.SessionCart);
        if (!sessionList.Contains(serviceId))
        {
          sessionList.Add(serviceId);
          HttpContext.Session.SetObject(SD.SessionCart, sessionList);
        }
      }

      return RedirectToAction(nameof(Index));
    }
    public IActionResult Details(int? id)
    {
      var service = _unitOfWork.Service.GetFirstOrDefault(
        filter: s=> s.Id==id,
        includeProperties: "Category,Frequency");

      return View(service);
    }
    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
