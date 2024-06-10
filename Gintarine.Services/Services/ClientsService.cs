using Gintarine.ExternalClients.Post.Client;
using Gintarine.Repositories.Entities;
using Gintarine.Repositories.Repositories;
using Gintarine.Services.Mapping;
using Gintarine.Services.Validators;
using Microsoft.Extensions.Logging;

namespace Gintarine.Services.Services;

public class ClientsService : IClientsService
{
    private readonly IGenericRepository<Client> _genericRepository;
    private readonly ILogger<ClientsService> _logger;
    private readonly IPostApiClient _postApiClient;
    
    public ClientsService(IGenericRepository<Client> genericRepository, IPostApiClient postApiClient, ILogger<ClientsService> logger)
    {
        _genericRepository = genericRepository;
        _postApiClient = postApiClient;
        _logger = logger;
    }

    public async Task<List<Models.Clients.Client>> GetClients()
    {
        var clients = await _genericRepository.GetAll();
        return ClientMapper.Map(clients);
    }

    public async Task ImportClients(List<Models.Clients.ClientImport> clients)
    {
        if (clients.Count != 0)
        {
            foreach (var client in clients)
            {
                var isExistingClient = await IsClientExists(client.Name);
                if (!isExistingClient)
                {
                    await AddClient(client);
                }
            }

            await _genericRepository.Save();
        }
    }

    public async Task ImportClientsPostCodes()
    {
        var clients = await _genericRepository.GetAll();
        if (clients.Count != 0)
        {
            foreach (var client in clients)
            {
                var shouldContinue = await TryProcessClientPostCodeUpdate(client);
                if (!shouldContinue)
                {
                    break;
                }
            }
    
            await _genericRepository.Save();
        }
    }

    private async Task<bool> IsClientExists(string clientName)
    {
        return await _genericRepository
            .Any(x => x.Name.Equals(clientName));
    }
    private async Task AddClient(Models.Clients.ClientImport client)
    {
        var clientEntity = ClientMapper.Map(client);
        await _genericRepository.Add(clientEntity);
    }

    private async Task<bool> TryProcessClientPostCodeUpdate(Client client)
    {
        try
        {
            var postResult = await _postApiClient.SearchPostCode(client.Address);
            if (postResult.Error == default && 
                PostCodeValidator.IsValidPostcode(postResult.PostCode) &&
                !client.PostCode.Equals(postResult.PostCode))
            {
                client.PostCode = postResult.PostCode;
                await _genericRepository.Update(client);
            }
            else if (postResult.Error is { IsCritical: true })
            {
                _logger.LogError("{message}", postResult.Error.Message);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving post code");
        }

        return true;
    }
}