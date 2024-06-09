using Gintarine.DTOs.Clients;
using Gintarine.Models.Clients;

namespace Gintarine.Mapping;

public static class ClientsMapper
{
    public static List<ClientResponseDto> Map(IEnumerable<Client> clients)
    {
        return clients.Select(Map<Client, ClientResponseDto>).ToList();
    }

    public static List<ClientImport> Map(IEnumerable<ClientImportDto> clients)
    {
        return clients.Select(Map<ClientImport, ClientImportDto>).ToList();
    }
    
    private static TModel Map<TModel, TDto>(TDto client)
        where TModel : ClientBase, new()
        where TDto : ClientDtoBase, new()
    {
        return new TModel
        {
            Address = client.Address,
            Name = client.Name,
            PostCode = client.PostCode
        };
    }
    
    private static TDto Map<TModel, TDto>(TModel client) 
        where TModel : ClientBase, new()
        where TDto : ClientDtoBase, new()
    {
        return new TDto
        {
            Address = client.Address,
            Name = client.Name,
            PostCode = client.PostCode
        };
    }
}