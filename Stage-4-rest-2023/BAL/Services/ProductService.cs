using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stage4rest2023.Models;
using Stage4rest2023.Repositories;

namespace Stage4rest2023.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public (IEnumerable<object>, Pager) GetAllProducts(string searchfield, ProductType? dropdown, int page, int pageSize)
        {
            var (products, numberOfProducts) = _productRepository.GetAllProducts(searchfield, dropdown, page, pageSize);
            Pager pager = new Pager(numberOfProducts, page, pageSize);
            return (products, pager);
        }

        public Product GetProductById(int id)
        {
            return _productRepository.GetProductById(id);
        }

        public void PostProduct(Product product)
        {
            _productRepository.AddProduct(product);
        }

        public void PutProduct(Product product)
        {
            _productRepository.PutProduct(product);
        }

        public void DeleteProduct(int id)
        {
            _productRepository.DeleteProduct(id);
        }
    }
}