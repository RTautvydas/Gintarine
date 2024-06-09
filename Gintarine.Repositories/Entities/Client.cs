using System.ComponentModel.DataAnnotations;

namespace Gintarine.Repositories.Entities;

public class Client : Auditable
{
    public string Name { get; set; }
    
    public string Address { get; set; }
    
    [MaxLength(16)]
    public string PostCode { get; set; }
}