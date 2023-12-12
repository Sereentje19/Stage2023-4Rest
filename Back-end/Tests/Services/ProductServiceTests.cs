using BLL.Services;
using DAL.Repositories;
using Moq;
using PL.Models;

namespace Tests.Services;

public class ProductServiceTests
{
    
    private static IEnumerable<object> GetSampleProducts()
    {
        return new List<object>
        {
            new { ProductId = 1, Name = "Product1", Price = 100.00 },
            new { ProductId = 2, Name = "Product2", Price = 150.00 },
        };
    }
    
    [Fact]
    public async Task GetAllProducts_ShouldReturnPagedProducts()
    {
        const string searchField = "test";
        const string dropdown = "Laptop";
        const int page = 1;
        const int pageSize = 10;

        Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
        productRepositoryMock
            .Setup(repo => repo.GetAllProducts(searchField, page, pageSize, dropdown))
            .ReturnsAsync((GetSampleProducts(), 20));

        ProductService productService = new ProductService(productRepositoryMock.Object);

        (IEnumerable<object> products, Pager pager) =
            await productService.GetAllProducts(searchField, dropdown, page, pageSize);

        Assert.NotNull(products);
        Assert.Equal(20, pager.TotalItems);
    }
    
    [Fact]
    public async Task GetAllProducts_ShouldReturnProductsAndPager()
    {
        Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
        ProductService productService = new ProductService(productRepositoryMock.Object);

        productRepositoryMock.Setup(repo => repo.GetAllProducts(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync((Enumerable.Empty<object>(), 0));

        const string searchField = "test";
        const string dropdown = "Laptop";
        const int page = 1;
        const int pageSize = 10;

        (IEnumerable<object> products, Pager pager) = await productService.GetAllProducts(searchField, dropdown, page, pageSize);

        Assert.NotNull(products);
        Assert.Empty(products);
        Assert.NotNull(pager);
        Assert.Equal(0, pager.TotalItems);
    }

    [Fact]
    public async Task GetProductTypes_ShouldReturnListOfProductTypes()
    {
        Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
        ProductService productService = new ProductService(mockRepository.Object);

        List<ProductType> productTypes = new List<ProductType>
        {
            new ProductType { Id = 1, Name = "Type1" },
            new ProductType { Id = 2, Name = "Type2" },
        };

        mockRepository.Setup(repo => repo.GetProductTypes())
            .ReturnsAsync(productTypes);

        IEnumerable<ProductType> result = await productService.GetProductTypes();

        Assert.NotNull(result);
        Assert.IsType<List<ProductType>>(result);
        Assert.Equal(2, result.Count()); 
    }

    [Fact]
    public async Task GetProductById_ShouldReturnProduct()
    {
        const int productId = 1;
        Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
        Product expectedProduct = new Product { ProductId = productId, Type  = new ProductType{ Name = "Laptop", Id = 1}, ExpirationDate = DateTime.Today, PurchaseDate = DateTime.Today.AddDays(-10), SerialNumber = "cr42w85nf4"};

        productRepositoryMock
            .Setup(repo => repo.GetProductById(productId))
            .ReturnsAsync(expectedProduct);

        ProductService productService = new ProductService(productRepositoryMock.Object);

        Product actualProduct = await productService.GetProductById(productId);

        Assert.NotNull(actualProduct);
        Assert.Equal(expectedProduct.ProductId, actualProduct.ProductId);
        Assert.Equal(expectedProduct.Type, actualProduct.Type);
        Assert.Equal(expectedProduct.ExpirationDate, actualProduct.ExpirationDate);
        Assert.Equal(expectedProduct.PurchaseDate, actualProduct.PurchaseDate);
        Assert.Equal(expectedProduct.SerialNumber, actualProduct.SerialNumber);
    }

    [Fact]
    public async Task PostProduct_ShouldAddProduct()
    {
        Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
        ProductService productService = new ProductService(productRepositoryMock.Object);
        Product newProduct = new Product {  Type  = new ProductType{ Name = "Laptop", Id = 1}, ExpirationDate = DateTime.Today, PurchaseDate = DateTime.Today.AddDays(-10), SerialNumber = "cr42w85nf4"};

        await productService.PostProduct(newProduct);
        productRepositoryMock.Verify(repo => repo.AddProduct(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task PutProduct_ShouldUpdateProduct()
    {
        Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
        ProductService productService = new ProductService(productRepositoryMock.Object);

        Product existingProduct = new Product { ProductId = 1, Type  = new ProductType{ Name = "Laptop", Id = 1}, ExpirationDate = DateTime.Today, PurchaseDate = DateTime.Today.AddDays(-10), SerialNumber = "cr42w85nf4" };

        await productService.PutProduct(existingProduct);
        productRepositoryMock.Verify(repo => repo.PutProduct(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task DeleteProduct_ShouldDeleteProduct()
    {
        Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
        ProductService productService = new ProductService(productRepositoryMock.Object);

        const int productIdToDelete = 1;

        await productService.DeleteProduct(productIdToDelete);
        productRepositoryMock.Verify(repo => repo.DeleteProduct(productIdToDelete), Times.Once);
    }



}