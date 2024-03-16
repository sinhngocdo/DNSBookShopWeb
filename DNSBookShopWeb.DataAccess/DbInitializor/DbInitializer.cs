using DNSBookShopWeb.DataAccess.Data;
using DNSBookShopWeb.Models;
using DNSBookShopWeb.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNSBookShopWeb.DataAccess.DbInitializor
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, 
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;

        }

        public void Initialize()
        {
            //migrations if they are not applied
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _dbContext.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

                
            }


            //Create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();

                //If roles are not created, they we will create admin user as well
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@dnscompany.com",
                    Email = "admin@dnscompany.com",
                    Name = "DNSCompany",
                    PhoneNumber = "1234567890",
                    StreetAddress = "ID 23252 VA",
                    State = "CA",
                    PostalCode = "94040",
                    City = "QN City"
                }, "Admin@2801").GetAwaiter().GetResult();

                ApplicationUser user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@dnscompany.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }


            return;
        }
    }
}
