using EnterpriseMembers.Domain.Enums;

namespace EnterpriseMembers.Application.DTOs;

public class MemberDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MembershipType { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
}

public class CreateMemberDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MembershipType { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
}

public class UpdateMemberDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MembershipType { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
}
