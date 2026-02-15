using EnterpriseMembers.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace EnterpriseMembers.Infrastructure.Services;

public class InMemoryTokenBlacklistService : ITokenBlacklistService
{
    private readonly IMemoryCache _cache;
    private const string KeyPrefix = "blacklist_";

    public InMemoryTokenBlacklistService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task BlacklistTokenAsync(string token, DateTime expiry)
    {
        var key = GetKey(token);
        var timeToLive = expiry - DateTime.UtcNow;

        if (timeToLive > TimeSpan.Zero)
        {
            _cache.Set(key, true, timeToLive);
        }

        await Task.CompletedTask;
    }

    public async Task<bool> IsTokenBlacklistedAsync(string token)
    {
        var key = GetKey(token);
        var isBlacklisted = _cache.TryGetValue(key, out _);
        return await Task.FromResult(isBlacklisted);
    }

    private static string GetKey(string token)
    {
        // Use a hash to avoid storing full tokens in cache keys
        return $"{KeyPrefix}{token.GetHashCode()}";
    }
}
