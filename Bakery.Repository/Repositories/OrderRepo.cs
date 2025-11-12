using Bakery.Repository.Model;
using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Repository.Repositories
{
    public interface IOrderRepo
    {
        List<Order> GetAll();
        Order GetById(int id);
        void Create(Order entity);
        void Update(Order entity);
        bool Remove(Order entity);
    }

    public class OrderRepo : GenericRepository<Order>, IOrderRepo
    {
        public List<Order> GetAll()
        {
            return _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ToList();
        }

        public Order? GetById(int id)
        {
            return _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.OrderId == id);
        }

        public void Create(Order entity)
        {
            Create(entity);
        }

        public void Update(Order entity)
        {
            Update(entity);
        }

        public void Remove(Order entity)
        {
            Remove(entity);
        }
    }
}
