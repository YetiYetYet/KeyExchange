using API.Db.Entity.Entity.Interface;

namespace API.Repository;

public class PaginationSpecification<T> : Specification<T>
    where T : IBaseEntity, ISoftDelete
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = int.MaxValue;
}