using System.Linq.Expressions;

namespace ClickHouseApi.GenericRepository;

public interface IGenericRepository<T> where T : class, new()
{
    Task CreateTableAsync();
    Task DeleteTableAsync();
    Task InsertAsync(T entity);
    Task BulkInsertAsync(IEnumerable<T> entities);
    Task DeleteAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate);
}
