
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
