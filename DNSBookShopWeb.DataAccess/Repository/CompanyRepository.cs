using DNSBookShopWeb.DataAccess.Data;
using DNSBookShopWeb.DataAccess.Repository.IRepository;
using DNSBookShopWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNSBookShopWeb.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext _dbContext;

        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _dbContext = db;
        }

        public void Update(Company obj)
        {
            _dbContext.Companies.Update(obj);
        }
    }
}
