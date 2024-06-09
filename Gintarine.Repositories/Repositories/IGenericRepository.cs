using System.Linq.Expressions;

namespace Gintarine.Repositories.Repositories;

public interface IGenericRepository<T> where T : class
{
    public Task Add(T entity);
    public Task<List<T>> GetAll();
    public Task Update(T entity);
    public Task Save();
    Task<bool> Any(Expression<Func<T, bool>> expression);
}