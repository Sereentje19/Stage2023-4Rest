using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using PL.Models;

namespace Tests.Models
{

    public class EmployeeTests
    {
        [Fact]
        public void Employee_HasKeyAttributeOnEmployeeId()
        {
            var employeeIdProperty = typeof(Employee).GetProperty(nameof(Employee.EmployeeId));
            employeeIdProperty.Should().BeDecoratedWith<KeyAttribute>();
        }

        [Fact]
        public void Employee_Properties_NotNull_ShouldBeDecoratedWithRequiredAttribute()
        {
            var emailProperty = typeof(Employee).GetProperty(nameof(Employee.Email));
            var nameProperty = typeof(Employee).GetProperty(nameof(Employee.Name));

            emailProperty.Should().NotBeNull().And.BeDecoratedWith<RequiredAttribute>();
            nameProperty.Should().NotBeNull().And.BeDecoratedWith<RequiredAttribute>();
        }

        [Fact]
        public void Employee_Email_Should_Have_EmailAddressAttribute()
        {
            var emailProperty = typeof(Employee).GetProperty(nameof(Employee.Email));
            emailProperty.PropertyType.Should().Be(typeof(string));
            emailProperty.Should().BeDecoratedWith<EmailAddressAttribute>();

        }

        [Fact]
        public void Employee_IsArchived_ShouldNot_Have_RequiredAttribute()
        {
            var isArchivedProperty = typeof(Employee).GetProperty(nameof(Employee.IsArchived));
            isArchivedProperty.Should().NotBeDecoratedWith<RequiredAttribute>();
        }
    }
}