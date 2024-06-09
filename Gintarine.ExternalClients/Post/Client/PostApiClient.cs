using System.Text.Json;
using Gintarine.ExternalClients.Extensions;
using Gintarine.ExternalClients.Post.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MessageCodes = Gintarine.ExternalClients.Post.Constants.Codes;
using QueryConstants = Gintarine.ExternalClients.Post.Constants.Query;

namespace Gintarine.ExternalClients.Post.Client;

public class PostApiClient : IPostApiClient
{
    private readonly PostSettings _postSettings;
    private readonly HttpClient _httpClient;
    private readonly ILogger<PostApiClient> _logger;

    public PostApiClient(IOptions<PostSettings> postSettings,
        HttpClient httpClient, ILogger<PostApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _postSettings = postSettings.Value;
    }

    public async Task<PostResult> SearchPostCode(string address)
    {
        if (string.IsNullOrEmpty(address) || address.Length < Constants.MinAddressLength)
        {
            return PostResultFactory.CreateFailure(ErrorFactory.ErrorCode.InvalidAddressLength);
        }

        try
        {
            var queryParameters = BuildQueryParameters(address).ToQueryParameters();
            var response = await _httpClient.GetAsync(queryParameters);

            return await HandleResponseAsync(response, address);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get post code for address: {address}", address);
            return PostResultFactory.CreateFailure(ex);
        }
    }
    
    private async Task<PostResult> HandleResponseAsync(HttpResponseMessage response, string address)
    {
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = DeserializeResponse(content);

            if (result != null)
            {
                return ProcessResult(result);
            }
        }
        
        return PostResultFactory.CreateFailure(ErrorFactory.ErrorCode.UnknownError);
    }
    
    private Response DeserializeResponse(string content)
    {
        try
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<Response>(content, jsonOptions);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed deserialize content");
            return null;
        }
    }
    
    private static PostResult ProcessResult(Response result)
    {
        if (IsSuccessfulResult(result))
        {
            return PostResultFactory.CreateSuccess(result.Data.FirstOrDefault()?.PostCode);
        }

        var errorCode = MapErrorCode(result.MessageCode);
    
        return PostResultFactory.CreateFailure(errorCode);
    }

    private static bool IsSuccessfulResult(Response result)
    {
        return result.Success &&
               result.Total >= 1;
    }
    
    private static ErrorFactory.ErrorCode MapErrorCode(int messageCode)
    {
        return messageCode switch
        {
            MessageCodes.InvalidUrl => ErrorFactory.ErrorCode.InvalidUrl,
            MessageCodes.ServiceUnavailable => ErrorFactory.ErrorCode.ServiceUnavailable,
            MessageCodes.ApiKeyInvalid => ErrorFactory.ErrorCode.ApiKeyInvalid,
            MessageCodes.ReachDailyRequestLimit => ErrorFactory.ErrorCode.ReachDailyRequestLimit,
            MessageCodes.ApiKeyNotSet => ErrorFactory.ErrorCode.ApiKeyNotSet,
            MessageCodes.TermParameterNotValid => ErrorFactory.ErrorCode.TermParameterNotValid,
            _ => ErrorFactory.ErrorCode.UnknownError
        };
    }

    private List<KeyValuePair<string, string>> BuildQueryParameters(string address)
    {
        return new List<KeyValuePair<string, string>>
        {
            new(QueryConstants.Term, address),
            new(QueryConstants.Key, _postSettings.ApiKey)
        };
    }
}