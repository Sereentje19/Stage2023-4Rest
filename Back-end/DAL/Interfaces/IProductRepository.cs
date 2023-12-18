using DAL.Models;
using DAL.Models.Requests;

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
        Task UpdateProductAsync(ProductRequestDto productRequest);
        Task UpdateIsDeletedAsync(ProductRequestDto productRequest);
        Task DeleteProductAsync(int id);
    }
}
