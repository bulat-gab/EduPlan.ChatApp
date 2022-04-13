using System.Linq.Expressions;

namespace EduPlan.ChatApp.Infrastructure;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> All();

    Task<T> GetById(int id);

    Task<T> Create(T entity);

    Task<T> Update(T entity);

    Task<T> Delete(T entity);

    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
}
