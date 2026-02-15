using EnterpriseMembers.Application.DTOs;
using MediatR;

namespace EnterpriseMembers.Application.Features.Members.Queries;

public record GetMembersQuery(
    int Page = 1,
    int PageSize = 10,
    string? Search = null,
    bool ExpiredOnly = false,
    string SortBy = "name",
    string SortDir = "asc"
) : IRequest<PagedResultDto<MemberDto>>;
