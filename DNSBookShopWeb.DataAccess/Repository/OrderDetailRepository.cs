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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private ApplicationDbContext _dbContext;

        public OrderDetailRepository(ApplicationDbContext db) : base(db) 
        {
            _dbContext = db;
        }


        public void Update(OrderDetail obj)
        {
            _dbContext.OrderDetails.Update(obj);
        }
    }
}
