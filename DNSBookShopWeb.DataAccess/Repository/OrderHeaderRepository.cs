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
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _dbContext;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db) 
        {
            _dbContext = db;
        }


        public void Update(OrderHeader obj)
        {
            _dbContext.orderHeaders.Update(obj);
        }
    }
}
