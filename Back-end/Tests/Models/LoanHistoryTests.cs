using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentAssertions;
using PL.Models;

namespace Tests.Models
{

    public class LoanHistoryTests
    {
        [Fact]
        public void LoanHistoryId_Should_Have_KeyAttribute()
        {
            var loanHistoryIdProperty = typeof(LoanHistory).GetProperty(nameof(LoanHistory.LoanHistoryId));
            loanHistoryIdProperty.Should().BeDecoratedWith<KeyAttribute>();
        }

        [Fact]
        public void Employee_Should_Have_ForeignKeyAttributeForEmployeeId()
        {
            var employeeProperty = typeof(LoanHistory).GetProperty(nameof(LoanHistory.Employee));

            employeeProperty.Should()
                .BeDecoratedWith<ForeignKeyAttribute>()
                .Which.Name.Should().Be("EmployeeId");
        }

        [Fact]
        public void Product_Should_Have_ForeignKeyAttributeForProductId()
        {
            var productProperty = typeof(LoanHistory).GetProperty(nameof(LoanHistory.Product));

            productProperty.Should()
                .BeDecoratedWith<ForeignKeyAttribute>()
                .Which.Name.Should().Be("ProductId");
        }

        [Fact]
        public void LoanDate_Should_Have_RequiredAttribute()
        {
            var loanDateProperty = typeof(LoanHistory).GetProperty(nameof(LoanHistory.LoanDate));

            loanDateProperty.Should()
                .BeDecoratedWith<RequiredAttribute>();
        }
    }
}