namespace EnterpriseMembers.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IMemberRepository Members { get; }
    IUserRepository Users { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
