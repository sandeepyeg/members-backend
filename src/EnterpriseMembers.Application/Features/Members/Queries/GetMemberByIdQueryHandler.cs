using AutoMapper;
using EnterpriseMembers.Application.DTOs;
using EnterpriseMembers.Application.Services;
using MediatR;

namespace EnterpriseMembers.Application.Features.Members.Queries;

public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, MemberDto?>
{
    private readonly IMemberService _memberService;

    public GetMemberByIdQueryHandler(IMemberService memberService)
    {
        _memberService = memberService;
    }

    public async Task<MemberDto?> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        return await _memberService.GetMemberByIdAsync(request.Id);
    }
}
