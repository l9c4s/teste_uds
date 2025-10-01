namespace UserManagement.Application.DTOs.Users;


public record CreateUserDto(
    string Name,
    string Email,
    string Password,
    int AccessLevel
);