using Bakery.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Repository.Repositories
{
    public interface IOrderDetailRepo
    {
        List<OrderDetail> GetAll();
        OrderDetail GetById(int id);
        void Create(OrderDetail entity);
        void Update(OrderDetail entity);
        bool Remove(OrderDetail entity);
    }

    public class OrderDetailRepo : GenericRepository<OrderDetail>, IOrderDetailRepo
    {
        public List<OrderDetail> GetAll()
        {
            return _context.OrderDetails
                .Include(o => o.Order)
                .Include(p => p.Product)
                .ToList();
        }
        public OrderDetail? GetById(int id)
        {
            return _context.OrderDetails
                .Include(o => o.Order)
                .Include(p => p.Product)
                .FirstOrDefault(od => od.OrderDetailId == id);
        }
        public void Create(OrderDetail entity)
        {
            Create(entity);
        }
        public void Update(OrderDetail entity)
        {
            Update(entity);
        }
        public bool Remove(OrderDetail entity)
        {
            return Remove(entity);
        }
    }
}
