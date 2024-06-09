using System.Text.Json.Serialization;

namespace Gintarine.ExternalClients.Post.DTOs;

public class PostData
{
    [JsonPropertyName("post_code")]
    public string PostCode { get; set; }
    
    public string Address { get; set; }
    
    public string Street { get; set; }
    
    public string Number { get; set; }
    
    [JsonPropertyName("only_number")]
    public string OnlyNumber { get; set; }
    
    public string Housing { get; set; }
    
    public string City { get; set; }
    
    public string Eldership { get; set; }
    
    public string Municipality { get; set; }
    
    public string Post { get; set; }
    
    public string Mailbox { get; set; }
}