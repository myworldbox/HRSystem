using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using HRSystem.Infrastructure.Data;
using HRSystem.Domain.Entities;

namespace HRSystem.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly HRSystemContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(HRSystemContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(object id) =>
        await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _dbSet.AsNoTracking().ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
        await _dbSet.AsNoTracking().Where(predicate).ToListAsync();

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(object id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) =>
        await _dbSet.AnyAsync(predicate);

    public async Task<int> SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}

// Specialized repository for Contract
public class ContractRepository : Repository<ContractModel>, IContractRepository
{
    public ContractRepository(HRSystemContext context) : base(context) { }

    // Add contract-specific methods here if needed
}

// Specialized repository for Staff
public class StaffRepository : Repository<StaffModel>, IStaffRepository
{
    public StaffRepository(HRSystemContext context) : base(context) { }

    // Add staff-specific methods here if needed
}