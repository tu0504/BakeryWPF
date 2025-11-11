
using Bakery.Repository.Context;
using Bakery.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Repository.Repositories
{
    public class ProductRepository
    {
     public BakeryContext _context;

        public List <Product> GetAll()
        {
            _context = new BakeryContext();
            return _context.Products.Include("Category").ToList();
        }

        public void Create(Product product)
        {
            _context = new BakeryContext();
             _context.Products.Add(product);
            _context.SaveChanges();
        }
        public void Update(Product product)
        {
            _context = new BakeryContext();
            _context.Products.Update(product);
            _context.SaveChanges();
        }
        public void Delete(Product product)
        {
            _context = new BakeryContext();
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
        public Product GetById(int id)
        {
            _context = new();
            return _context.Products.Include("Category").FirstOrDefault(p => p.ProductId == id);
        }

        public List<Product> GetByName(string name)
        {
            _context = new BakeryContext();
            return _context.Products.Include(p => p.Category).Where(p => p.ProductName.Contains(name)).ToList();
        }
    }
}
