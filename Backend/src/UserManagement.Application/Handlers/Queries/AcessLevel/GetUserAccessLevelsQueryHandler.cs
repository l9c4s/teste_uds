

using AutoMapper;
using MediatR;
using UserManagement.Application.DTOs.AcessLevels;
using UserManagement.Application.Queries;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Application.Handlers.Queries.AcessLevel;

public class GetUserAccessLevelsQueryHandler : IRequestHandler<GetUserAccessLevelsQuery, IEnumerable<UserAccessLevelDto>>
{
    private readonly IUserAccessLevelRepository _userAccessLevelRepository;
    private readonly IMapper _mapper;

    public GetUserAccessLevelsQueryHandler(IUserAccessLevelRepository userAccessLevelRepository, IMapper mapper)
    {
        _userAccessLevelRepository = userAccessLevelRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserAccessLevelDto>> Handle(GetUserAccessLevelsQuery request, CancellationToken cancellationToken)
    {
        var userAccessLevels = await _userAccessLevelRepository.GetByUserIdAsync(request.UserId);
        return _mapper.Map<IEnumerable<UserAccessLevelDto>>(userAccessLevels);
    }
}