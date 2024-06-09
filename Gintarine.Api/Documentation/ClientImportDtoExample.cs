using Gintarine.DTOs.Clients;
using Gintarine.Helpers;
using Swashbuckle.AspNetCore.Filters;

namespace Gintarine.Documentation;

public class ClientImportDtoExample : IExamplesProvider<List<ClientImportDto>>
{
    public List<ClientImportDto> GetExamples()
    {
        const string jsonFilePath = "klientai.json";
        return JsonLoader.LoadClientsFromJson<List<ClientImportDto>>(jsonFilePath);
    }
}