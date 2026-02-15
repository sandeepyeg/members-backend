using MediatR;

namespace EnterpriseMembers.Application.Features.Members.Commands;

public class DeleteMemberCommand : IRequest<Unit>
{
    public int Id { get; set; }
}
