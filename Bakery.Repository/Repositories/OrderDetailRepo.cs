using Bakery.Repository.Models;
using Bakery.Repository.Context;
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
        OrderDetail? GetById(int id);
        void Create(OrderDetail entity);
        void Update(OrderDetail entity);
        bool Remove(OrderDetail entity);
    }

    public class OrderDetailRepo : GenericRepository<OrderDetail>, IOrderDetailRepo
    {
        // mark as new to indicate intentional hiding of base generic methods
        public new List<OrderDetail> GetAll()
        {
            return _context.OrderDetails
                .Include(o => o.Order)
                .Include(p => p.Product)
                .ToList();
        }
        public new OrderDetail? GetById(int id)
        {
            return _context.OrderDetails
                .Include(o => o.Order)
                .Include(p => p.Product)
                .FirstOrDefault(od => od.OrderDetailId == id);
        }
        public new void Create(OrderDetail entity)
        {
            base.Create(entity);
        }

        public new void Update(OrderDetail entity)
        {
            base.Update(entity);
        }

        public new bool Remove(OrderDetail entity)
        {
            return base.Remove(entity);
        }
    }
}
