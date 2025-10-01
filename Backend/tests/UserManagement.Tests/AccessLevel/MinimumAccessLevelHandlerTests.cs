using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UserManagement.Api.Config;
using UserManagement.Domain.Enums;
using Xunit;

namespace UserManagement.Tests.Api;

public class MinimumAccessLevelHandlerTests
{
    private readonly MinimumAccessLevelHandler _handler;

    public MinimumAccessLevelHandlerTests()
    {
        _handler = new MinimumAccessLevelHandler();
    }

    [Theory]
    [InlineData("Administrator", AccessLevel.Administrator, true)]
    [InlineData("Administrator", AccessLevel.Manager, true)]
    [InlineData("Administrator", AccessLevel.CommonUser, true)]
    [InlineData("Administrator", AccessLevel.Viewer, true)]
    [InlineData("Manager", AccessLevel.Administrator, false)]
    [InlineData("Manager", AccessLevel.Manager, true)]
    [InlineData("Manager", AccessLevel.CommonUser, true)]
    [InlineData("Manager", AccessLevel.Viewer, true)]
    [InlineData("User", AccessLevel.Administrator, false)]
    [InlineData("User", AccessLevel.Manager, false)]
    [InlineData("User", AccessLevel.CommonUser, true)]
    [InlineData("User", AccessLevel.Viewer, true)]
    [InlineData("Viewer", AccessLevel.Administrator, false)]
    [InlineData("Viewer", AccessLevel.Manager, false)]
    [InlineData("Viewer", AccessLevel.CommonUser, false)]
    [InlineData("Viewer", AccessLevel.Viewer, true)]
    public async Task HandleRequirementAsync_ShouldRespectHierarchy(
        string userAccessLevel, 
        AccessLevel requiredLevel, 
        bool shouldSucceed)
    {
        // Arrange
        var claims = new[]
        {
            new Claim("AccessLevel", userAccessLevel)
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        
        var context = new AuthorizationHandlerContext(
            new[] { new MinimumAccessLevelRequirement(requiredLevel) },
            principal,
            null);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        if (shouldSucceed)
        {
            context.HasSucceeded.Should().BeTrue();
        }
        else
        {
            context.HasSucceeded.Should().BeFalse();
        }
    }

    [Fact]
    public async Task HandleRequirementAsync_WithoutAccessLevelClaim_ShouldFail()
    {
        // Arrange
        var identity = new ClaimsIdentity(new Claim[0], "Test");
        var principal = new ClaimsPrincipal(identity);
        
        var context = new AuthorizationHandlerContext(
            new[] { new MinimumAccessLevelRequirement(AccessLevel.Viewer) },
            principal,
            null);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeFalse();
    }
}