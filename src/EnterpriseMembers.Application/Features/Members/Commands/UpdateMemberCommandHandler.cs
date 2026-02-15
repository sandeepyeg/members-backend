using EnterpriseMembers.Application.DTOs;
using EnterpriseMembers.Application.Services;
using MediatR;

namespace EnterpriseMembers.Application.Features.Members.Commands;

public class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, MemberDto>
{
    private readonly IMemberService _memberService;

    public UpdateMemberCommandHandler(IMemberService memberService)
    {
        _memberService = memberService;
    }

    public async Task<MemberDto> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var dto = new UpdateMemberDto
        {
            Name = request.Name,
            Email = request.Email,
            MembershipType = request.MembershipType,
            ExpiryDate = request.ExpiryDate
        };

        return await _memberService.UpdateMemberAsync(request.Id, dto);
    }
}
