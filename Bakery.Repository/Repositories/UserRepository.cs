using Bakery.Repository.Context;
using Bakery.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Repository.Repositories
{

    public class UserRepository
    {
        public BakeryContext _ctx;

        public List<User> GetAll()
        {
            _ctx = new();
            return _ctx.Users.ToList();
        }
        public void Add(User user)
        {
            _ctx = new();
            _ctx.Users.Add(user);
            _ctx.SaveChanges();
        }
        public void Update(User user)
        {
            _ctx = new();
            _ctx.Users.Update(user);
            _ctx.SaveChanges();
        }
        public void Delete(User user)
        {
            _ctx = new();
            _ctx.Users.Remove(user);
            _ctx.SaveChanges();
        }
        public User GetById(int id) {
            _ctx = new();
            return _ctx.Users.Find(id);
        }
        public User GetByEmail(string email) 
        {
            _ctx = new();
            return _ctx.Users.FirstOrDefault(u => u.Email == email);
        }

        public bool ExistsByEmailOrPhone(string email, string phone)
        {
            _ctx = new();
            return _ctx.Users.Any( u => u.Email == email || u.Phone == phone);
        }
    }
}
