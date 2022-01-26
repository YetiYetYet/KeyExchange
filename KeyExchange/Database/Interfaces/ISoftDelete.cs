namespace KeyExchange.Database.Interfaces;

public interface ISoftDelete
{
    bool SoftDeleted { get; set; }
    DateTime? DeletedOn { get; set; }
    int? DeletedBy { get; set; }
}