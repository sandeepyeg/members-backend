using EnterpriseMembers.Application.DTOs;
using MediatR;

namespace EnterpriseMembers.Application.Features.Members.Commands;

public class CreateMemberCommand : IRequest<MemberDto>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MembershipType { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
}
