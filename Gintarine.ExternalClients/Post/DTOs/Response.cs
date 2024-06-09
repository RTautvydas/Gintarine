using System.Text.Json.Serialization;

namespace Gintarine.ExternalClients.Post.DTOs;

public class Response
{
    public string Status { get; set; }
    
    public bool Success { get; set; }
    
    
    public string Message { get; set; }
    
    
    [JsonPropertyName("message_code")]
    public int MessageCode { get; set; }
    
    public int Total { get; set; }
    
    public List<PostData> Data { get; set; }
}