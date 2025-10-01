using MediatR;
using UserManagement.Application.DTOs.Auth;
using UserManagement.Application.DTOs.Users;

namespace UserManagement.Application.Commands;

public record CreateUserCommand(
    string Name,
    string Email,
    string Password,
    int AccessLevelId
) : IRequest<UserDto>;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<AuthResponseDto>;