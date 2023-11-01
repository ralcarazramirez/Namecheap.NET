using System.ComponentModel.DataAnnotations;
using System.Net;

namespace NameCheap;

public record NamecheapOptions
{
    private const string SandboxUrl = "https://api.sandbox.namecheap.com/xml.response";
    private const string ProductionUrl = "https://api.namecheap.com/xml.response";
    private Uri? _apiUrl;
    [Required]
    public string? ApiUser { get; set; }
    [Required]
    public string? ApiKey { get; set; }
    
    [Required]
    public string? UserName { get; set; }
    
    public bool IsSandBox { get; set; }
    
    public Uri Url => IsSandBox ? new Uri(ProductionUrl) : new Uri(SandboxUrl);
    
    public Uri ApiUrl
    {
        get => _apiUrl ??= IsSandBox ? new Uri(SandboxUrl) : new Uri(ProductionUrl);
        set => _apiUrl = value;
    }
    
    [Required]
    public required IPAddress ClientIp { get; set; }
}