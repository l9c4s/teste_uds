using MediatR;
using UserManagement.Application.DTOs.Users;

namespace UserManagement.Application.Queries;

public record GetUsersQuery() : IRequest<IEnumerable<UserDto>>;

public record GetUserByIdQuery(int Id) : IRequest<UserDto?>;