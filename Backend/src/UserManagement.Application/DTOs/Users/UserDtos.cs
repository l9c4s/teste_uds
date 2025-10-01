using UserManagement.Application.DTOs.AcessLevels;

namespace UserManagement.Application.DTOs.Users;

public record UserDto(
    int Id,
    string Name,
    string Email,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string? AccessLevel
);