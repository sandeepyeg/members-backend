using EnterpriseMembers.Application.Interfaces;
using EnterpriseMembers.Infrastructure.Data;
using EnterpriseMembers.Infrastructure.Repositories;
using EnterpriseMembers.Infrastructure.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnterpriseMembers.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var rawConnectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        var sqliteBuilder = new SqliteConnectionStringBuilder(rawConnectionString)
        {
            // Azure Files is network storage; disable pooling and avoid WAL for stability.
            Pooling = false,
            Mode = SqliteOpenMode.ReadWriteCreate,
            Cache = SqliteCacheMode.Shared
        };

        // Add DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(sqliteBuilder.ToString(), sqlite =>
            {
                sqlite.CommandTimeout(30);
            }));

        // Add Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Add Repositories (still needed for direct injection in some cases)
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Add Services
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, JwtTokenService>();

        // Add Token Blacklist Service (in-memory for development)
        services.AddSingleton<ITokenBlacklistService, InMemoryTokenBlacklistService>();

        return services;
    }
}
