using Gintarine.Models.Clients;

namespace Gintarine.Services.Services;

public interface IClientsService
{
    public Task<List<Client>> GetClients();
    public Task ImportClients(List<ClientImport> clients);
    public Task ImportClientsPostCodes();
}