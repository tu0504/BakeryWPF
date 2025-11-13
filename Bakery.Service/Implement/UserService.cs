using Bakery.Repository.Models;
using Bakery.Repository.Repositories.Implement;
using Bakery.Repository.Repositories.Interface;
using Bakery.Service.Interface;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Bakery.Service.Implement
{
    public class UserServices : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserServices(IUserRepository repo)
        {
            _userRepository = repo;
        }

        public void DeleteCustomer(User user)
        {
            _userRepository.DeleteCustomer(user);
        }

        public List<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public User GetUserByUserName(string userName)
        {
            return _userRepository.GetUserByUserName(userName);
        }

        public void SaveUser(User user)
        {
            _userRepository.SaveUser(user);
        }

        public List<User> SearchByName(string fullName)
        {
            return _userRepository.SearchByName(fullName);
        }

        public void UpdateCustomer(User user)
        {
            _userRepository.UpdateCustomer(user);
        }
        public User AuthenticateUser(string email, string password)
        {
          

            User user = _userRepository.GetUserByEmailAndPassword(email, password);

            if (user == null)
            {
                return null; 
            }

           
            if (user.Status == false)
            {
                throw new UnauthorizedAccessException("Tài khoản của bạn đã bị khóa.");
            }

            return user;
        }
    }
}
