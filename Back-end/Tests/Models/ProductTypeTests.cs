using FluentAssertions;
using PL.Models;

namespace Tests.Models
{

    public class ProductTypeTests
    {
        [Fact]
        public void Enum_Values_Should_Match_Expected()
        {
            var expectedValues = new[]
            {
                ProductType.Not_Selected,
                ProductType.Laptop,
                ProductType.Monitor,
                ProductType.Stoel
            };

            var enumValues = Enum.GetValues(typeof(ProductType));
            enumValues.Should().BeEquivalentTo(expectedValues);
        }

        [Theory]
        [InlineData(ProductType.Not_Selected)]
        [InlineData(ProductType.Laptop)]
        [InlineData(ProductType.Monitor)]
        [InlineData(ProductType.Stoel)]
        public void Enum_Has_Valid_Names(ProductType value)
        {
            value.ToString().Should().NotBeNullOrEmpty();
        }
    }
}