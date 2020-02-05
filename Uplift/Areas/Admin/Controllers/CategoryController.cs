using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.Areas.Admin.Controllers
{
  [Area("Admin")]
  public class CategoryController : Controller
  {
    private readonly IUnitOfWork _unitOfWork;
    public CategoryController(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
      return View();
    }
    public IActionResult Upsert(int? id)
    {
      var item = new Category();
      if (id == null) {
        return View(item);
      }
      item = _unitOfWork.Category.Get(id.Value);
      if (item == null)
      {
        return NotFound();
      }

      return View(item);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(Category category)
    {
      if (ModelState.IsValid)
      {
        if (category.Id == 0)
        {
          _unitOfWork.Category.Add(category);
        }
        else
        {
          _unitOfWork.Category.Update(category);
        }
        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
      }
      return View(category);
    }

    #region API CALLS
    [HttpGet]
    public IActionResult GetAll() {
      return Json(new {
        data = _unitOfWork.Category.GetAll()
      });
    }
    [HttpDelete]
    public IActionResult Delete(int id)
    {
      var obj = _unitOfWork.Category.Get(id);
      if (obj == null)
      {
        return Json(new { 
          success=false,
          message="Error while deleting"
        });
      }

      _unitOfWork.Category.Remove(obj);
      _unitOfWork.Save();
      return Json(new
      {
        success = true,
        message = "Delete successfully"
      });
    }
    #endregion
  }
}