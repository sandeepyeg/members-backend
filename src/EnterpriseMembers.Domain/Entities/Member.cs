using EnterpriseMembers.Domain.Common;
using EnterpriseMembers.Domain.Enums;

namespace EnterpriseMembers.Domain.Entities;

public class Member : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public MembershipType MembershipType { get; set; }
    public DateTime ExpiryDate { get; set; }
}
