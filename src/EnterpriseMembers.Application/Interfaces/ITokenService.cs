using EnterpriseMembers.Application.DTOs;

namespace EnterpriseMembers.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(int userId, string email, List<string> roles, List<string> permissions);
    int GetTokenExpiryMinutes();
}
