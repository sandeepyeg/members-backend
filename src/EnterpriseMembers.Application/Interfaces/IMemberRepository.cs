using EnterpriseMembers.Domain.Entities;

namespace EnterpriseMembers.Application.Interfaces;

public interface IMemberRepository : IRepository<Member>
{
    Task<(List<Member> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? search,
        bool expiredOnly,
        string sortBy,
        string sortDir);
    
    Task<Member?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email, int? excludeId = null);
}
