using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;
using Back_end.Repositories;

namespace Back_end.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public (IEnumerable<object>, Pager) GetAll(int page, int pageSize)
        {
            IEnumerable<Product> products = _productRepository.GetAll();

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

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }
    }
}