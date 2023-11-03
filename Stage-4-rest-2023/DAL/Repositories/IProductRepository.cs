using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stage4rest2023.Models;

namespace Stage4rest2023.Repositories
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