using System.ComponentModel.DataAnnotations;

namespace API.Db.Entity.Entity.Interface;

public interface IBaseEntity
{
    [Key]
    public int Id { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public int? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}