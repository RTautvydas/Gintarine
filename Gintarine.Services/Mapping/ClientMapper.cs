using Gintarine.Repositories.Entities;

namespace Gintarine.Services.Mapping;

public static class ClientMapper
{
    public static List<Models.Clients.Client> Map(IEnumerable<Client> clients)
    {
        return clients.Select(Map).ToList();
    }
    
    public static Client Map<T>(T client)
        where T : Models.Clients.ClientBase, new()
    {
        return new Client
        {
            Address = client.Address,
            Name = client.Name,
            PostCode = client.PostCode
        };
    }

    private static Models.Clients.Client Map(Client client)
    {
        return new Models.Clients.Client
        {
            Address = client.Address,
            Name = client.Name,
            PostCode = client.PostCode
        };
    }
}