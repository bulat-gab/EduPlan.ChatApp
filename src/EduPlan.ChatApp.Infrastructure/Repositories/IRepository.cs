using System.Linq.Expressions;

namespace EduPlan.ChatApp.Infrastructure;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> All();

    Task<T> GetById(int id);

    Task<T> CreateAsync(T entity);

    Task<T> UpdateAsync(T entity);

    Task<T> DeleteAsync(T entity);

    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
}
