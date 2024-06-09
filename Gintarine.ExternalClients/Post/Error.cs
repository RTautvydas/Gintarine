namespace Gintarine.ExternalClients.Post;

public class Error
{
    public Error(string message, bool isCritical)
    {
        Message = message;
        IsCritical = isCritical;
    }

    public Error(string message)
    {
        Message = message;
    }

    public bool IsCritical { get; }
    
    public string Message { get; }
}