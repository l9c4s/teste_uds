using AutoMapper;
using FluentAssertions;
using Moq;
using UserManagement.Application.Commands;
using UserManagement.Application.DTOs.Users;
using UserManagement.Application.Handlers.Commands.Auth;
using UserManagement.Application.Interfaces.Services;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Exceptions.Users;
using UserManagement.Domain.Interfaces;
using Xunit;

namespace UserManagement.Tests.Application;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _mapperMock = new Mock<IMapper>();
        
        _handler = new LoginCommandHandler(
            _userRepositoryMock.Object,
            _passwordServiceMock.Object,
            _tokenServiceMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCredentials_ShouldReturnAuthResponse()
    {
        // Arrange
        var email = "test@test.com";
        var password = "password123";
        var passwordHash = "hashed_password";
        var token = "jwt_token";

        var user = new User
        {
            Id = 1,
            Name = "Test User",
            Email = email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow,
            UserAccessLevels = new List<UserAccessLevel>
            {
                new()
                {
                    IsActive = true,
                    AccessLevel = new AccessLevel { Name = "Administrator" }
                }
            }
        };

            var expectedUserDto = new UserDto(
            user.Id,
            user.Name,
            user.Email,
            user.CreatedAt,
            user.UpdatedAt,
            "Administrator"
        );
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync(user);
        
        _passwordServiceMock.Setup(x => x.VerifyPassword(password, passwordHash))
            .Returns(true);
        
        _tokenServiceMock.Setup(x => x.GenerateToken(user))
            .Returns(token);
        
        _mapperMock.Setup(x => x.Map<UserDto>(user))
            .Returns(expectedUserDto);


        var command = new LoginCommand(email, password);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().Be(token);
        result.User.Should().NotBeNull();
        result.User.Id.Should().Be(user.Id);
        result.User.Name.Should().Be(user.Name);
        result.User.Email.Should().Be(user.Email);
        result.User.AccessLevel.Should().Be("Administrator");
    }

    [Fact]
    public async Task Handle_WithInvalidEmail_ShouldThrowInvalidCredentialsException()
    {
        // Arrange
        var email = "nonexistent@test.com";
        var password = "password123";

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync((User?)null);

        var command = new LoginCommand(email, password);

        // Act & Assert
        await _handler.Invoking(x => x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<InvalidCredentialsException>();
    }

    [Fact]
    public async Task Handle_WithInvalidPassword_ShouldThrowInvalidCredentialsException()
    {
        // Arrange
        var email = "test@test.com";
        var password = "wrongpassword";
        var passwordHash = "hashed_password";

        var user = new User
        {
            Email = email,
            PasswordHash = passwordHash
        };

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync(user);
        
        _passwordServiceMock.Setup(x => x.VerifyPassword(password, passwordHash))
            .Returns(false);

        var command = new LoginCommand(email, password);

        // Act & Assert
        await _handler.Invoking(x => x.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<InvalidCredentialsException>();
    }
}