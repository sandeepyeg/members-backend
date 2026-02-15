using EnterpriseMembers.Application.DTOs;
using EnterpriseMembers.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace EnterpriseMembers.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly ITokenBlacklistService _tokenBlacklistService;

    public AuthService(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        ITokenBlacklistService tokenBlacklistService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _tokenBlacklistService = tokenBlacklistService;
    }

    public async Task<LoginResponseDto> LoginAsync(string email, string password)
    {
        // Validate credentials
        if (!await ValidateCredentialsAsync(email, password))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        // Get user with roles and permissions
        var user = await _unitOfWork.Users.GetByEmailAsync(email);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        // Get roles and permissions
        var roles = await _unitOfWork.Users.GetUserRolesAsync(user.Id);
        var permissions = await _unitOfWork.Users.GetUserPermissionsAsync(user.Id);

        // Convert to string lists
        var roleNames = roles.Select(r => r.Name).ToList();
        var permissionNames = permissions.Select(p => p.Name).ToList();

        // Generate token
        var token = _tokenService.GenerateToken(user.Id, user.Email, roleNames, permissionNames);

        return new LoginResponseDto
        {
            AccessToken = token,
            ExpiresIn = _tokenService.GetTokenExpiryMinutes() * 60, // Convert to seconds
            User = new UserDto
            {
                Email = user.Email,
                Roles = roleNames,
                Permissions = permissionNames
            }
        };
    }

    public async Task LogoutAsync(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException("Token cannot be null or empty", nameof(token));
        }

        // Parse token to get expiry
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var expiry = jwtToken.ValidTo;

        // Blacklist the token
        await _tokenBlacklistService.BlacklistTokenAsync(token, expiry);
    }

    public async Task<bool> ValidateCredentialsAsync(string email, string password)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email);

        if (user == null)
        {
            return false;
        }

        return _passwordHasher.VerifyPassword(password, user.PasswordHash);
    }
}
