using Bulky.DataAccess.Models;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository _companyRepo;

        public CompanyController(ICompanyRepository codb)
        {
            _companyRepo = codb;
        }

        public IActionResult Index()
        {
            List<Company> objProductList = _companyRepo.GetAll().ToList();
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            if (id==null || id==0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company company = _companyRepo.Get(u=>u.Id==id);
                return View(company);
            }             
        }
        [HttpPost]
        public IActionResult Upsert(Company company, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
            
                if (company.Id == 0)
                {
                    _companyRepo.Add(company);
                }
                else
                {
                    _companyRepo.Update(company);
                }

                _companyRepo.Save();
                TempData["success"] = "Company created successfully!";
                return RedirectToAction("Index", "Company");
            } 
            else
            {
                return View(company);

            }

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Company? companyFromDb = _companyRepo.Get(u => u.Id == id);
            //Category? categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (companyFromDb == null)
            {
                return NotFound(companyFromDb);
            }

            return View(companyFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Company? company = _companyRepo.Get(u => u.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            _companyRepo.Remove(company);
            _companyRepo.Save();
            TempData["success"] = "Company deleted successfully!";
            return RedirectToAction("Index", "Company");
        }
    }
}

