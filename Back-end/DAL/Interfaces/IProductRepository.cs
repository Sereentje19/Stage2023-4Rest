using DAL.Models;

namespace DAL.Interfaces
{
    public interface IProductRepository
    {
        Task<(IEnumerable<object>, int)> GetAllProducts(string searchfield, int page,
            int pageSize, string dropdown);

        Task<(IEnumerable<object>, int)> GetAllDeletedProducts(string searchfield, int page,
            int pageSize, string dropdown);
        Task<IEnumerable<ProductType>> GetProductTypesAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task UpdateIsDeletedAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
