
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace NameCheap;

public class NamecheapClient
{
    private readonly XNamespace _ns = XNamespace.Get("http://api.namecheap.com/xml.response");
    private readonly NamecheapOptions _options;
    private readonly HttpClient _client;
    private List<KeyValuePair<string, string?>> _parameters = new();

    internal NamecheapClient(IOptionsSnapshot<NamecheapOptions> optionsSnapshot, HttpClient client)
    {
        _options = optionsSnapshot.Value ?? throw new ArgumentNullException(nameof(optionsSnapshot));
        _client = client;
    }

    internal NamecheapClient AddParameter(string key, string? value)
    {
        _parameters.Add(new KeyValuePair<string, string?>(key, value));
        return this;
    }

    internal async Task<XDocument> ExecuteAsync(string command, CancellationToken token = default)
    {
        var url = new StringBuilder();
        url.Append(_options.IsSandBox ? "https://api.sandbox.namecheap.com/xml.response?" : "https://api.namecheap.com/xml.response?");
        url.Append("Command=").Append(command)
            .Append("&ApiUser=").Append(_options.ApiUser)
            .Append("&UserName=").Append(_options.UserName)
            .Append("&ApiKey=").Append(_options.ApiKey)
            .Append("&ClientIp=").Append(_options.ClientIp);

        foreach (var param in _parameters)
            url.Append('&').Append(param.Key).Append('=').Append(param.Value);

        var responseBody = await _client.GetStreamAsync(url.ToString(), token);
        var doc = await XDocument.LoadAsync(responseBody, LoadOptions.None, token);

        if (doc.Root!.Attribute("Status")!.Value.Equals("ERROR", StringComparison.OrdinalIgnoreCase))
            throw new ApplicationException(string.Join(",", doc.Root.Element(_ns + "Errors")!.Elements(_ns + "Error").Select(o => o.Value).ToArray()));
        else
            return doc;
    }
}