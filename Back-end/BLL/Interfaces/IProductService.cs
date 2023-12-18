using DAL.Models;

namespace BLL.Interfaces
{
    public interface IProductService
    {
        Task<(IEnumerable<object>, Pager)> GetAllProducts(string searchfield, string dropdown, int page, int pageSize);

        Task<(IEnumerable<object>, Pager)> GetAllDeletedProducts(string searchfield, string dropdown,
            int page, int pageSize);
        Task<IEnumerable<ProductType>> GetProductTypesAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task UpdateIsDeletedAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
