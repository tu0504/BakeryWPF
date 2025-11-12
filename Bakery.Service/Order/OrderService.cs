using Bakery.Repository.Model;
using Bakery.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Service
{
    public interface IOrderService
    {
        List<Order> GetAll();
        Order GetById(int id);
        void Create(Order entity);
        void Update(Order entity);
        bool Remove(Order entity);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;

        public OrderService(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public void Create(Order entity)
        {
            try
            {
                _orderRepo.Create(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the order.", ex);
            }
        }

        public List<Order> GetAll()
        {
            try
            {
                return _orderRepo.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving orders.", ex);
            }
        }

        public Order GetById(int id)
        {
            try
            {
                return _orderRepo.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the order.", ex);
            }
        }

        public bool Remove(Order entity)
        {
            try
            {
                if (_orderRepo.GetById(entity.OrderId) == null)
                {
                    return false;
                }

                return _orderRepo.Remove(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing the order.", ex);
            }
        }

        public void Update(Order entity)
        {
            try
            {
                _orderRepo.Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the order.", ex);
            }
        }
    }
}
