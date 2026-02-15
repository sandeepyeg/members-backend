using EnterpriseMembers.Application.DTOs;
using MediatR;

namespace EnterpriseMembers.Application.Features.Members.Queries;

public class GetMemberByIdQuery : IRequest<MemberDto?>
{
    public int Id { get; set; }
}
