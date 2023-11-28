using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using FluentAssertions;
using PL.Models;

namespace Tests.Models
{

    public class PasswordResetCodeTests
    {
[Fact]
        public void ResetCodeId_Should_Have_KeyAttribute()
        {
            var resetCodeIdProperty = typeof(PasswordResetCode).GetProperty(nameof(PasswordResetCode.ResetCodeId));
            resetCodeIdProperty.Should().BeDecoratedWith<KeyAttribute>();
        }

        [Fact]
        public void UserId_Should_Have_ForeignKeyAttribute()
        {
            var userIdProperty = typeof(PasswordResetCode).GetProperty(nameof(PasswordResetCode.UserId));
            var foreignKeyAttribute = userIdProperty.GetCustomAttribute<ForeignKeyAttribute>();
    
            foreignKeyAttribute.Should().NotBeNull();
            foreignKeyAttribute!.Name.Should().Be("UserId");
        }

        [Fact]
        public void Code_Should_Have_RequiredAttribute()
        {
            var codeProperty = typeof(PasswordResetCode).GetProperty(nameof(PasswordResetCode.Code));
            codeProperty.Should().BeDecoratedWith<RequiredAttribute>();
        }

        [Fact]
        public void Properties_Should_Have_ExpectedDataTypes()
        {
            var resetCodeIdProperty = typeof(PasswordResetCode).GetProperty(nameof(PasswordResetCode.ResetCodeId));
            var userIdProperty = typeof(PasswordResetCode).GetProperty(nameof(PasswordResetCode.UserId));
            var codeProperty = typeof(PasswordResetCode).GetProperty(nameof(PasswordResetCode.Code));
            var expirationTimeProperty = typeof(PasswordResetCode).GetProperty(nameof(PasswordResetCode.ExpirationTime));

            resetCodeIdProperty.PropertyType.Should().Be(typeof(int));
            userIdProperty.PropertyType.Should().Be(typeof(int));
            codeProperty.PropertyType.Should().Be(typeof(string));
            expirationTimeProperty.PropertyType.Should().Be(typeof(DateTime));
        }
    }
}