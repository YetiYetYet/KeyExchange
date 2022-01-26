using System.Linq.Expressions;
using API.Db.Entity.Entity.Interface;

namespace API.Repository;

public class BaseSpecification<T> : ISpecification<T> where T : IBaseEntity, ISoftDelete
{
    public List<Expression<Func<T, bool>>>? Conditions { get; set; } = null;

    public Expression<Func<T, object>>[]? Includes { get; set; } = null;

    public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; set; } = null;

    public string[]? OrderByStrings { get; set; } = default;
}