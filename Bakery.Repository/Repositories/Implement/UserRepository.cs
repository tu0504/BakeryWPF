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
            var u = _context.Users.ToList();
            return u;
        }

        public User GetUserById(int id)
        {
            var u = _context.Users.FirstOrDefault(x => x.UserId == id);
            return u;
        }

        public User GetUserByEmail(string email)
        {
            var u = _context.Users.FirstOrDefault(x => x.Email.Equals(email));
            return u;
        }

        public void SaveUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }


        public void UpdateCustomer(User user)
        {
            var tracked = _context.Users.FirstOrDefault(x => x.UserId == user.UserId);
            if (tracked == null) throw new ArgumentException("User not found.");

            _context.Entry(tracked).CurrentValues.SetValues(user);
            _context.SaveChanges();
        }

        public List<User> SearchByNameOrEmail(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<User>();
            }

            string search = searchTerm.Trim().ToLower();


            var u = _context.Users
                .Where(x => x.FullName.ToLower().Contains(search) || x.Email.ToLower().Contains(search)
                )
                .Where(x => x.Status == true)
                .ToList();

            return u;
        }



        public User? GetUserByEmailAndPassword(string email, string password)
        {
            string lowerEmail = email.ToLower();

            return _context.Users.FirstOrDefault(u =>
                u.Email.ToLower() == lowerEmail &&
                u.Password == password);
        }
    }
}