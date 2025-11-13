

using Bakery.Repository.Context;
using Bakery.Repository.Models;

namespace Bakery.Repository.Repositories
{
    public class CategoryRepository
    {
        private  BakeryContext _context;


        public List<Category> GetAll()
        {
            _context = new();
            return _context.Categories.ToList();
        }

        public Category GetById(int id)
        {
            _context = new();
            return _context.Categories.FirstOrDefault(c => c.CategoryId == id);
        }

        public void Create(Category category)
        {
            _context = new();
            _context.Categories.Add(category);
            _context.SaveChanges();
        }
        public void Update (Category category)
        {
            _context = new();
            _context.Categories.Update(category);
            _context.SaveChanges();
        }
        public void Delete (Category category)
        {
            _context = new();
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

    }
}
