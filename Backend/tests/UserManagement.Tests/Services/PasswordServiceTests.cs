using FluentAssertions;
using UserManagement.Infrastructure.Services;
using Xunit;
using Xunit.Abstractions;

namespace UserManagement.Tests.Services;

public class PasswordServiceTests
{

    // QUE FIQUE BEM CLARO QUE CRIEI ESTES TESTES APENAS PARA GERAR HASH PARA O INSERT INICIAL DO BANCO DE DADOS 
    // SE LEU ISSO ME DA UM ALO NO 11 940654903 whatsapp

    private readonly ITestOutputHelper _output;
    private readonly PasswordService _passwordService;

    public PasswordServiceTests(ITestOutputHelper output)
    {
        _output = output;
        _passwordService = new PasswordService();
    }

    [Fact]
    public void HashPassword_ShouldGenerateValidHash()
    {
        // Arrange
        var password = "admin123";

        // Act
        var hashedPassword = _passwordService.HashPassword(password);

        // Assert
        hashedPassword.Should().NotBeNullOrEmpty();
        hashedPassword.Should().NotBe(password);
        hashedPassword.Should().StartWith("$2a$"); // BCrypt hash format
        hashedPassword.Length.Should().Be(60); // BCrypt hash length

        _output.WriteLine($"Original Password: {password}");
        _output.WriteLine($"Hashed Password: {hashedPassword}");
    }

    [Fact]
    public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        var password = "mySecurePassword123";
        var hashedPassword = _passwordService.HashPassword(password);

        // Act
        var isValid = _passwordService.VerifyPassword(password, hashedPassword);

        // Assert
        isValid.Should().BeTrue();

        _output.WriteLine($"Password verification successful for: {password}");
    }

    [Fact]
    public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        var correctPassword = "mySecurePassword123";
        var incorrectPassword = "wrongPassword456";
        var hashedPassword = _passwordService.HashPassword(correctPassword);

        // Act
        var isValid = _passwordService.VerifyPassword(incorrectPassword, hashedPassword);

        // Assert
        isValid.Should().BeFalse();

        _output.WriteLine($"Password verification correctly failed for wrong password: {incorrectPassword}");
    }

    [Theory]
    [InlineData("admin123")]
    [InlineData("user@123")]
    [InlineData("P@ssw0rd!")]
    [InlineData("123456")]
    [InlineData("SuperSecurePassword2024")]
    public void HashPassword_WithDifferentPasswords_ShouldGenerateUniqueHashes(string password)
    {
        // Act
        var hash1 = _passwordService.HashPassword(password);
        var hash2 = _passwordService.HashPassword(password);

        // Assert
        hash1.Should().NotBeNullOrEmpty();
        hash2.Should().NotBeNullOrEmpty();
        hash1.Should().NotBe(hash2); // BCrypt generates different salts

        // Both hashes should verify the same password
        _passwordService.VerifyPassword(password, hash1).Should().BeTrue();
        _passwordService.VerifyPassword(password, hash2).Should().BeTrue();

        _output.WriteLine($"Password: {password}");
        _output.WriteLine($"Hash 1: {hash1}");
        _output.WriteLine($"Hash 2: {hash2}");
        _output.WriteLine("---");
    }

    [Fact]
    public void HashPassword_WithEmptyPassword_ShouldHandleGracefully()
    {
        // Arrange
        var emptyPassword = "";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _passwordService.HashPassword(emptyPassword));

        exception.Message.Should().Contain("Password cannot be null or empty");

        _output.WriteLine("Empty password correctly rejected");
    }

    [Fact]
    public void HashPassword_WithNullPassword_ShouldHandleGracefully()
    {
        // Arrange
        string? nullPassword = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _passwordService.HashPassword(nullPassword!));

        exception.Message.Should().Contain("Password cannot be null or empty");

        _output.WriteLine("Null password correctly rejected");
    }

    [Fact]
    public void GenerateHashForAdminUser_ShouldCreateValidHash()
    {
        // Arrange - Esta é a senha do usuário admin no banco
        var adminPassword = "admin123";

        // Act
        var adminHash = _passwordService.HashPassword(adminPassword);

        // Assert
        adminHash.Should().NotBeNullOrEmpty();
        _passwordService.VerifyPassword(adminPassword, adminHash).Should().BeTrue();

        _output.WriteLine("=== HASH PARA USUÁRIO ADMIN ===");
        _output.WriteLine($"Password: {adminPassword}");
        _output.WriteLine($"Hash: {adminHash}");
        _output.WriteLine("Este hash pode ser usado para atualizar o banco de dados se necessário.");
    }

    [Theory]
    [InlineData("user123", "User")]
    [InlineData("manager456", "Manager")]
    [InlineData("viewer789", "Viewer")]
    public void GenerateHashesForTestUsers_ShouldCreateValidHashes(string password, string userType)
    {
        // Act
        var hash = _passwordService.HashPassword(password);

        // Assert
        hash.Should().NotBeNullOrEmpty();
        _passwordService.VerifyPassword(password, hash).Should().BeTrue();

        _output.WriteLine($"=== HASH PARA {userType.ToUpper()} ===");
        _output.WriteLine($"Password: {password}");
        _output.WriteLine($"Hash: {hash}");
        _output.WriteLine("---");
    }
}