

using MediatR;
using UserManagement.Application.Commands;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Application.Handlers.Commands.AcessLevels;


public class RevokeAccessLevelCommandHandler : IRequestHandler<RevokeAccessLevelCommand, bool>
{
    private readonly IUserAccessLevelRepository _userAccessLevelRepository;

    public RevokeAccessLevelCommandHandler(IUserAccessLevelRepository userAccessLevelRepository)
    {
        _userAccessLevelRepository = userAccessLevelRepository;
    }

    public async Task<bool> Handle(RevokeAccessLevelCommand request, CancellationToken cancellationToken)
    {
        await _userAccessLevelRepository.RevokeAccessAsync(request.UserId, request.AccessLevelId);
        return true;
    }
}