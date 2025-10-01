using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;
using UserManagement.Infrastructure.Data;

namespace UserManagement.Infrastructure.Repositories;

public class AccessLevelRepository : IAccessLevelRepository
{
    private readonly ApplicationDbContext _context;

    public AccessLevelRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AccessLevel?> GetByIdAsync(int id)
    {
        return await _context.AccessLevels.FindAsync(id);
    }

    public async Task<AccessLevel?> GetByNameAsync(string name)
    {
        return await _context.AccessLevels
            .FirstOrDefaultAsync(a => a.Name == name);
    }

    public async Task<IEnumerable<AccessLevel>> GetAllAsync()
    {
        return await _context.AccessLevels
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<AccessLevel> CreateAsync(AccessLevel accessLevel)
    {
        _context.AccessLevels.Add(accessLevel);
        await _context.SaveChangesAsync();
        return accessLevel;
    }

    public async Task<AccessLevel> UpdateAsync(AccessLevel accessLevel)
    {
        accessLevel.UpdatedAt = DateTime.UtcNow;
        _context.Entry(accessLevel).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return accessLevel;
    }

    public async Task DeleteAsync(int id)
    {
        var accessLevel = await _context.AccessLevels.FindAsync(id);
        if (accessLevel != null)
        {
            _context.AccessLevels.Remove(accessLevel);
            await _context.SaveChangesAsync();
        }
    }
}

