using DNSBookShopWeb.DataAccess.Repository.IRepository;
using DNSBookShopWeb.Models.ViewModels;
using DNSBookShopWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DNSBookShopWeb.Utility;
using Microsoft.AspNetCore.Authorization;

namespace DNSBookShopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

            return View(objCompanyList);
        }


        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category
            //    .GetAll().Select(u => new SelectListItem
            //    {//projections in EF Core
            //        Text = u.Name,
            //        Value = u.Id.ToString()
            //    });

            ////ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;
            
            if (id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update function
                Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            //if (obj.Title == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "The display order cannot exactly match the Name");
            //}


            if (ModelState.IsValid)
            {
                if (companyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(companyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(companyObj);
                }
                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {             
                return View(companyObj);
            }

            //return View();
        }

        #region old Edit function
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Company? CompanyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
        //    //Company? CompanyFromDb = _dbContext.Companys.FirstOrDefault(u=>u.Id == id);
        //    //Company? CompanyFromDb = _dbContext.Companys.Where(u=>u.Id==id).FirstOrDefault();

        //    if (CompanyFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(CompanyFromDb);
        //}

        //[HttpPost]
        //public IActionResult Edit(Company obj)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Company.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Company updated successfully";
        //        return RedirectToAction("Index");
        //    }

        //    return View();
        //}
        #endregion



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting " });
            }


            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete successful " });
        }
        #endregion
    }
}
