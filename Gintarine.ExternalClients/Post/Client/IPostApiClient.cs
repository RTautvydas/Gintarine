namespace Gintarine.ExternalClients.Post.Client;

public interface IPostApiClient
{
    Task<PostResult> SearchPostCode(string address);
}