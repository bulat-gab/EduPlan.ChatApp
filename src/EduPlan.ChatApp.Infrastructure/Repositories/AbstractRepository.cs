using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace EduPlan.ChatApp.Infrastructure;

public abstract class AbstractRepository<T> : IRepository<T> where T : class
{
    protected ChatAppDbContext dbContext;

    internal DbSet<T> dbSet;

    public AbstractRepository(ChatAppDbContext context)
    {
        this.dbContext = context;
        this.dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> All()
    {
        return await dbSet.ToListAsync();
    }

    public async Task<T> GetById(int id)
    {
        return await dbSet.FindAsync(id);
    }

    public async Task<T> CreateAsync(T entity)
    {
        dbSet.Add(entity);
        await this.dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<T> DeleteAsync(T entity)
    {
        dbSet.Remove(entity);
        await this.dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        dbSet.Update(entity);
        await this.dbContext.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
    {
        return await dbSet.Where(predicate).ToListAsync();
    }
}
