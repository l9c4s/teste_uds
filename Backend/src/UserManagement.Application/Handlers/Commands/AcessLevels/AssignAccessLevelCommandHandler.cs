
using AutoMapper;
using MediatR;
using UserManagement.Application.Commands;
using UserManagement.Application.DTOs.AcessLevels;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Exceptions;
using UserManagement.Domain.Exceptions.Users;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Application.Handlers.Commands.AcessLevels;



public class AssignAccessLevelCommandHandler : IRequestHandler<AssignAccessLevelCommand, UserAccessLevelDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IAccessLevelRepository _accessLevelRepository;
    private readonly IUserAccessLevelRepository _userAccessLevelRepository;
    private readonly IMapper _mapper;

    public AssignAccessLevelCommandHandler(
        IUserRepository userRepository,
        IAccessLevelRepository accessLevelRepository,
        IUserAccessLevelRepository userAccessLevelRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _accessLevelRepository = accessLevelRepository;
        _userAccessLevelRepository = userAccessLevelRepository;
        _mapper = mapper;
    }

    public async Task<UserAccessLevelDto> Handle(AssignAccessLevelCommand request, CancellationToken cancellationToken)
    {
        // Verificar se o usuário existe
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new UserNotFoundException(request.UserId);

        // Verificar se o nível de acesso existe
        var accessLevel = await _accessLevelRepository.GetByIdAsync(request.AccessLevelId);
        if (accessLevel == null)
            throw new DomainException($"Access level with id {request.AccessLevelId} not found");

        // Verificar se o usuário já possui este nível de acesso ativo
        if (await _userAccessLevelRepository.UserHasAccessLevelAsync(request.UserId, request.AccessLevelId))
        {
            throw new DomainException($"User already has access level {accessLevel.Name}");
        }

        // Criar a associação
        var userAccessLevel = new UserAccessLevel
        {
            UserId = request.UserId,
            AccessLevelId = request.AccessLevelId,
            AssignedAt = DateTime.UtcNow,
            IsActive = true
        };

        var createdUserAccessLevel = await _userAccessLevelRepository.CreateAsync(userAccessLevel);

        // Carregar as entidades relacionadas para o mapeamento
        createdUserAccessLevel.User = user;
        createdUserAccessLevel.AccessLevel = accessLevel;

        return _mapper.Map<UserAccessLevelDto>(createdUserAccessLevel);
    }
}
