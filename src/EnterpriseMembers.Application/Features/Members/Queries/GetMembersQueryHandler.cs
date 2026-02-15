using AutoMapper;
using EnterpriseMembers.Application.DTOs;
using EnterpriseMembers.Application.Services;
using MediatR;

namespace EnterpriseMembers.Application.Features.Members.Queries;

public class GetMembersQueryHandler : IRequestHandler<GetMembersQuery, PagedResultDto<MemberDto>>
{
    private readonly IMemberService _memberService;

    public GetMembersQueryHandler(IMemberService memberService)
    {
        _memberService = memberService;
    }

    public async Task<PagedResultDto<MemberDto>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _memberService.GetMembersAsync(
            request.Page,
            request.PageSize,
            request.Search,
            request.ExpiredOnly,
            request.SortBy,
            request.SortDir);

        return new PagedResultDto<MemberDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}
