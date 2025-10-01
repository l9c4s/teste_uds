using UserManagement.Application.DTOs.Users;


namespace UserManagement.Application.DTOs.Auth;

public record AuthResponseDto(
    string Token,
    UserDto User
);
