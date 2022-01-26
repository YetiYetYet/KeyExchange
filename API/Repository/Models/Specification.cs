using API.Db.Entity.Entity.Interface;

namespace API.Repository;

public class Specification<T> : BaseSpecification<T>
    where T : IBaseEntity, ISoftDelete
{
    public string? Keyword { get; set; }
    public Search? AdvancedSearch { get; set; }
    public Filters<T>? Filters { get; set; }
}
