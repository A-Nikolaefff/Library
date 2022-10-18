using System.Linq.Expressions;
using LinqSpecs;

namespace Library.Repository;

public interface IRepository<T> 
{ 
        Task<T> CreateAsync(T item); 
        Task<IEnumerable<T>> Get();
        Task<IEnumerable<T>> Get(Specification<T> spec);
        Task<IEnumerable<T>> Get(Specification<T> spec, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> Get(params Expression<Func<T, object>>[] includes);
        Task<T?> GetFirstOrDefault(Specification<T> spec);
        Task<T?> GetFirstOrDefault(Specification<T> spec, params Expression<Func<T, object>>[] includes);
        void Update(T entityData);
        Task<T?> Delete(int id);
        Task<int> SaveChangesAsync();
}

