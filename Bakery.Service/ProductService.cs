using Bakery.Repository.Models;
using Bakery.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakery.Service
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService()
        {
            _productRepository = new ProductRepository();
        }

        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAll();
        }

        public Product? GetProductById(int id)
        {
            return _productRepository.GetById(id);
        }

        public List<Product> GetProductsByName(string name)
        {
            return _productRepository.GetByName(name);
        }

        public void CreateProduct(Product product)
        {
            _productRepository.Create(product);
        }

        public void UpdateProduct(Product product)
        {
           _productRepository.Update(product);
        }

        public void DeleteProduct(Product product)
        {
            _productRepository.Delete(product);
        }
    }
}

