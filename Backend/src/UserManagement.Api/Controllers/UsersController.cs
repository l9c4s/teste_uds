using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Commands;
using UserManagement.Application.DTOs.Users;
using UserManagement.Application.Queries;
using UserManagement.Domain.Enums;

namespace UserManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    [HttpGet]
    [Authorize(Policy = nameof(AccessLevel.CommonUser))]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _mediator.Send(new GetUsersQuery());
        return Ok(users);
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Policy = nameof(AccessLevel.CommonUser))]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(id));

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    [HttpPost("register")]
    [Authorize]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        
        var command = new CreateUserCommand(createUserDto.Name,
                                            createUserDto.Email,
                                            createUserDto.Password,
                                            createUserDto.AccessLevel);
        var user = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

}