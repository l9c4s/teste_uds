using AutoMapper;
using MediatR;
using UserManagement.Application.Commands;
using UserManagement.Application.DTOs.AcessLevels;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Exceptions;
using UserManagement.Domain.Interfaces;

public class CreateAccessLevelCommandHandler : IRequestHandler<CreateAccessLevelCommand, AccessLevelDto>
{
    private readonly IAccessLevelRepository _accessLevelRepository;
    private readonly IMapper _mapper;

    public CreateAccessLevelCommandHandler(IAccessLevelRepository accessLevelRepository, IMapper mapper)
    {
        _accessLevelRepository = accessLevelRepository;
        _mapper = mapper;
    }

    public async Task<AccessLevelDto> Handle(CreateAccessLevelCommand request, CancellationToken cancellationToken)
    {
        // Verificar se já existe um nível de acesso com este nome
        var existingAccessLevel = await _accessLevelRepository.GetByNameAsync(request.Name);
        if (existingAccessLevel != null)
        {
            throw new DomainException($"Access level with name {request.Name} already exists");
        }

        var accessLevel = _mapper.Map<AccessLevel>(request);
        var createdAccessLevel = await _accessLevelRepository.CreateAsync(accessLevel);

        return _mapper.Map<AccessLevelDto>(createdAccessLevel);
    }
}