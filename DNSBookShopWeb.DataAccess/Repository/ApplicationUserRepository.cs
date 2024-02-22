using DNSBookShopWeb.DataAccess.Data;
using DNSBookShopWeb.DataAccess.Repository.IRepository;
using DNSBookShopWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DNSBookShopWeb.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _dbContext;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db) 
        {
            _dbContext = db;
        }
    }
}
