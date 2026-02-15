using EnterpriseMembers.Application.DTOs;

namespace EnterpriseMembers.Application.Services;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(string email, string password);
    Task LogoutAsync(string token);
    Task<bool> ValidateCredentialsAsync(string email, string password);
}
