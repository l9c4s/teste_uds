

using MediatR;
using UserManagement.Application.Queries;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Application.Handlers.Queries.AcessLevel;

public class CheckUserAccessLevelQueryHandler : IRequestHandler<CheckUserAccessLevelQuery, bool>
{
    private readonly IUserAccessLevelRepository _userAccessLevelRepository;

    public CheckUserAccessLevelQueryHandler(IUserAccessLevelRepository userAccessLevelRepository)
    {
        _userAccessLevelRepository = userAccessLevelRepository;
    }

    public async Task<bool> Handle(CheckUserAccessLevelQuery request, CancellationToken cancellationToken)
    {
        return await _userAccessLevelRepository.UserHasAccessLevelAsync(request.UserId, request.AccessLevelId);
    }
}