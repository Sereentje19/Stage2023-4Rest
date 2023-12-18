using DAL.Data;
using DAL.Exceptions;
using DAL.Models;
using DAL.Models.Responses;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.Repositories
{
    public class ProductRepositoryTests
    {
        private readonly List<Product> _products = new()
        {
            new Product
            {
                ProductId = 1, SerialNumber = "12345", ExpirationDate = DateTime.Now.AddDays(30),
                PurchaseDate = DateTime.Now, Type = new ProductType() { Id = 1, Name = "Laptop" }
            },
            new Product
            {
                ProductId = 2, SerialNumber = "67890", ExpirationDate = DateTime.Now.AddDays(60),
                PurchaseDate = DateTime.Now, Type = new ProductType() { Id = 2, Name = "0" }
            },
            new Product
            {
                ProductId = 3, SerialNumber = "", ExpirationDate = DateTime.Now.AddDays(60),
                PurchaseDate = DateTime.Now, Type = new ProductType() { Id = 3, Name = "Laptop" }
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

                (IEnumerable<object> result, int totalCount) =
                    await productRepository.GetAllProducts("", 1, 5, "0");

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

                (IEnumerable<object> result, int totalCount) =
                    await productRepository.GetAllProducts("123", 1, 10, "laptop");

                IEnumerable<ProductResponse> products = (IEnumerable<ProductResponse>)result;
                ProductResponse productResponse = products.First();

                Assert.NotNull(result); 
                Assert.Single(result);
                Assert.Equal(1, totalCount);
                Assert.Equal("12345", productResponse.SerialNumber);
                Assert.Equal("Laptop", productResponse.Type.Name);
            }
        }

        [Fact]
        public async Task GetProductTypes_ShouldReturnListOfProductTypes()
        {

            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                List<ProductType> productTypes = new List<ProductType>
                {
                    new ProductType { Id = 1, Name = "Type1" },
                    new ProductType { Id = 2, Name = "Type2" },
                };

                context.ProductTypes.AddRange(productTypes);
                await context.SaveChangesAsync();

                ProductRepository yourService = new ProductRepository(context);

                IEnumerable<ProductType> result = await yourService.GetProductTypes();

                Assert.NotNull(result);
                Assert.IsType<List<ProductType>>(result);
                Assert.Equal(2, result.Count()); 
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
                    Assert.Equal("Laptop", result.Type.Name);
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
                    Assert.Equal("Laptop", addedProduct.Type.Name);
                }
            }

            [Fact]
            public async Task AddProduct_EmptySerialNumber_ShouldThrowException()
            {
                using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
                {
                    ProductRepository productRepository = new ProductRepository(context);
                    await Assert.ThrowsAsync<InputValidationException>(() =>
                        productRepository.AddProduct(_products[2]));
                }
            }

            [Fact]
            public async Task AddProduct_NotSelectedType_ShouldThrowException()
            {
                using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
                {
                    ProductRepository productRepository = new ProductRepository(context);
                    await Assert.ThrowsAsync<InputValidationException>(() =>
                        productRepository.AddProduct(_products[1]));
                }
            }

            [Fact]
            public async Task PutProduct_ShouldUpdateProduct()
            {
                using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
                {
                    ProductRepository productRepository = new ProductRepository(context);
                    Product existingProduct = new Product
                    {
                        ProductId = 1,
                        SerialNumber = "12345",
                        Type = new ProductType { Id = 1, Name = "Laptop" },
                    };

                    await context.AddAsync(existingProduct);
                    await context.SaveChangesAsync();

                    Product updatedProduct = new Product
                    {
                        ProductId = 1,
                        SerialNumber = "67890",
                        Type = new ProductType { Id = 2, Name = "Desktop" },
                    };

                    await productRepository.PutProduct(updatedProduct);

                    Product result = await context.Products.FindAsync(1);
                    Assert.NotNull(result);
                    Assert.Equal(updatedProduct.SerialNumber, result.SerialNumber);
                    Assert.Equal(updatedProduct.Type.Id, result.Type.Id);
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
                        Type = new ProductType() { Id = 1, Name = "Laptop" }
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