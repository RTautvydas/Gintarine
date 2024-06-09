namespace Gintarine.ExternalClients.Post;

public static class PostResultFactory
{
    public static PostResult CreateSuccess(string postCode)
    {
        return new PostResult { PostCode = postCode };
    }

    public static PostResult CreateFailure(Exception exception)
    {
        return new PostResult { Error = new Error(exception.Message) };
    }

    public static PostResult CreateFailure(ErrorFactory.ErrorCode errorCode)
    {
        var error = ErrorFactory.Create(errorCode);
        return new PostResult { Error = error };
    }
}