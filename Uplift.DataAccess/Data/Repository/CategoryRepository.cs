using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
  public class CategoryRepository : Repository<Category>, ICategoryRepository 
  {
    private readonly ApplicationDbContext _db;
    public CategoryRepository(ApplicationDbContext db) : base(db)
    {
      _db = db;
    }

    public IEnumerable<SelectListItem> GetCategoryListForDropDown()
    {
      return _db.Categories.Select(i => new SelectListItem() {
        Text  = i.Name,
        Value = i.Id.ToString()
      });
    }

    public void Update(Category category)
    {
      var obj = _db.Categories.Find(category.Id);

      obj.Name = category.Name;
      obj.DisplayOrder = category.DisplayOrder;

      _db.SaveChanges();
      //throw new NotImplementedException();
    }
  }
}
