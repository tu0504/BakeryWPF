using Bakery.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Base;

namespace Bakery.Repository.Repositories
{
    public interface IOrderRepo
    {
        List<Order> GetAll();
        Order? GetById(int id);
        void Create(Order entity);
        void Update(Order entity);
        bool Remove(Order entity);
    }

    public class OrderRepo : GenericRepository<Order>, IOrderRepo
    {
        // Use 'new' because base GenericRepository already defines GetAll/ Create/Update/Remove/GetById
        public new List<Order> GetAll()
        {
            return _context.Orders
        .Include(o => o.User)
        .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .ToList();
        }

        public new Order? GetById(int id)
        {
            return _context.Orders
                .Include(o => o.User)
        .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .FirstOrDefault(o => o.OrderId == id);
        }
        public new void Create(Order entity)
        {
            // Call base implementation to avoid recursion
            base.Create(entity);
        }

        public new void Update(Order entity)
        {
            base.Update(entity);
        }

        public new bool Remove(Order entity)
        {
            return base.Remove(entity);
        }
    }
}
