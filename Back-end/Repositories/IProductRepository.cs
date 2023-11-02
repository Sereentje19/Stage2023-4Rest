using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_end.Models;

namespace Back_end.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts(string searchfield, ProductType? dropdown);
        Product GetProductById(int id);
        void AddProduct(Product product);
        void PutProduct(Product product);
        void DeleteProduct(int id);
    }
}