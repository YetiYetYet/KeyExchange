using System.Linq.Expressions;
using API.Db.Entity.Entity.Interface;

namespace API.Repository;

public interface ISpecification<T> where T : IBaseEntity, ISoftDelete
{
    public List<Expression<Func<T, bool>>>? Conditions { get; set; }

    public Expression<Func<T, object>>[]? Includes { get; set; }

    public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; set; }

    public string[]? OrderByStrings { get; set; }
}