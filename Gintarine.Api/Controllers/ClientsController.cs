using Gintarine.Documentation;
using Gintarine.DTOs.Clients;
using Gintarine.Mapping;
using Gintarine.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Gintarine.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientsController : ControllerBase
{
    private readonly IClientsService _clientsService;
    
    public ClientsController(IClientsService clientsService)
    {
        _clientsService = clientsService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClientResponseDto>>> GetClients()
    {
        var clients = await _clientsService.GetClients();
        var clientDtos = ClientsMapper.Map(clients);
        return Ok(clientDtos);
    }
    
    [HttpPost("import")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerRequestExample(typeof(List<ClientImportDto>), typeof(ClientImportDtoExample))]
    public async Task<IActionResult> ImportClients([FromBody] List<ClientImportDto> clients)
    {
        if (clients == null || clients.Count == 0)
        {
            return BadRequest("No clients provided for import.");
        }

        var mappedClients = ClientsMapper.Map(clients);
        await _clientsService.ImportClients(mappedClients);
        return Ok();
    }

    [HttpPost("postCodes/import")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ImportPostCodes()
    {
        await _clientsService.ImportClientsPostCodes();
        return Ok();
    }
}