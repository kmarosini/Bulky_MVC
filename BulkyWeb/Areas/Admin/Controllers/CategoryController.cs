using Bulky.DataAccess.Migrations;
using Bulky.DataAccess.Models;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository db)
        {
            _categoryRepo = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _categoryRepo.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Display order cannot exactly match the name.");
            }

            if (ModelState.IsValid)
            {
                _categoryRepo.Add(obj);
                _categoryRepo.Save();
                TempData["success"] = "Category created successfully!";
                return RedirectToAction("Index", "Category");
            }
            return View();

        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _categoryRepo.Get(u => u.Id == id);
            //Category? categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound(categoryFromDb);
            }

            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(obj);
                _categoryRepo.Save();
                TempData["success"] = "Category updated successfully!";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _categoryRepo.Get(u => u.Id == id);
            //Category? categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound(categoryFromDb);
            }

            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? category = _categoryRepo.Get(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _categoryRepo.Remove(category);
            _categoryRepo.Save();
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("Index", "Category");
        }
    }
}
