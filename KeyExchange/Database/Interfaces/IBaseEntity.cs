using System.ComponentModel.DataAnnotations;

namespace KeyExchange.Database.Interfaces;

public interface IBaseEntity
{
    [Key]
    public int Id { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public int? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}