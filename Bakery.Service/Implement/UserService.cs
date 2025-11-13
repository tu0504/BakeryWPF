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

        public User GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }

        public void SaveUser(User user)
        {
            _userRepository.SaveUser(user);
        }

        public List<User> SearchByNameOrEmail(string searchTerm)
        {
            return _userRepository.SearchByNameOrEmail(searchTerm);
        }

        public void UpdateCustomer(User user)
        {
            User existingUser = _userRepository.GetUserById(user.UserId);

            if (existingUser == null)
            {
                throw new ArgumentException($"User with ID {user.UserId} not found.");
            }

            User userWithSameEmail = _userRepository.GetUserByEmail(user.Email); // Giả định có hàm này
    
    if (userWithSameEmail != null && userWithSameEmail.UserId != user.UserId)
    {
        // Trùng email với người dùng khác!
        throw new ArgumentException("Email này đã được sử dụng bởi người dùng khác.");
    }
    
  
            // 2. Cập nhật các trường (Field-by-Field update)
            existingUser.UserName = user.UserName;
            existingUser.FullName = user.FullName;
            existingUser.Email = user.Email;
            existingUser.Role = user.Role;
            existingUser.Address = user.Address;
            existingUser.Phone = user.Phone;
            existingUser.Status = user.Status;

            

            // 4. Gọi Repository để lưu thay đổi (Repository chỉ cần SaveChanges() vì entity đang được theo dõi)
            _userRepository.UpdateCustomer(existingUser);
        }
    }
}
