using Bakery.Repository.Context;
using Bakery.Repository.Models;
using Bakery.Repository.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Repository.Repositories.Implement
{
    public class UserRepository : GenericRepository<UserRepository>, IUserRepository
    {
        public UserRepository() { }

        public void DeleteCustomer(User user)
        {
            var u = _context.Users.FirstOrDefault(x => x.UserId == user.UserId);

            if (u != null)
            {
                u.Status = false;
                _context.SaveChanges();
            }

            _context.Users.Remove(u);
            _context.SaveChanges();
        }

        public List<User> GetAll()
        {
            var u = _context.Users.Where(x => x.Status == true).ToList();
            return u;
        }

        public User GetUserById(int id)
        {
            var u = _context.Users.FirstOrDefault(x => x.UserId == id);
            return u;
        }

        public User GetUserByUserName(string userName)
        {
            var u = _context.Users.FirstOrDefault(x => x.UserName.Equals(userName));
            return u;
        }

        public void SaveUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }


        public void UpdateCustomer(User user)
        {
            var u = _context.Users.FirstOrDefault(x => x.UserId == user.UserId);

            if (u != null)
            {
                _context.Update(user);
                _context.SaveChanges();
            }
        }

        public List<User> SearchByName(string fullName)
        {
            var u = _context.Users
                .Where(x => x.FullName.Contains(fullName) && x.Status == true)
                .ToList();
            return u;
        }

       

        public User? GetUserByEmailAndPassword(string email, string password)
        {
            return _context.Users.FirstOrDefault(u =>
                u.Email == email &&
                u.Password == password);
        }
    }
}