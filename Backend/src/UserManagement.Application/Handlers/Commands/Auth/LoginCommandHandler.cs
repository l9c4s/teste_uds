



using AutoMapper;
using MediatR;
using UserManagement.Application.Commands;
using UserManagement.Application.DTOs.Auth;
using UserManagement.Application.DTOs.Users;
using UserManagement.Application.Interfaces.Services;
using UserManagement.Domain.Exceptions.Users;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Application.Handlers.Commands.Auth;


public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordService passwordService,
        ITokenService tokenService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Find user by email
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
        {
            throw new InvalidCredentialsException();
        }

        // Verify password
        if (!_passwordService.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        // Generate token
        var token = _tokenService.GenerateToken(user);

        // Return response
        return new AuthResponseDto(token, _mapper.Map<UserDto>(user));
    }
}