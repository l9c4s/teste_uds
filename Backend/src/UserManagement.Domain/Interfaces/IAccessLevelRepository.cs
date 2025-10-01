

using UserManagement.Domain.Entities;

namespace UserManagement.Domain.Interfaces;

public interface IAccessLevelRepository
{
    Task<AccessLevel?> GetByIdAsync(int id);
    Task<AccessLevel?> GetByNameAsync(string name);
    Task<IEnumerable<AccessLevel>> GetAllAsync();
    Task<AccessLevel> CreateAsync(AccessLevel accessLevel);
    Task<AccessLevel> UpdateAsync(AccessLevel accessLevel);
    Task DeleteAsync(int id);
}