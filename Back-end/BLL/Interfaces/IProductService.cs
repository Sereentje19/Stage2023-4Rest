using DAL.Models;

namespace BLL.Interfaces
{
    public interface IProductService
    {
        Task<(IEnumerable<object>, Pager)> GetAllProducts(string searchfield, string dropdown, int page, int pageSize);

        Task<(IEnumerable<object>, Pager)> GetAllDeletedProducts(string searchfield, string dropdown,
            int page, int pageSize);
        Task<IEnumerable<ProductType>> GetProductTypes();
        Task<Product> GetProductById(int id);
        Task PostProduct(Product product);
        Task PutProduct(Product product);
        Task PutIsDeleted(Product product);
        Task DeleteProduct(int id);
    }
}
