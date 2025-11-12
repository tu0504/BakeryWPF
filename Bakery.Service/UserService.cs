using Bakery.Repository.Models;
using Bakery.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
namespace Bakery.Service
{
    public class UserService
    {
        public UserRepository _repo;
        
        public List<User> GetAllUsers()
        {
            return _repo.GetAll();
        }
        public void UpdateUser(User user)
        {
             _repo.Update(user);
        }
        public void DeleteUser(User user)
        {
            _repo.Delete(user);
        }

        // Logic đăng kí người dùng
        public User RegisterNewUser (User newUser, string confirmPassword)
        {
            // 1. Kiểm tra Password có khớp với confirm không và not blank
            if (string.IsNullOrEmpty(newUser.Password) || newUser.Password != confirmPassword)
            {
                throw new ArgumentException("Password and confirm password do not match.");
            }
            // 2. Check tồn tại email và phone
            if (_repo.ExistsByEmailOrPhone(newUser.Email, newUser.Phone))
            {
                throw new ArgumentException("Email or Phone Number already exists.");
            }
            // 3. Mã hóa password
            newUser.Password = HashPassword(newUser.Password);
            // 4. Lưu new user
            _repo.Add(newUser);
            return newUser;
        }

        // Login 
        public User Authenticate(string email, string password)
        {
            var user = _repo.GetByEmail(email);
            if (user == null)
            {
                return null;
            }
            if (VerifyPassword(password, user.Password))
            {
                return user;
            }
            return null;

        }
        // Hàm helper HashPassword
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Hàm helper verify password
        private bool VerifyPassword(string rawPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(rawPassword, hashedPassword);
        }

    }
}
