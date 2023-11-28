using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using FluentAssertions;
using PL.Models;

namespace Tests.Models
{

    public class ProductTests
    {
        [Fact]
        public void ProductId_Should_Have_KeyAttribute()
        {
            var productIdProperty = typeof(Product).GetProperty(nameof(Product.ProductId));
            productIdProperty.Should().BeDecoratedWith<KeyAttribute>();
        }

        [Fact]
        public void PurchaseDate_Should_Have_RequiredAttribute()
        {
            var purchaseDateProperty = typeof(Product).GetProperty(nameof(Product.PurchaseDate));
            purchaseDateProperty.Should().BeDecoratedWith<RequiredAttribute>();
        }

        [Fact]
        public void Type_Should_Have_ColumnAttribute_With_TypeName()
        {
            var typeProperty = typeof(Product).GetProperty(nameof(Product.Type));

            var columnAttribute = typeProperty.GetCustomAttribute<ColumnAttribute>();
            columnAttribute.Should().NotBeNull();
            columnAttribute.TypeName.Should().Be("nvarchar(24)");
        }

        [Fact]
        public void SerialNumber_Should_Have_RequiredAttribute()
        {
            var serialNumberProperty = typeof(Product).GetProperty(nameof(Product.SerialNumber));
            serialNumberProperty.Should().BeDecoratedWith<RequiredAttribute>();
        }
    }
}