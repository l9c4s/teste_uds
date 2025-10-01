
using AutoMapper;
using MediatR;
using UserManagement.Application.DTOs.Users;
using UserManagement.Application.Queries;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Application.Handlers.Queries.User;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        return user != null ? _mapper.Map<UserDto>(user) : null;
    }
}