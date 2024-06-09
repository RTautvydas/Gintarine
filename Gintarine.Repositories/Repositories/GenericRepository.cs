using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Gintarine.Repositories.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly GintarineContext _databaseContext;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(GintarineContext context)
    {
        _databaseContext = context;
        _dbSet = context.Set<T>();
    }

    public async Task Add(T entity)
    {
        await _dbSet.AddAsync(entity);
        await Save();
    }
    
    public async Task<List<T>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task Update(T entity)
    {
        _dbSet.Update(entity);
        await Save();
    }

    public async Task Save()
    {
        await _databaseContext.SaveChangesAsync();
    }

    public async Task<bool> Any(Expression<Func<T, bool>> expression)
    {
        return await _databaseContext.Set<T>().AnyAsync(expression);
    }
}