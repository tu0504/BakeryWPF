using Bakery.Repository.Models;
using Bakery.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Service
{
    public class CategoryService
    {
        private CategoryRepository _repo;

       
        public void CreateCategory(Category category)
        {
            _repo = new();
            _repo.Create(category);
        }
        public void UpdateCategory(Category category)
        {
            _repo = new();
            _repo.Update(category);
        }
        public void DeleteCategory(Category category)
        {
            _repo = new();
            _repo.Delete(category);
        }
        public List<Category> GetAllCategories()
        {
            _repo = new();
            return _repo.GetAll();
        }
        public Category GetCategoryById (int id)
        {
            _repo = new();
            return _repo.GetById(id);
        }
    }
}
