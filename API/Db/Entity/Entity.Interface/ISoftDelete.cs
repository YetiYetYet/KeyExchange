namespace API.Db.Entity.Entity.Interface;

public interface ISoftDelete
{
    bool SoftDeleted { get; set; }
    DateTime? DeletedOn { get; set; }
    Guid? DeletedBy { get; set; }
}