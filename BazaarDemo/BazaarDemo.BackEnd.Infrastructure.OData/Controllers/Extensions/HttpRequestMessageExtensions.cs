using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;


public static class HttpRequestMessageExtensions
{
    public static Uri TrimQueryStringsAndRebuildUri(this HttpRequestMessage httpRequestMessage, params string[] queryStringName)
    {
        if (!queryStringName.Any()) { throw new ArgumentNullException("queryStringName"); }

        var queryStrings = "?" + string.Join("&", httpRequestMessage.GetQueryNameValuePairs().Where(x => !queryStringName.Contains(x.Key)).Select(x => string.Concat(x.Key, "=", x.Value)));

        return new Uri(httpRequestMessage.RequestUri.GetLeftPart(UriPartial.Path) + queryStrings);
    }
}

