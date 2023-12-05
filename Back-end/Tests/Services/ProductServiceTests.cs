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
        const ProductType dropdown = ProductType.Laptop;
        const int page = 1;
        const int pageSize = 10;

        Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
        productRepositoryMock
            .Setup(repo => repo.GetAllProducts(searchField, dropdown, page, pageSize))
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

        productRepositoryMock.Setup(repo => repo.GetAllProducts(It.IsAny<string>(), It.IsAny<ProductType?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((Enumerable.Empty<object>(), 0));

        const string searchField = "test";
        ProductType? dropdown = ProductType.Laptop;
        const int page = 1;
        const int pageSize = 10;

        (IEnumerable<object> products, Pager pager) = await productService.GetAllProducts(searchField, dropdown, page, pageSize);

        Assert.NotNull(products);
        Assert.Empty(products);
        Assert.NotNull(pager);
        Assert.Equal(0, pager.TotalItems);
    }

    [Fact]
    public void GetProductTypeStrings_ShouldReturnProductTypeStrings()
    {
        Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
        List<string> expectedProductTypes = new List<string> { "Laptop", "Desktop", "Mobile" };

        productRepositoryMock
            .Setup(repo => repo.GetProductTypeStrings())
            .Returns(expectedProductTypes);

        ProductService productService = new ProductService(productRepositoryMock.Object);
        List<string> actualProductTypes = productService.GetProductTypeStrings();

        Assert.NotNull(actualProductTypes);
        Assert.Equal(expectedProductTypes.Count, actualProductTypes.Count);
        Assert.Equal(expectedProductTypes, actualProductTypes);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnProduct()
    {
        const int productId = 1;
        Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
        Product expectedProduct = new Product { ProductId = productId, Type  = ProductType.Laptop, ExpirationDate = DateTime.Today, PurchaseDate = DateTime.Today.AddDays(-10), SerialNumber = "cr42w85nf4"};

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
        Product newProduct = new Product {  Type  = ProductType.Laptop, ExpirationDate = DateTime.Today, PurchaseDate = DateTime.Today.AddDays(-10), SerialNumber = "cr42w85nf4"};

        await productService.PostProduct(newProduct);
        productRepositoryMock.Verify(repo => repo.AddProduct(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task PutProduct_ShouldUpdateProduct()
    {
        Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
        ProductService productService = new ProductService(productRepositoryMock.Object);

        Product existingProduct = new Product { ProductId = 1, Type  = ProductType.Laptop, ExpirationDate = DateTime.Today, PurchaseDate = DateTime.Today.AddDays(-10), SerialNumber = "cr42w85nf4" };

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