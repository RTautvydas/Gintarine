using Gintarine.Repositories.Entities;

namespace Gintarine.Repositories;

public abstract class Auditable: EntityBase
{
    public DateTime CreatedAt { get; set; }
    
    public DateTime? ModifiedAt { get; set; }
    
    public virtual ICollection<Log> Logs { get; set; }
}