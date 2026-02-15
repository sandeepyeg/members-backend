using EnterpriseMembers.Application.DTOs;

namespace EnterpriseMembers.Application.Services;

public interface IMemberService
{
    // Query operations
    Task<(List<MemberDto> Items, int TotalCount)> GetMembersAsync(
        int page,
        int pageSize,
        string? search,
        bool expiredOnly,
        string sortBy,
        string sortDir);

    Task<MemberDto?> GetMemberByIdAsync(int id);
    Task<MemberDto?> GetMemberByEmailAsync(string email);

    // Command operations
    Task<MemberDto> CreateMemberAsync(CreateMemberDto dto);
    Task<MemberDto> UpdateMemberAsync(int id, UpdateMemberDto dto);
    Task DeleteMemberAsync(int id);

    // Business logic operations
    Task<bool> IsMembershipExpiredAsync(int memberId);
    Task<int> GetActiveMembersCountAsync();
}
