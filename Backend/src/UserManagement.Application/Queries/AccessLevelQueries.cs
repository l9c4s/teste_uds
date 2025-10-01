using MediatR;
using UserManagement.Application.DTOs.AcessLevels;

namespace UserManagement.Application.Queries;

public record GetAccessLevelsQuery() : IRequest<IEnumerable<AccessLevelDto>>;

public record GetUserAccessLevelsQuery(int UserId) : IRequest<IEnumerable<UserAccessLevelDto>>;

public record GetActiveUserAccessLevelsQuery(int UserId) : IRequest<IEnumerable<UserAccessLevelDto>>;

public record CheckUserAccessLevelQuery(int UserId, int AccessLevelId) : IRequest<bool>;