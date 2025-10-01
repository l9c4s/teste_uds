
using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;
using UserManagement.Infrastructure.Data;

namespace UserManagement.Infrastructure.Repositories;

public class UserAccessLevelRepository : IUserAccessLevelRepository
{
    private readonly ApplicationDbContext _context;

    public UserAccessLevelRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserAccessLevel?> GetByIdAsync(int id)
    {
        return await _context.UserAccessLevels
            .Include(ual => ual.User)
            .Include(ual => ual.AccessLevel)
            .FirstOrDefaultAsync(ual => ual.Id == id);
    }

    public async Task<IEnumerable<UserAccessLevel>> GetByUserIdAsync(int userId)
    {
        return await _context.UserAccessLevels
            .Include(ual => ual.AccessLevel)
            .Where(ual => ual.UserId == userId)
            .OrderBy(ual => ual.AssignedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserAccessLevel>> GetActiveByUserIdAsync(int userId)
    {
        return await _context.UserAccessLevels
            .Include(ual => ual.AccessLevel)
            .Where(ual => ual.UserId == userId && ual.IsActive)
            .OrderBy(ual => ual.AssignedAt)
            .ToListAsync();
    }

    public async Task<UserAccessLevel?> GetActiveUserAccessLevelAsync(int userId, int accessLevelId)
    {
        return await _context.UserAccessLevels
            .Include(ual => ual.User)
            .Include(ual => ual.AccessLevel)
            .FirstOrDefaultAsync(ual => ual.UserId == userId
                                      && ual.AccessLevelId == accessLevelId
                                      && ual.IsActive);
    }

    public async Task<UserAccessLevel> CreateAsync(UserAccessLevel userAccessLevel)
    {
        _context.UserAccessLevels.Add(userAccessLevel);
        await _context.SaveChangesAsync();
        return userAccessLevel;
    }

    public async Task<UserAccessLevel> UpdateAsync(UserAccessLevel userAccessLevel)
    {
        _context.Entry(userAccessLevel).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return userAccessLevel;
    }

    public async Task DeleteAsync(int id)
    {
        var userAccessLevel = await _context.UserAccessLevels.FindAsync(id);
        if (userAccessLevel != null)
        {
            _context.UserAccessLevels.Remove(userAccessLevel);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RevokeAccessAsync(int userId, int accessLevelId)
    {
        var userAccessLevel = await GetActiveUserAccessLevelAsync(userId, accessLevelId);
        if (userAccessLevel != null)
        {
            userAccessLevel.IsActive = false;
            userAccessLevel.RevokedAt = DateTime.UtcNow;
            await UpdateAsync(userAccessLevel);
        }
    }

    public async Task<bool> UserHasAccessLevelAsync(int userId, int accessLevelId)
    {
        return await _context.UserAccessLevels
            .AnyAsync(ual => ual.UserId == userId
                          && ual.AccessLevelId == accessLevelId
                          && ual.IsActive);
    }
}