﻿using Bulky.DataAccess.Models;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICategoryRepository _categoryRepo;

        public ProductController(IProductRepository db, ICategoryRepository cdb, IWebHostEnvironment webHostEnvironment)
        {
            _productRepo = db;
            _categoryRepo = cdb;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _productRepo.GetAll().ToList();
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _categoryRepo.GetAll().ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id==null || id==0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _productRepo.Get(u=>u.Id==id);
                return View(productVM);
            }             
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    using( var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }
                _productRepo.Add(productVM.Product);
                _productRepo.Save();
                TempData["success"] = "Product created successfully!";
                return RedirectToAction("Index", "Product");
            } 
            else
            {
                productVM.CategoryList = _categoryRepo.GetAll().ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);

            }

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _productRepo.Get(u => u.Id == id);
            //Category? categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (productFromDb == null)
            {
                return NotFound(productFromDb);
            }

            return View(productFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? product = _productRepo.Get(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _productRepo.Remove(product);
            _productRepo.Save();
            TempData["success"] = "Product deleted successfully!";
            return RedirectToAction("Index", "Product");
        }
    }
}

