using AutoMapper;
using MediatR;
using UserManagement.Application.DTOs.AcessLevels;
using UserManagement.Application.Queries;
using UserManagement.Domain.Interfaces;


namespace UserManagement.Application.Handlers.Queries.AcessLevel;

public class GetAccessLevelsQueryHandler : IRequestHandler<GetAccessLevelsQuery, IEnumerable<AccessLevelDto>>
{
    private readonly IAccessLevelRepository _accessLevelRepository;
    private readonly IMapper _mapper;

    public GetAccessLevelsQueryHandler(IAccessLevelRepository accessLevelRepository, IMapper mapper)
    {
        _accessLevelRepository = accessLevelRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AccessLevelDto>> Handle(GetAccessLevelsQuery request, CancellationToken cancellationToken)
    {
        var accessLevels = await _accessLevelRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<AccessLevelDto>>(accessLevels);
    }
}