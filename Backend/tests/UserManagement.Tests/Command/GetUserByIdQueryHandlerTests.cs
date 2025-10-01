using AutoMapper;
using FluentAssertions;
using Moq;
using UserManagement.Application.DTOs.Users;
using UserManagement.Application.Handlers.Queries.User;
using UserManagement.Application.Queries;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;
using Xunit;

namespace UserManagement.Tests.Application;

public class GetUserByIdQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetUserByIdQueryHandler _handler;

    public GetUserByIdQueryHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetUserByIdQueryHandler(_userRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingUser_ShouldReturnMappedUserDto()
    {
        // Arrange
        var userId = 1;
        var user = new User
        {
            Id = userId,
            Name = "John Doe",
            Email = "john@test.com",
            CreatedAt = DateTime.UtcNow,
            UserAccessLevels = new List<UserAccessLevel>
            {
                new() { IsActive = true, AccessLevel = new AccessLevel { Name = "Administrator" } }
            }
        };

        var expectedUserDto = new UserDto(
            userId, 
            "John Doe", 
            "john@test.com", 
            DateTime.UtcNow, 
            null, 
            "Administrator"
        );

        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);
        _mapperMock.Setup(x => x.Map<UserDto>(user)).Returns(expectedUserDto);

        var query = new GetUserByIdQuery(userId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedUserDto);

        // Verify calls
        _userRepositoryMock.Verify(x => x.GetByIdAsync(userId), Times.Once);
        _mapperMock.Verify(x => x.Map<UserDto>(user), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingUser_ShouldReturnNull()
    {
        // Arrange
        var userId = 999;

        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync((User?)null);

        var query = new GetUserByIdQuery(userId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();

        // Verify calls
        _userRepositoryMock.Verify(x => x.GetByIdAsync(userId), Times.Once);
        _mapperMock.Verify(x => x.Map<UserDto>(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithUserWithoutAccessLevel_ShouldReturnUserWithUnknownLevel()
    {
        // Arrange
        var userId = 1;
        var user = new User
        {
            Id = userId,
            Name = "John Doe",
            Email = "john@test.com",
            CreatedAt = DateTime.UtcNow,
            UserAccessLevels = new List<UserAccessLevel>() // No access levels
        };

        var expectedUserDto = new UserDto(
            userId, 
            "John Doe", 
            "john@test.com", 
            DateTime.UtcNow, 
            null, 
            "Unknown"
        );

        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);
        _mapperMock.Setup(x => x.Map<UserDto>(user)).Returns(expectedUserDto);

        var query = new GetUserByIdQuery(userId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.AccessLevel.Should().Be("Unknown");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Handle_WithInvalidUserId_ShouldReturnNull(int invalidUserId)
    {
        // Arrange
        _userRepositoryMock.Setup(x => x.GetByIdAsync(invalidUserId)).ReturnsAsync((User?)null);

        var query = new GetUserByIdQuery(invalidUserId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();

        // Verify calls
        _userRepositoryMock.Verify(x => x.GetByIdAsync(invalidUserId), Times.Once);
    }

    [Fact]
    public async Task Handle_WithUserWithMultipleAccessLevels_ShouldReturnActiveAccessLevel()
    {
        // Arrange
        var userId = 1;
        var user = new User
        {
            Id = userId,
            Name = "John Doe",
            Email = "john@test.com",
            CreatedAt = DateTime.UtcNow,
            UserAccessLevels = new List<UserAccessLevel>
            {
                new() { IsActive = false, AccessLevel = new AccessLevel { Name = "Administrator" } }, // Inactive
                new() { IsActive = true, AccessLevel = new AccessLevel { Name = "Manager" } }, // Active
                new() { IsActive = false, AccessLevel = new AccessLevel { Name = "User" } } // Inactive
            }
        };

        var expectedUserDto = new UserDto(
            userId, 
            "John Doe", 
            "john@test.com", 
            DateTime.UtcNow, 
            null, 
            "Manager" // Should return the active one
        );

        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);
        _mapperMock.Setup(x => x.Map<UserDto>(user)).Returns(expectedUserDto);

        var query = new GetUserByIdQuery(userId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.AccessLevel.Should().Be("Manager");
    }
}