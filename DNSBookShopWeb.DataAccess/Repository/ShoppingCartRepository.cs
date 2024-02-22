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
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _dbContext;

        public ShoppingCartRepository(ApplicationDbContext db) : base(db) 
        {
            _dbContext = db;
        }


        public void Update(ShoppingCart obj)
        {
            _dbContext.ShoppingCarts.Update(obj);
        }
    }
}
