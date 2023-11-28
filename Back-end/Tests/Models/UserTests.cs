using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using PL.Models;

namespace Tests.Models
{

    public class UserTests
    {
        [Fact]
        public void UserId_Should_Have_KeyAttribute()
        {
            // Arrange
            var userIdProperty = typeof(User).GetProperty(nameof(User.UserId));

            // Act & Assert
            userIdProperty.Should().BeDecoratedWith<KeyAttribute>();
        }

        [Fact]
        public void Email_Should_Have_RequiredAttribute()
        {
            // Arrange
            var emailProperty = typeof(User).GetProperty(nameof(User.Email));

            // Act & Assert
            emailProperty.Should().BeDecoratedWith<RequiredAttribute>();
        }

        [Fact]
        public void Name_Should_Have_RequiredAttribute()
        {
            // Arrange
            var nameProperty = typeof(User).GetProperty(nameof(User.Name));

            // Act & Assert
            nameProperty.Should().BeDecoratedWith<RequiredAttribute>();
        }

        [Fact]
        public void PasswordHash_Should_Not_Have_RequiredAttribute()
        {
            // Arrange
            var passwordHashProperty = typeof(User).GetProperty(nameof(User.PasswordHash));

            // Act & Assert
            passwordHashProperty.Should().NotBeDecoratedWith<RequiredAttribute>();
        }

        [Fact]
        public void PasswordSalt_Should_Not_Have_RequiredAttribute()
        {
            // Arrange
            var passwordSaltProperty = typeof(User).GetProperty(nameof(User.PasswordSalt));

            // Act & Assert
            passwordSaltProperty.Should().NotBeDecoratedWith<RequiredAttribute>();
        }
    }
}