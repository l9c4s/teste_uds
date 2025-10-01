
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using UserManagement.Application.Commands;
using UserManagement.Application.Handlers.Commands.User;
using UserManagement.Application.Interfaces.Services;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Exceptions.Users;
using UserManagement.Domain.Interfaces;


namespace UserManagement.Tests.Application;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<IAccessLevelRepository> _accessLevelRepositoryMock;
    private readonly Mock<IUserAccessLevelRepository> _userAccessLevelRepositoryMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _accessLevelRepositoryMock = new Mock<IAccessLevelRepository>();
        _userAccessLevelRepositoryMock = new Mock<IUserAccessLevelRepository>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        _handler = new CreateUserCommandHandler(
            _userRepositoryMock.Object,
            _passwordServiceMock.Object,
            _accessLevelRepositoryMock.Object,
            _userAccessLevelRepositoryMock.Object,
            _httpContextAccessorMock.Object);
    }

    [Fact]
    public async Task Handle_AsAdmin_WithValidAccessLevelId_ShouldCreateUserWithSpecifiedLevel()
    {
        // Arrange
        var command = new CreateUserCommand("John Doe", "john@test.com", "password123", 1);
        var hashedPassword = "hashed_password";

        var accessLevel = new AccessLevel { Id = 1, Name = "Administrator" };
        var createdUser = new User
        {
            Id = 1,
            Name = command.Name,
            Email = command.Email,
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow,
        };

        // Setup admin user context
        SetupUserContext("Administrator");
        
        _userRepositoryMock.Setup(x => x.EmailExistsAsync(command.Email)).ReturnsAsync(false);
        _passwordServiceMock.Setup(x => x.HashPassword(command.Password)).Returns(hashedPassword);
        _accessLevelRepositoryMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(accessLevel);
        _userRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);
        _userAccessLevelRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<UserAccessLevel>()))
            .Returns((UserAccessLevel ual) => Task.FromResult(ual));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Email.Should().Be(command.Email);
        result.AccessLevel.Should().Be("Administrator");

        // Verify calls
        _userRepositoryMock.Verify(x => x.EmailExistsAsync(command.Email), Times.Once);
        _passwordServiceMock.Verify(x => x.HashPassword(command.Password), Times.Once);
        _accessLevelRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);
        _userRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Once);
        _userAccessLevelRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<UserAccessLevel>()), Times.Once);
    }

    [Fact]
    public async Task Handle_AsNonAdmin_ShouldCreateUserWithViewerLevel()
    {
        // Arrange
        var command = new CreateUserCommand("John Doe", "john@test.com", "password123", 1);
        var hashedPassword = "hashed_password";

        var viewerAccessLevel = new AccessLevel { Id = 4, Name = "Viewer" };
        var createdUser = new User
        {
            Id = 1,
            Name = command.Name,
            Email = command.Email,
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow
        };

        // Setup non-admin user context
        SetupUserContext("User");
        
        _userRepositoryMock.Setup(x => x.EmailExistsAsync(command.Email)).ReturnsAsync(false);
        _passwordServiceMock.Setup(x => x.HashPassword(command.Password)).Returns(hashedPassword);
        _accessLevelRepositoryMock.Setup(x => x.GetByIdAsync(4)).ReturnsAsync(viewerAccessLevel);
        _userRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);
        _userAccessLevelRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<UserAccessLevel>()))
                                            .Returns((UserAccessLevel ual) => Task.FromResult(ual));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(command.Name);
        result.Email.Should().Be(command.Email);
        result.AccessLevel.Should().Be("Viewer");

        // Verify that the specified AccessLevelId was ignored and Viewer was used
        _accessLevelRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Never); // Admin level not checked
        _accessLevelRepositoryMock.Verify(x => x.GetByIdAsync(4), Times.Once); // Viewer level checked
    }

    [Fact]
    public async Task Handle_WithExistingEmail_ShouldThrowEmailAlreadyExistsException()
    {
        // Arrange
        var command = new CreateUserCommand("John Doe", "existing@test.com", "password123", 1);

        _userRepositoryMock.Setup(x => x.EmailExistsAsync(command.Email)).ReturnsAsync(true);

        // Act & Assert
        await _handler.Invoking(x => x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<EmailAlreadyExistsException>()
            .WithMessage("*existing@test.com*");

        // Verify that only email check was called
        _userRepositoryMock.Verify(x => x.EmailExistsAsync(command.Email), Times.Once);
        _userRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_AsAdmin_WithInvalidAccessLevelId_ShouldThrowAccessLevelNotFoundException()
    {
        // Arrange
        var command = new CreateUserCommand("John Doe", "john@test.com", "password123", 999);

        SetupUserContext("Administrator");
        _userRepositoryMock.Setup(x => x.EmailExistsAsync(command.Email)).ReturnsAsync(false);
        _accessLevelRepositoryMock.Setup(x => x.GetByIdAsync(999)).ReturnsAsync((AccessLevel?)null);

        // Act & Assert
        await _handler.Invoking(x => x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<AccessLevelNotFoundException>();

        // Verify that access level was checked
        _accessLevelRepositoryMock.Verify(x => x.GetByIdAsync(999), Times.Once);
        _userRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithoutAccessLevelId_ShouldCreateUserWithViewerLevel()
    {
        // Arrange
        var command = new CreateUserCommand("John Doe", "john@test.com", "password123", 1); // NULL AccessLevelId
        var hashedPassword = "hashed_password";

        var viewerAccessLevel = new AccessLevel { Id = 4, Name = "Viewer" };
        var createdUser = new User
        {
            Id = 1,
            Name = command.Name,
            Email = command.Email,
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow
        };

        // Setup any user context (doesn't matter since AccessLevelId is null)
        SetupUserContext("Manager");
        
        _userRepositoryMock.Setup(x => x.EmailExistsAsync(command.Email)).ReturnsAsync(false);
        _passwordServiceMock.Setup(x => x.HashPassword(command.Password)).Returns(hashedPassword);
        _accessLevelRepositoryMock.Setup(x => x.GetByIdAsync(4)).ReturnsAsync(viewerAccessLevel);
        _userRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(createdUser);
        _userAccessLevelRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<UserAccessLevel>()))
                                            .Returns((UserAccessLevel ual) => Task.FromResult(ual));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.AccessLevel.Should().Be("Viewer");

        // Verify that Viewer level was used as default
        _accessLevelRepositoryMock.Verify(x => x.GetByIdAsync(4), Times.Once);
    }

    private void SetupUserContext(string accessLevel)
    {
        var claims = new List<Claim> { new("AccessLevel", accessLevel) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
    }
}