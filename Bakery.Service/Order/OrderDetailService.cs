using Bakery.Repository.Models;
using Bakery.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Service
{
    public interface IOrderDetailService
    {
        List<OrderDetail> GetAll();
        OrderDetail GetById(int id);
        void Create(OrderDetail entity);
        void Update(OrderDetail entity);
        bool Remove(OrderDetail entity);
    }

    public class OrderDetailService
    {
        private readonly IOrderDetailRepo _orderDetailRepository;

        public OrderDetailService(IOrderDetailRepo orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        public List<OrderDetail> GetAll()
        {
            try
            {
                return _orderDetailRepository.GetAll();
            }
            catch (Exception ex)
            {
                // Log exception or handle as needed
                throw new Exception("Error retrieving all order details", ex);
            }
        }

        public OrderDetail GetById(int id)
        {
            try
            {
                return _orderDetailRepository.GetById(id);
            }
            catch (Exception ex)
            {
                // Log exception or handle as needed
                throw new Exception($"Error retrieving order detail with ID {id}", ex);
            }
        }

        public void Create(OrderDetail entity)
        {
            try
            {
                _orderDetailRepository.Create(entity);
            }
            catch (Exception ex)
            {
                // Log exception or handle as needed
                throw new Exception("Error creating order detail", ex);
            }
        }

        public void Update(OrderDetail entity)
        {
            try
            {
                _orderDetailRepository.Update(entity);
            }
            catch (Exception ex)
            {
                // Log exception or handle as needed
                throw new Exception($"Error updating order detail with ID {entity.OrderDetailId}", ex);
            }
        }

        public bool Remove(OrderDetail entity)
        {
            try
            {
                return _orderDetailRepository.Remove(entity);
            }
            catch (Exception ex)
            {
                // Log exception or handle as needed
                throw new Exception($"Error removing order detail with ID {entity.OrderDetailId}", ex);
            }
        }
        
    }
}
