using System.ComponentModel.DataAnnotations;

namespace Gintarine.ExternalClients.Post;

public class PostSettings
{
    [Required(AllowEmptyStrings = false)]
    public string Url { get; set; }
    
    
    [Required(AllowEmptyStrings = false)]
    public string ApiKey { get; set; }
}