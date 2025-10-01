
using UserManagement.Domain.Entities;

namespace UserManagement.Domain.Interfaces;


public interface IUserAccessLevelRepository
{
    Task<UserAccessLevel?> GetByIdAsync(int id);
    Task<IEnumerable<UserAccessLevel>> GetByUserIdAsync(int userId);
    Task<IEnumerable<UserAccessLevel>> GetActiveByUserIdAsync(int userId);
    Task<UserAccessLevel?> GetActiveUserAccessLevelAsync(int userId, int accessLevelId);
    Task<UserAccessLevel> CreateAsync(UserAccessLevel userAccessLevel);
    Task<UserAccessLevel> UpdateAsync(UserAccessLevel userAccessLevel);
    Task DeleteAsync(int id);
    Task RevokeAccessAsync(int userId, int accessLevelId);
    Task<bool> UserHasAccessLevelAsync(int userId, int accessLevelId);
}