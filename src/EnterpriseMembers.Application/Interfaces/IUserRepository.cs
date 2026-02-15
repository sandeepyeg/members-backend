using EnterpriseMembers.Domain.Entities;

namespace EnterpriseMembers.Application.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<List<Role>> GetUserRolesAsync(int userId);
    Task<List<Permission>> GetUserPermissionsAsync(int userId);
}
