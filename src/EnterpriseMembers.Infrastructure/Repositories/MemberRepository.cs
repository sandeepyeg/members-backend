using EnterpriseMembers.Application.Interfaces;
using EnterpriseMembers.Domain.Entities;
using EnterpriseMembers.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseMembers.Infrastructure.Repositories;

public class MemberRepository : Repository<Member>, IMemberRepository
{
    public MemberRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<(List<Member> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? search,
        bool expiredOnly,
        string sortBy,
        string sortDir)
    {
        var query = _dbSet.AsNoTracking().AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(m => m.Name.Contains(search) || m.Email.Contains(search));
        }

        // Apply expired filter
        if (expiredOnly)
        {
            query = query.Where(m => m.ExpiryDate < DateTime.UtcNow);
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync();

        // Apply sorting
        query = sortBy.ToLower() switch
        {
            "name" => sortDir.ToLower() == "desc"
                ? query.OrderByDescending(m => m.Name)
                : query.OrderBy(m => m.Name),
            "expirydate" => sortDir.ToLower() == "desc"
                ? query.OrderByDescending(m => m.ExpiryDate)
                : query.OrderBy(m => m.ExpiryDate),
            _ => query.OrderBy(m => m.ExpiryDate)
        };

        // Apply pagination
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Member?> GetByEmailAsync(string email)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(m => m.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
    {
        var query = _dbSet.AsNoTracking().Where(m => m.Email == email);

        if (excludeId.HasValue)
        {
            query = query.Where(m => m.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}
