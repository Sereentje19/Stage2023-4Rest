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
            IEnumerable<Product> products = _productRepository.GetAllProducts(searchfield, dropdown);

            int skipCount = Math.Max(0, (page - 1) * pageSize);
            var pager = new Pager(products.Count(), page, pageSize);

            var pagedproducts = products
            .Skip(skipCount)
            .Take(pageSize)
            .Select(pro => new
            {
                pro.SerialNumber,
                pro.PurchaseDate,
                pro.ExpirationDate,
                pro.ProductId,
                Type = pro.Type.ToString(),
            })
            .ToList();
            return (pagedproducts.Cast<object>(), pager);
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