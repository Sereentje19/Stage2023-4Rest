using BLL.Services;
using DAL.Data;
using DAL.Exceptions;
using DAL.Interfaces;
using DAL.Models;
using DAL.Models.Dtos.Requests;
using DAL.Models.Dtos.Responses;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Repositories
{
    public class ProductRepositoryTests
    {
        private readonly List<Product> _products = new List<Product>
        {
            new Product
            {
                ProductId = 1, SerialNumber = "12345", ExpirationDate = DateTime.Now.AddDays(30),
                PurchaseDate = DateTime.Now, Type = new ProductType() { Id = 1, Name = "Laptop" }, IsDeleted = false
            },
            new Product
            {
                ProductId = 2, SerialNumber = "67890", ExpirationDate = DateTime.Now.AddDays(60),
                PurchaseDate = DateTime.Now, Type = new ProductType() { Id = 2, Name = "0" }, IsDeleted = false
            },
            new Product
            {
                ProductId = 3, SerialNumber = "", ExpirationDate = DateTime.Now.AddDays(60),
                PurchaseDate = DateTime.Now, Type = new ProductType() { Id = 3, Name = "Laptop" }, IsDeleted = false
            },
        };

        private static ProductRequestDto MapProductToDto(Product pro)
        {
            return new ProductRequestDto()
            {
                ProductId = pro.ProductId,
                SerialNumber = pro.SerialNumber,
                ExpirationDate = pro.ExpirationDate,
                PurchaseDate = pro.PurchaseDate,
                Type = pro.Type
            };
        }

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
        public async Task GetAllDeletedProducts_ShouldReturnPagedResult()
        {
            await using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                ProductRepository repository = new ProductRepository(context);
                (IEnumerable<object>, int) result =
                    await repository.GetAllDeletedProducts("someSearchField", 1, 1, "someDropdown");
                Assert.IsType<(IEnumerable<object>, int)>(result);

                (IEnumerable<object> returnedProducts, int totalCount) = result;
                Assert.All(returnedProducts, prod => Assert.True(((Product)prod).IsDeleted));
            }
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnPagedResult()
        {
            await using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                ProductRepository repository = new ProductRepository(context);
                (IEnumerable<object>, int) result =
                    await repository.GetAllProducts("someSearchField", 1, 1, "someDropdown");

                Assert.IsType<(IEnumerable<object>, int)>(result);

                (IEnumerable<object> returnedProducts, int totalCount) = result;
                Assert.All(returnedProducts, prod => Assert.False(((Product)prod).IsDeleted));
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

                IEnumerable<ProductType> result = await yourService.GetProductTypesAsync();

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

                Product result = await productRepository.GetProductByIdAsync(1);

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
                Product result = await productRepository.GetProductByIdAsync(1);
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task AddProduct_ValidProduct_ShouldAddToDatabase()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                ProductRepository productRepository = new ProductRepository(context);

                await productRepository.CreateProductAsync(_products.First());
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
                    productRepository.CreateProductAsync(_products[2]));
            }
        }

        [Fact]
        public async Task AddProduct_NotSelectedType_ShouldThrowException()
        {
            using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                ProductRepository productRepository = new ProductRepository(context);
                await Assert.ThrowsAsync<InputValidationException>(() =>
                    productRepository.CreateProductAsync(_products[1]));
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
                        productRepository.UpdateProductAsync(MapProductToDto(nonexistentProduct)));
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

                await productRepository.DeleteProductAsync(_products.First().ProductId);

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
                await Assert.ThrowsAsync<NotFoundException>(() => productRepository.DeleteProductAsync(999));
            }
        }

        [Fact]
        public async Task CreateProductAsync_TypeExists_ShouldSetProductType()
        {
            DbContextOptions<ApplicationDbContext> options = CreateNewOptions();
            await using (ApplicationDbContext context = new ApplicationDbContext(options))
            {
                ProductType productType = new ProductType { Name = "ExistingType" };
                await context.ProductTypes.AddAsync(productType);
                await context.SaveChangesAsync();
            }

            await using (ApplicationDbContext context = new ApplicationDbContext(options))
            {
                ProductRepository repository = new ProductRepository(context);
                Product product = new Product
                {
                    SerialNumber = "drftr",
                    ProductId = 1,
                    Type = new ProductType { Name = "ExistingType" }, 
                };

                await repository.CreateProductAsync(product);

                Product createdProduct = await context.Products.FindAsync(product.ProductId);
                Assert.NotNull(createdProduct);
                Assert.NotNull(createdProduct.Type);
                Assert.Equal("ExistingType", createdProduct.Type.Name);
            }
        }


        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateProduct()
        {
            await using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Product existingProduct = new Product
                {
                    ProductId = 1,
                    Type = new ProductType { Name = "ExistingType" },
                    SerialNumber = "xerftftre"
                };
                await context.Products.AddAsync(existingProduct);
                await context.SaveChangesAsync();

                ProductRepository repository = new ProductRepository(context);

                ProductRequestDto updatedProductRequest = new ProductRequestDto
                {
                    ProductId = 1,
                    Type = new ProductType { Name = "UpdatedType" }
                };

                await repository.UpdateProductAsync(updatedProductRequest);
                Product updatedProduct =
                    await context.Products.Include(p => p.Type).FirstOrDefaultAsync(p => p.ProductId == 1);

                Assert.NotNull(updatedProduct);
                Assert.Equal(updatedProductRequest.ProductId, updatedProduct.ProductId);
                Assert.Equal(updatedProductRequest.ProductId, updatedProduct.ProductId);
                Assert.Equal(updatedProductRequest.Type, updatedProduct.Type);
                Assert.Equal(updatedProductRequest.Type.Name, updatedProduct.Type.Name);
            }
        }

        [Fact]
        public async Task UpdateIsDeletedAsync_ShouldUpdateIsDeleted()
        {
            await using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                Product existingProduct = new Product
                {
                    ProductId = 1,
                    IsDeleted = false,
                    TimeDeleted = DateTime.MinValue
                };
                await context.Products.AddAsync(existingProduct);
                await context.SaveChangesAsync();

                ProductRepository repository = new ProductRepository(context);

                ProductRequestDto updateRequest = new ProductRequestDto
                {
                    ProductId = 1,
                    IsDeleted = true
                };

                await repository.UpdateIsDeletedAsync(updateRequest);

                Product updatedProduct = await context.Products.FirstOrDefaultAsync(p => p.ProductId == 1);
                Assert.NotNull(updatedProduct);
                Assert.Equal(updateRequest.IsDeleted, updatedProduct.IsDeleted);

                if (updateRequest.IsDeleted)
                {
                    Assert.Equal(DateTime.Today, updatedProduct.TimeDeleted.Date);
                }
                else
                {
                    Assert.Equal(DateTime.MinValue, updatedProduct.TimeDeleted);
                }
            }
        }

        [Fact]
        public async Task UpdateIsDeletedAsync_ProductNotFound_ShouldThrowNotFoundException()
        {
            await using (ApplicationDbContext context = new ApplicationDbContext(CreateNewOptions()))
            {
                ProductRepository repository = new ProductRepository(context);

                ProductRequestDto updateRequest = new ProductRequestDto
                {
                    ProductId = 1,
                    IsDeleted = true
                };

                await Assert.ThrowsAsync<NotFoundException>(() => repository.UpdateIsDeletedAsync(updateRequest));
            }
        }
    }
}