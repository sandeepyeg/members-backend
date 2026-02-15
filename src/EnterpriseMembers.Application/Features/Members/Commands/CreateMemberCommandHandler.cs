using EnterpriseMembers.Application.DTOs;
using EnterpriseMembers.Application.Services;
using MediatR;

namespace EnterpriseMembers.Application.Features.Members.Commands;

public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, MemberDto>
{
    private readonly IMemberService _memberService;

    public CreateMemberCommandHandler(IMemberService memberService)
    {
        _memberService = memberService;
    }

    public async Task<MemberDto> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var dto = new CreateMemberDto
        {
            Name = request.Name,
            Email = request.Email,
            MembershipType = request.MembershipType,
            ExpiryDate = request.ExpiryDate
        };

        return await _memberService.CreateMemberAsync(dto);
    }
}
