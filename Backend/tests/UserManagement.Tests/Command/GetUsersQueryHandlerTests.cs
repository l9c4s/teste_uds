using AutoMapper;
using FluentAssertions;
using Moq;
using UserManagement.Application.DTOs.Users;
using UserManagement.Application.Handlers.Queries;
using UserManagement.Application.Handlers.Queries.User;
using UserManagement.Application.Queries;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;
using Xunit;

namespace UserManagement.Tests.Application;

public class GetUsersQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetUsersQueryHandler _handler;

    public GetUsersQueryHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetUsersQueryHandler(_userRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithUsers_ShouldReturnMappedUserDtos()
    {
        // Arrange
        var users = new List<User>
        {
            new()
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@test.com",
                CreatedAt = DateTime.UtcNow,
                UserAccessLevels = new List<UserAccessLevel>
                {
                    new() { IsActive = true, AccessLevel = new AccessLevel { Name = "Administrator" } }
                }
            },
            new()
            {
                Id = 2,
                Name = "Jane Smith",
                Email = "jane@test.com",
                CreatedAt = DateTime.UtcNow,
                UserAccessLevels = new List<UserAccessLevel>
                {
                    new() { IsActive = true, AccessLevel = new AccessLevel { Name = "User" } }
                }
            }
        };

        var expectedUserDtos = new List<UserDto>
        {
            new(1, "John Doe", "john@test.com", DateTime.UtcNow, null, "Administrator"),
            new(2, "Jane Smith", "jane@test.com", DateTime.UtcNow, null, "User")
        };

        _userRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(users);
        _mapperMock.Setup(x => x.Map<IEnumerable<UserDto>>(users)).Returns(expectedUserDtos);

        var query = new GetUsersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedUserDtos);

        // Verify calls
        _userRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        _mapperMock.Verify(x => x.Map<IEnumerable<UserDto>>(users), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNoUsers_ShouldReturnEmptyList()
    {
        // Arrange
        var emptyUsers = new List<User>();
        var emptyUserDtos = new List<UserDto>();

        _userRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(emptyUsers);
        _mapperMock.Setup(x => x.Map<IEnumerable<UserDto>>(emptyUsers)).Returns(emptyUserDtos);

        var query = new GetUsersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        // Verify calls
        _userRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        _mapperMock.Verify(x => x.Map<IEnumerable<UserDto>>(emptyUsers), Times.Once);
    }

    [Fact]
    public async Task Handle_WithUsersWithoutAccessLevels_ShouldReturnUsersWithUnknownLevel()
    {
        // Arrange
        var users = new List<User>
        {
            new()
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@test.com",
                CreatedAt = DateTime.UtcNow,
                UserAccessLevels = new List<UserAccessLevel>() // Empty access levels
            }
        };

        var expectedUserDtos = new List<UserDto>
        {
            new(1, "John Doe", "john@test.com", DateTime.UtcNow, null, "Unknown")
        };

        _userRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(users);
        _mapperMock.Setup(x => x.Map<IEnumerable<UserDto>>(users)).Returns(expectedUserDtos);

        var query = new GetUsersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().AccessLevel.Should().Be("Unknown");
    }

    [Fact]
    public async Task Handle_WithUsersWithInactiveAccessLevels_ShouldReturnUsersWithUnknownLevel()
    {
        // Arrange
        var users = new List<User>
        {
            new()
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@test.com",
                CreatedAt = DateTime.UtcNow,
                UserAccessLevels = new List<UserAccessLevel>
                {
                    new() { IsActive = false, AccessLevel = new AccessLevel { Name = "Administrator" } } // Inactive
                }
            }
        };

        var expectedUserDtos = new List<UserDto>
        {
            new(1, "John Doe", "john@test.com", DateTime.UtcNow, null, "Unknown")
        };

        _userRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(users);
        _mapperMock.Setup(x => x.Map<IEnumerable<UserDto>>(users)).Returns(expectedUserDtos);

        var query = new GetUsersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().AccessLevel.Should().Be("Unknown");
    }
}
