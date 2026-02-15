using EnterpriseMembers.Domain.Entities;
using EnterpriseMembers.Domain.Enums;
using EnterpriseMembers.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseMembers.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Ensure database is created
        await context.Database.MigrateAsync();

        // Seed Permissions
        if (!await context.Permissions.AnyAsync())
        {
            var permissions = new List<Permission>
            {
                new() { Name = PermissionNames.MembersRead, Description = "Can read members", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new() { Name = PermissionNames.MembersWrite, Description = "Can create and update members", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new() { Name = PermissionNames.MembersDelete, Description = "Can delete members", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };

            await context.Permissions.AddRangeAsync(permissions);
            await context.SaveChangesAsync();
        }

        // Seed Roles
        if (!await context.Roles.AnyAsync())
        {
            var roles = new List<Role>
            {
                new() { Name = "Admin", Description = "Administrator with full access", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new() { Name = "Reader", Description = "Read-only access", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }

        // Seed RolePermissions
        if (!await context.RolePermissions.AnyAsync())
        {
            var adminRole = await context.Roles.FirstAsync(r => r.Name == "Admin");
            var readerRole = await context.Roles.FirstAsync(r => r.Name == "Reader");

            var allPermissions = await context.Permissions.ToListAsync();
            var readPermission = allPermissions.First(p => p.Name == PermissionNames.MembersRead);

            var rolePermissions = new List<RolePermission>();

            // Admin gets all permissions
            foreach (var permission in allPermissions)
            {
                rolePermissions.Add(new RolePermission { RoleId = adminRole.Id, PermissionId = permission.Id });
            }

            // Reader gets only read permission
            rolePermissions.Add(new RolePermission { RoleId = readerRole.Id, PermissionId = readPermission.Id });

            await context.RolePermissions.AddRangeAsync(rolePermissions);
            await context.SaveChangesAsync();
        }

        // Seed Admin User
        if (!await context.Users.AnyAsync())
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("123456");
            var adminUser = new User
            {
                Email = "admin@company.com",
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();

            // Assign Admin role to admin user
            var adminRole = await context.Roles.FirstAsync(r => r.Name == "Admin");
            var userRole = new UserRole { UserId = adminUser.Id, RoleId = adminRole.Id };
            await context.UserRoles.AddAsync(userRole);
            await context.SaveChangesAsync();
        }

        // Seed Sample Members
        if (!await context.Members.AnyAsync())
        {
            var members = new List<Member>
            {
                new()
                {
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    MembershipType = MembershipType.Premium,
                    ExpiryDate = DateTime.UtcNow.AddMonths(6),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new()
                {
                    Name = "Jane Smith",
                    Email = "jane.smith@example.com",
                    MembershipType = MembershipType.Basic,
                    ExpiryDate = DateTime.UtcNow.AddMonths(3),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new()
                {
                    Name = "Bob Johnson",
                    Email = "bob.johnson@example.com",
                    MembershipType = MembershipType.Premium,
                    ExpiryDate = DateTime.UtcNow.AddMonths(-2), // Expired
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new()
                {
                    Name = "Alice Williams",
                    Email = "alice.williams@example.com",
                    MembershipType = MembershipType.Basic,
                    ExpiryDate = DateTime.UtcNow.AddMonths(12),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new()
                {
                    Name = "Charlie Brown",
                    Email = "charlie.brown@example.com",
                    MembershipType = MembershipType.Premium,
                    ExpiryDate = DateTime.UtcNow.AddMonths(-1), // Expired
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new()
                {
                    Name = "Diana Prince",
                    Email = "diana.prince@example.com",
                    MembershipType = MembershipType.Basic,
                    ExpiryDate = DateTime.UtcNow.AddMonths(9),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new()
                {
                    Name = "Ethan Hunt",
                    Email = "ethan.hunt@example.com",
                    MembershipType = MembershipType.Premium,
                    ExpiryDate = DateTime.UtcNow.AddMonths(4),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            await context.Members.AddRangeAsync(members);
            await context.SaveChangesAsync();
        }
    }
}
