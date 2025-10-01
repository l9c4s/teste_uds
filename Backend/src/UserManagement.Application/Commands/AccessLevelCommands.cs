using MediatR;
using UserManagement.Application.DTOs;
using UserManagement.Application.DTOs.AcessLevels;

namespace UserManagement.Application.Commands;

public record AssignAccessLevelCommand(
    int UserId,
    int AccessLevelId
) : IRequest<UserAccessLevelDto>;

public record RevokeAccessLevelCommand(
    int UserId,
    int AccessLevelId
) : IRequest<bool>;

public record CreateAccessLevelCommand(
    string Name,
    string? Description
) : IRequest<AccessLevelDto>;