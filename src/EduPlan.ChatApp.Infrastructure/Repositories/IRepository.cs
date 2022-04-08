using System.Linq.Expressions;

namespace EduPlan.ChatApp.Infrastructure;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> All();

    Task<T> GetById(int id);

    Task<bool> CreateAsync(T entity);

    Task<bool> UpdateAsync(T entity);

    Task<bool> DeleteAsync(T entity);

    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
}
