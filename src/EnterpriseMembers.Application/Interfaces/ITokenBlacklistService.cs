namespace EnterpriseMembers.Application.Interfaces;

public interface ITokenBlacklistService
{
    Task BlacklistTokenAsync(string token, DateTime expiry);
    Task<bool> IsTokenBlacklistedAsync(string token);
}
