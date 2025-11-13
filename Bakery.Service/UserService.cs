using Bakery.Repository.Models;
using Bakery.Repository.Repositories.Implement;
using Bakery.Repository.Repositories.Interface;
using Bakery.Service.Interface;
using Bakery.Service.Implement;
using System;
using System.Collections.Generic;

namespace Bakery.Service
{
    // Lightweight wrapper to provide the concrete type `UserService` used across the WPF project
    // Delegates to the existing implementation in Bakery.Service.Implement.UserServices
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly UserServices _impl;

        public UserService()
        {
            _repo = new UserRepository();
            _impl = new UserServices(_repo);
        }

        // Expose repository-level authentication helper expected by UI code
        public User GetUserByEmailAndPassword(string email, string password)
        {
            return _repo.GetUserByEmailAndPassword(email, password);
        }

        // Delegate IUserService methods
        public List<User> GetAll() => _impl.GetAll();
        public User GetUserById(int id) => _impl.GetUserById(id);
        public User GetUserByUserName(string userName) => _impl.GetUserByUserName(userName);
        public void SaveUser(User user) => _impl.SaveUser(user);
        public void DeleteCustomer(User user) => _impl.DeleteCustomer(user);
        public void UpdateCustomer(User user) => _impl.UpdateCustomer(user);

        public User GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public List<User> SearchByNameOrEmail(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
