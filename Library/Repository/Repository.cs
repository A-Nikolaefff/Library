using System.Linq.Expressions;
using Library.Domain;
using Library.Storage;
using LinqSpecs;
using Microsoft.EntityFrameworkCore;

namespace Library.Repository;

public abstract class Repository<T> : IRepository<T> where T : Entity
{
    private readonly LibraryContext _context;

    public Repository(LibraryContext context)
    {
            this._context = context;
    }
        
    public virtual async Task<T> CreateAsync(T newEntity)
    {
        var entityEntry = await _context.Set<T>().AddAsync(newEntity);
        return entityEntry.Entity;
    }
        
    public virtual async Task<IEnumerable<T>> Get()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> Get(Specification<T> spec)
    {
        return await _context.Set<T>().Where(spec).ToListAsync();
    }

    public async Task<IEnumerable<T>> Get(Specification<T> spec, params Expression<Func<T, object>>[] includes)
    {
        return await Include(includes).Where(spec).ToListAsync();
    }

    public async Task<IEnumerable<T>> Get(params Expression<Func<T, object>>[] includes)
    {
        return await Include(includes).ToListAsync();
    }

    public async Task<T?> GetFirstOrDefault(Specification<T> spec)
    {
        return await _context.Set<T>().Where(spec).FirstOrDefaultAsync();

    }

    public async Task<T?> GetFirstOrDefault(Specification<T> spec, params Expression<Func<T, object>>[] includes)
    {
        return await Include(includes).Where(spec).FirstOrDefaultAsync();
    }

    public void Update(T entityData)
    {
        _context.Set<T>().Update(entityData);
    }
        
    public virtual async Task<T?> Delete(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity is null) return null;
        return _context.Set<T>().Remove(entity).Entity;
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    private IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
    {
        return includes
            .Aggregate<Expression<Func<T, object>>, IQueryable<T>>
            (_context.Set<T>().AsNoTracking(), (current, includeProperty)
                => current.Include(includeProperty));
    }
}

