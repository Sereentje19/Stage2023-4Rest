using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using PL.Exceptions;
using PL.Models;
using PL.Models.Responses;

namespace Tests.Repositories
{

    public class ProductRepositoryTests
    {
        private readonly List<Product> _products = new()
        {
            new Product
            {
                ProductId = 1, SerialNumber = "12345", ExpirationDate = DateTime.Now.AddDays(30),
                PurchaseDate = DateTime.Now, Type = ProductType.Laptop
            },
            new Product
            {
                ProductId = 2, SerialNumber = "67890", ExpirationDate = DateTime.Now.AddDays(60),
                PurchaseDate = DateTime.Now, Type = ProductType.Not_Selected
            },
            new Product
            {
                ProductId = 3, SerialNumber = "", ExpirationDate = DateTime.Now.AddDays(60),
                PurchaseDate = DateTime.Now, Type = ProductType.Monitor
            },
            
        };

        private static DbContextOptions<ApplicationDbContext> CreateNewOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }


        [Fact]
        public async Task GetAllProducts_ReturnsProducts()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                ProductRepository productRepository = new ProductRepository(context);

                await context.Products.AddRangeAsync(_products);
                await context.SaveChangesAsync();

                (IEnumerable<object> result, int totalCount) = await productRepository.GetAllProducts("", ProductType.Not_Selected, 1, 5);

                Assert.NotNull(result);
                Assert.NotEmpty(result);
                Assert.Equal(_products.Count, totalCount);
            }
        }

        [Fact]
        public async Task GetAllProducts_WithFilters_ReturnsFilteredProducts()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                ProductRepository productRepository = new ProductRepository(context);

                await context.Products.AddRangeAsync(_products);
                await context.SaveChangesAsync();

                (IEnumerable<object> result, int totalCount) = await productRepository.GetAllProducts("123", ProductType.Laptop, 1, 10);

                IEnumerable<ProductResponse> products = (IEnumerable<ProductResponse>)result;
                ProductResponse productResponse = products.First();
                
                Assert.NotNull(result);
                Assert.Single(result);
                Assert.Equal(1, totalCount);
                Assert.Equal("12345", productResponse.SerialNumber);
                Assert.Equal(ProductType.Laptop.ToString(), productResponse.Type);
            }
        }
        
        [Fact]
        public void GetProductTypeStrings_ShouldReturnCorrectList()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                ProductRepository yourClassInstance = new ProductRepository(context);
                List<string> result = yourClassInstance.GetProductTypeStrings();

                Assert.NotNull(result);
                Assert.Equal(3, result.Count);

                Assert.Contains("Monitor", result);
                Assert.Contains("Laptop", result);
                Assert.Contains("Stoel", result);
                Assert.DoesNotContain("UndefinedType", result);
            }
        }
    
        [Fact]
        public async Task GetProductById_ExistingId_ReturnsProduct()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                ProductRepository productRepository = new ProductRepository(context);

                await context.Products.AddAsync(_products.First());
                await context.SaveChangesAsync();
                
                Product result = await productRepository.GetProductById(1);

                Assert.NotNull(result);
                Assert.Equal("12345", result.SerialNumber);
                Assert.Equal(ProductType.Laptop, result.Type);
            }
        }

        [Fact]
        public async Task GetProductById_NonexistentId_ReturnsNull()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                ProductRepository productRepository = new ProductRepository(context);
                Product result = await productRepository.GetProductById(1);
                Assert.Null(result);
            }
        }
        
        [Fact]
        public async Task AddProduct_ValidProduct_ShouldAddToDatabase()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                ProductRepository productRepository = new ProductRepository(context);
                
                await productRepository.AddProduct(_products.First());
                Product addedProduct = await context.Products.FindAsync(_products[0].ProductId);
                
                Assert.NotNull(addedProduct);
                Assert.Equal("12345", addedProduct.SerialNumber);
                Assert.Equal(ProductType.Laptop, addedProduct.Type);
            }
        }

        [Fact]
        public async Task AddProduct_EmptySerialNumber_ShouldThrowException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                ProductRepository productRepository = new ProductRepository(context);
                await Assert.ThrowsAsync<InputValidationException>(() => productRepository.AddProduct(_products[2]));
            }
        }

        [Fact]
        public async Task AddProduct_NotSelectedType_ShouldThrowException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                ProductRepository productRepository = new ProductRepository(context);
                await Assert.ThrowsAsync<InputValidationException>(() => productRepository.AddProduct(_products[1]));
            }
        }
        
        [Fact]
        public async Task PutProduct_ExistingProduct_ShouldUpdateInDatabase()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                ProductRepository productRepository = new ProductRepository(context);

                Product product = new Product
                {
                    SerialNumber = "12345",
                    ExpirationDate = DateTime.Now.AddDays(30),
                    PurchaseDate = DateTime.Now,
                    Type = ProductType.Laptop
                };

                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();

                product.SerialNumber = "67890";
                product.Type = ProductType.Monitor;

                await productRepository.PutProduct(product);

                Product updatedProductInDatabase = await context.Products.FindAsync(product.ProductId);
                Assert.NotNull(updatedProductInDatabase);
                Assert.Equal("67890", updatedProductInDatabase.SerialNumber);
                Assert.Equal(ProductType.Monitor, updatedProductInDatabase.Type);
            }
        }

        [Fact]
        public async Task PutProduct_NonexistentProduct_ShouldNotUpdateInDatabase()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                ProductRepository productRepository = new ProductRepository(context);

                Product nonexistentProduct = new Product
                {
                    ProductId = 999, 
                    SerialNumber = "12345",
                    ExpirationDate = DateTime.Now.AddDays(30),
                    PurchaseDate = DateTime.Now,
                    Type = ProductType.Laptop
                };

                NotFoundException actualException =
                    await Assert.ThrowsAsync<NotFoundException>(() =>
                        productRepository.PutProduct(nonexistentProduct));
                Assert.NotNull(actualException);
            }
        }
        
        [Fact]
        public async Task DeleteProduct_ExistingProduct_ShouldRemoveFromDatabase()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                ProductRepository productRepository = new ProductRepository(context);
                
                await context.Products.AddAsync(_products.First());
                await context.SaveChangesAsync();

                await productRepository.DeleteProduct(_products.First().ProductId);

                Product deletedProduct = await context.Products.FindAsync(_products.First().ProductId);
                Assert.Null(deletedProduct);
            }
        }

        [Fact]
        public async Task DeleteProduct_NonexistentProduct_ShouldThrowNotFoundException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                ProductRepository productRepository = new ProductRepository(context);
                await Assert.ThrowsAsync<NotFoundException>(() => productRepository.DeleteProduct(999));
            }
        }
    }
}