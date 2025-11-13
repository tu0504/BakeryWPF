

using Bakery.Repository.Context;
using Bakery.Repository.Models;

namespace Bakery.Repository.Repositories
{
    public class CategoryRepository
    {
        public BakeryContext _context;

        public CategoryRepository()
        {
            
        }

        public CategoryRepository(BakeryContext context) { _context = context; }

        public List<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        public Category GetById(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.CategoryId == id);
        }

        public void Create(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }
        public void Update (Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }
        public void Delete (Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

    }
}
