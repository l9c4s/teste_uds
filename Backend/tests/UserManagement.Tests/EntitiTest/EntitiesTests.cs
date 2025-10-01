using FluentAssertions;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Exceptions;
using Xunit;

namespace UserManagement.Tests.Domain;

public class EntitiesTests
{
    [Fact]
    public void User_Creation_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var name = "John Doe";
        var email = "john@example.com";
        var passwordHash = "hashed_password";

        // Act
        var user = new User
        {
            Name = name,
            Email = email,
            PasswordHash = passwordHash
        };

        // Assert
        user.Name.Should().Be(name);
        user.Email.Should().Be(email);
        user.PasswordHash.Should().Be(passwordHash);
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        user.UserAccessLevels.Should().NotBeNull();
        user.UserAccessLevels.Should().BeEmpty();
    }

    [Fact]
    public void User_Creation_WithoutExplicitCreatedAt_ShouldSetToUtcNow()
    {
        // Act
        var user = new User();

        // Assert
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        user.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void AccessLevel_Creation_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var name = "Administrator";
        var description = "Full system access";

        // Act
        var accessLevel = new AccessLevel
        {
            Name = name,
            Description = description
        };

        // Assert
        accessLevel.Name.Should().Be(name);
        accessLevel.Description.Should().Be(description);
        accessLevel.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void UserAccessLevel_Creation_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var userId = 1;
        var accessLevelId = 1;

        // Act
        var userAccessLevel = new UserAccessLevel
        {
            UserId = userId,
            AccessLevelId = accessLevelId,
            IsActive = true
        };

        // Assert
        userAccessLevel.UserId.Should().Be(userId);
        userAccessLevel.AccessLevelId.Should().Be(accessLevelId);
        userAccessLevel.IsActive.Should().BeTrue();
        userAccessLevel.AssignedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void DomainException_ShouldHaveCorrectMessage()
    {
        // Arrange
        var message = "Test domain exception";

        // Act
        var exception = new DomainException(message);

        // Assert
        exception.Message.Should().Be(message);
        exception.Should().BeOfType<DomainException>();
    }
}