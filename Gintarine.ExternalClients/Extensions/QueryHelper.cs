using Microsoft.AspNetCore.Http.Extensions;

namespace Gintarine.ExternalClients.Extensions;

public static class QueryHelper
{
    public static string ToQueryParameters(this List<KeyValuePair<string, string>> parameters)
    {
        QueryBuilder queryBuilder = new();
        if (parameters == default)
        {
            return queryBuilder.ToQueryString().Value;
        }

        foreach (var parameter in parameters)
        {
            queryBuilder.Add(parameter.Key, parameter.Value);
        }
        
        return queryBuilder.ToQueryString().Value;
    }
}