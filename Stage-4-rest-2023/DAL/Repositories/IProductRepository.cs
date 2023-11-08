using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stage4rest2023.Models;

namespace Stage4rest2023.Repositories
{
    public interface IProductRepository
    {
        (IEnumerable<object>, int) GetAllProducts(string searchfield, ProductType? dropdown, int page, int pageSize);
        Product GetProductById(int id);
        void AddProduct(Product product);
        void PutProduct(Product product);
        void DeleteProduct(int id);
    }
}