using System.Linq.Expressions;
using Library.Domain;
using LinqSpecs;

namespace Library.Specifications;

public class ByIdSpec<T> : Specification<T> where T : Entity
{
    public int Id { get; set; }

    public ByIdSpec(int id)
    {
        Id = id;
    }
    
    public override Expression<Func<T, bool>> ToExpression()
    {
        return e => e.Id == Id;
    }
}