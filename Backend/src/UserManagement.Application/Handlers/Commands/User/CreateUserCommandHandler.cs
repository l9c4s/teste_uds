using MediatR;
using Microsoft.AspNetCore.Http;
using UserManagement.Application.Commands;
using UserManagement.Application.DTOs.Users;
using UserManagement.Application.Interfaces.Services;
using UserManagement.Domain.Exceptions.Users;
using UserManagement.Domain.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Handlers.Commands.User;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IAccessLevelRepository _accessLevelRepository;
    private readonly IUserAccessLevelRepository _userAccessLevelRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IPasswordService passwordService,
        IAccessLevelRepository accessLevelRepository,
        IUserAccessLevelRepository userAccessLevelRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _accessLevelRepository = accessLevelRepository;
        _userAccessLevelRepository = userAccessLevelRepository;
        _httpContextAccessor = httpContextAccessor;
    }



    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Check if email already exists
        if (await _userRepository.EmailExistsAsync(request.Email))
        {
            throw new EmailAlreadyExistsException(request.Email);
        }


        bool isAdmin = GetAccessUser();
        int accessLevelIdToAssign = 4; // Default: Viewer
        AccessLevel? finalAccessLevel = null;

        // Apenas admins podem definir níveis diferentes de Viewer
        if (request.AccessLevelId != 0 && isAdmin && request.AccessLevelId != 4)
        {
            var requestedAccessLevel = await _accessLevelRepository.GetByIdAsync(request.AccessLevelId);
            if (requestedAccessLevel == null)
            {
                throw new AccessLevelNotFoundException(request.AccessLevelId);
            }
            accessLevelIdToAssign = request.AccessLevelId;
            finalAccessLevel = requestedAccessLevel;
        }
        else
        {
            // Buscar o AccessLevel Viewer
            finalAccessLevel = await _accessLevelRepository.GetByIdAsync(4);
            if (finalAccessLevel == null)
            {
                throw new InvalidOperationException("Nível de acesso 'Viewer' (ID: 4) não encontrado no sistema");
            }
        }




        // Create user entity
        var user = new Domain.Entities.User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = _passwordService.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        


        // Save to database
        var createdUser = await _userRepository.CreateAsync(user);

          var userAccessLevel = new UserAccessLevel
        {
            UserId = createdUser.Id,
            AccessLevelId = accessLevelIdToAssign,
            IsActive = true,
            AssignedAt = DateTime.UtcNow
        };

        await _userAccessLevelRepository.CreateAsync(userAccessLevel);



            
            
        return new UserDto(
            createdUser.Id,
            createdUser.Name,
            createdUser.Email,
            createdUser.CreatedAt,
            createdUser.UpdatedAt,
            finalAccessLevel?.Name
        );
    }

    private bool GetAccessUser()
    {
        // Verificar o nível de acesso do usuário atual
        var currentUser = _httpContextAccessor.HttpContext?.User;
        var currentUserAccessLevel = currentUser?.FindFirst("AccessLevel")?.Value;
        var isAdmin = currentUserAccessLevel == "Administrator";
        return isAdmin;

    }

   
    
}