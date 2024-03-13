using DNSBookShopWeb.DataAccess.Repository.IRepository;
using DNSBookShopWeb.Models.ViewModels;
using DNSBookShopWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DNSBookShopWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using DNSBookShopWeb.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using DNSBookShopWeb.DataAccess.Repository;
using Microsoft.AspNetCore.Identity;

namespace DNSBookShopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManagement(string userId)
        {
            //update function
            string roleId = _dbContext.UserRoles.FirstOrDefault(u=> u.UserId == userId).RoleId;

            RoleManagementVM roleVm = new RoleManagementVM()
            {
                ApplicationUser = _dbContext.ApplicationUsers.Include(u=>u.Company).FirstOrDefault(u=>u.Id == userId),
                RoleList = _dbContext.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _dbContext.Companies.Select(i=> new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            roleVm.ApplicationUser.Role = _dbContext.Roles.FirstOrDefault(u => u.Id == roleId).Name;

            return View(roleVm);
        }

        [HttpPost]
        public IActionResult RoleManagement(RoleManagementVM roleManagementVM)
        {
            
            string RoleId = _dbContext.UserRoles.FirstOrDefault(u => u.UserId == roleManagementVM.ApplicationUser.Id).RoleId;
            string oldRole = _dbContext.Roles.FirstOrDefault(u => u.Id == RoleId).Name;

            if (!(roleManagementVM.ApplicationUser.Role == oldRole))
            {
                //a role was updated
                ApplicationUser applicationUser = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Id == roleManagementVM.ApplicationUser.Id);
                if(roleManagementVM.ApplicationUser.Role == SD.Role_Company)
                {
                    applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
                }
                if(oldRole == SD.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }

                _dbContext.SaveChanges();

                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagementVM.ApplicationUser.Role).GetAwaiter().GetResult();
            }
            TempData["Success"] = "Change Role successfully";

            return RedirectToAction("Index");
        }







        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList = _dbContext.ApplicationUsers.Include(u=>u.Company).ToList();

            var userRoles = _dbContext.UserRoles.ToList();
            var roles = _dbContext.Roles.ToList();

            foreach (var user in objUserList)
            {
                var roleId = userRoles.FirstOrDefault(u=> u.UserId== user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

                if (user.Company == null)
                {
                    user.Company = new() { Name = "" };
                }
            }

            return Json(new { data = objUserList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id)
        {
            var objFromDb = _dbContext.ApplicationUsers.FirstOrDefault(u=>u.Id== id);
            if(objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }

            if(objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now) 
            {
                //user is currently locked and we need to unlock them
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "Operation successful " });
        }
        #endregion
    }
}
