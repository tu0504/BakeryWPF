using Bakery.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Bakery.DAL.Repositories
{
    public class UserRepository
    {
        private readonly BakeryContext _context;

        public UserRepository(BakeryContext context)
        {
            _context = context;
        }

        public User? GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
