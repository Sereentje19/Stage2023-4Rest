using System.Text;
using BLL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PL.Controllers;

namespace Tests.Controllers;

public class ProductControllerTests
{
    [Fact]
    public async Task GetAllProducts_ReturnsOkResultWithPagedProducts()
    {
        Mock<IProductService> productServiceMock = new Mock<IProductService>();
        productServiceMock.Setup(s => s.GetAllProducts(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((GetSampleProducts(), new Pager(totalItems: 10, currentPage: 1, pageSize: 5)));

        ProductController controller = new ProductController(productServiceMock.Object);
        IActionResult result = await controller.GetAllProducts("search", "dropdown", 1, 5);
       
        Assert.IsType<OkObjectResult>(result);
        productServiceMock.Verify(s => s.GetAllProducts("search", "dropdown", 1, 5), Times.Once);
    }
    
    [Fact]
    public async Task GetAllDeletedProducts_ReturnsOkResultWithPagedDeletedProducts()
    {
        Mock<IProductService> productServiceMock = new Mock<IProductService>();
        productServiceMock.Setup(s => s.GetAllDeletedProducts(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync((GetSampleProducts(), new Pager(totalItems: 10, currentPage: 1, pageSize: 5)));

        ProductController controller = new ProductController(productServiceMock.Object);
        IActionResult result = await controller.GetAllDeletedProducts("search", "dropdown", 1, 5);
        
        Assert.IsType<OkObjectResult>(result);
        productServiceMock.Verify(s => s.GetAllDeletedProducts("search", "dropdown", 1, 5), Times.Once);
    }


    private IEnumerable<object> GetSampleProducts()
    {
        return new List<object>
        {
            new Product{ProductId = 1, IsDeleted = false, SerialNumber = "crefr"},
            new Product{ProductId = 2, IsDeleted = true, SerialNumber = "23fd"}
        };
    }
    
    private IEnumerable<ProductType> GetSampleProductTypes()
    {
        return new List<ProductType>
        {
            new ProductType { Id = 1, Name = "Type1" },
            new ProductType { Id = 2, Name = "Type2" },
        };
    }
    
    [Fact]
    public async Task GetProductTypesAsync_ReturnsOkResultWithProductTypes()
    {
        Mock<IProductService> productServiceMock = new Mock<IProductService>();
        productServiceMock.Setup(s => s.GetProductTypesAsync())
            .ReturnsAsync(GetSampleProductTypes());

        ProductController controller = new ProductController(productServiceMock.Object);
        IActionResult result = await controller.GetProductTypesAsync();

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        IEnumerable<ProductType> productTypes = okResult.Value as IEnumerable<ProductType>;

        Assert.NotNull(productTypes);
        productServiceMock.Verify(s => s.GetProductTypesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task GetProductByIdAsync_ReturnsOkResultWithProduct()
    {
        const int productId = 1; 
        Mock<IProductService> productServiceMock = new Mock<IProductService>();
        productServiceMock.Setup(s => s.GetProductByIdAsync(productId))
            .ReturnsAsync(new Product{ ProductId = 1, SerialNumber = "fretgf" });

        ProductController controller = new ProductController(productServiceMock.Object);
        IActionResult result = await controller.GetProductByIdAsync(productId);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(result);
        Product product = okResult.Value as Product;

        Assert.NotNull(product);
        productServiceMock.Verify(s => s.GetProductByIdAsync(productId), Times.Once);
    }
    
    [Fact]
    public async Task CreateProductAsync_ReturnsOkResult()
    {
        Mock<IProductService> productServiceMock = new Mock<IProductService>();
        IFormFile file = GetSampleFormFile(); 
        ProductRequestDto productRequest = new ProductRequestDto
        {
            Type = new ProductType{ Name = "Laptop" },
            FileType = "SampleFileType",
            PurchaseDate = DateTime.Now,
            ExpirationDate = DateTime.Now.AddYears(1),
            SerialNumber = "SampleSerialNumber"
        };

        ProductController controller = new ProductController(productServiceMock.Object);
        IActionResult result = await controller.CreateProductAsync(file, productRequest);

        Assert.IsType<OkObjectResult>(result);
        productServiceMock.Verify(s => s.CreateProductAsync(It.IsAny<Product>()), Times.Once);
    }

    private IFormFile GetSampleFormFile()
    {
        byte[] fileBytes = "SampleFileContent"u8.ToArray();
        MemoryStream fileStream = new MemoryStream(fileBytes);
    
        return new FormFile(fileStream, 0, fileBytes.Length, "file", "sample.txt");
    }
    
    [Fact]
    public async Task UpdateProductAsync_ReturnsOkResult()
    {
        Mock<IProductService> productServiceMock = new Mock<IProductService>();
        ProductRequestDto productRequestDto = new ProductRequestDto();

        ProductController controller = new ProductController(productServiceMock.Object);
        IActionResult result = await controller.UpdateProductAsync(productRequestDto);

        Assert.IsType<OkObjectResult>(result);
        productServiceMock.Verify(s => s.UpdateProductAsync(It.IsAny<ProductRequestDto>()), Times.Once);
    }

    [Fact]
    public async Task UpdateIsDeletedAsync_ReturnsOkResult()
    {
        Mock<IProductService> productServiceMock = new Mock<IProductService>();
        ProductRequestDto productRequestDto = new ProductRequestDto();

        ProductController controller = new ProductController(productServiceMock.Object);
        IActionResult result = await controller.UpdateIsDeletedAsync(productRequestDto);

        Assert.IsType<OkObjectResult>(result);
        productServiceMock.Verify(s => s.UpdateIsDeletedAsync(It.IsAny<ProductRequestDto>()), Times.Once);
    }
    
    [Fact]
    public async Task DeleteProductAsync_ReturnsOkResult()
    {
        Mock<IProductService> productServiceMock = new Mock<IProductService>();
        ProductController controller = new ProductController(productServiceMock.Object);
        IActionResult result = await controller.DeleteProductAsync(1);

        Assert.IsType<OkObjectResult>(result);
        productServiceMock.Verify(s => s.DeleteProductAsync(1), Times.Once);
    }

    
}