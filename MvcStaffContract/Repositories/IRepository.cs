using MvcStaffContract.Models;
using System.Linq.Expressions;

namespace MvcStaffContract.Repositories;

public interface IRepository<T> where T : class
{
    // Queries
    Task<T?> GetByIdAsync(object id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    // Commands
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(object id);

    // Save changes (optional if you use Unit of Work)
    Task<int> SaveChangesAsync();
}

public interface IContractRepository : IRepository<ContractModel>
{
}

public interface IStaffRepository : IRepository<StaffModel>
{
}
