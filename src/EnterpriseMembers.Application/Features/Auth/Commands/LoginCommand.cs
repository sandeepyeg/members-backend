using EnterpriseMembers.Application.DTOs;
using MediatR;

namespace EnterpriseMembers.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<LoginResponseDto>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
