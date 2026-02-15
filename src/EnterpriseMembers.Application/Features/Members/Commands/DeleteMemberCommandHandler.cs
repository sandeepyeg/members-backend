using EnterpriseMembers.Application.Services;
using MediatR;

namespace EnterpriseMembers.Application.Features.Members.Commands;

public class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand, Unit>
{
    private readonly IMemberService _memberService;

    public DeleteMemberCommandHandler(IMemberService memberService)
    {
        _memberService = memberService;
    }

    public async Task<Unit> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        await _memberService.DeleteMemberAsync(request.Id);
        return Unit.Value;
    }
}
