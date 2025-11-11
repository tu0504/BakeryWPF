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
        public CategoryRepository _repo;

        public CategoryService (CategoryRepository repo)
        {
            _repo = repo;
        }

        public void CreateCategory(Category category)
        {
            _repo.Create(category);
        }
        public void UpdateCategory(Category category)
        {
            _repo.Update(category);
        }
        public void DeleteCategory(Category category)
        {
            _repo.Delete(category);
        }
        public List<Category> GetAllCategories()
        {
            return _repo.GetAll();
        }
        public Category GetCategoryById (int id)
        {
            return _repo.GetById(id);
        }
    }
}
