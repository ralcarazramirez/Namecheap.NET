namespace NameCheapTests;

[TestClass]
public abstract class TestBase
{
    protected static readonly Lazy<string> _apiUser;
    protected static readonly Lazy<string> _apiKey;
    protected static readonly Lazy<string> _clientIp;

    protected static IDomainsApi _api = null!;

    // Domain used (and re-used for testing) - changing this value could have adverse effects to the test suite
    protected const string _domainName = "aeb80572-9b17-4ac9-8c24-048d2991119b.com"; // eaba62ff-e035-417a-8760-bd2d33972a25.com";
    protected const string TestUserFirstName = "TestFirstName";
    protected const string TestUserLastName = "TestLastName";
    protected static readonly Lazy<(string SecondLevelDomain, string TopLevelDomain)> DomainParts = new Lazy<(string SecondLevelDomain, string TopLevelDomain)>(() =>
    {
        var parts = _domainName.Split('.');
        return (parts[0], parts[1]);
    });

    // whether domain exists - null
    private static bool? _domainExists = null;

    static TestBase()
    {
        var config = new Lazy<IConfiguration>(() => LoadSettings());
        _apiUser = new Lazy<string>(() => config.Value.GetSection("apiUser").Value);
        _apiKey = new Lazy<string>(() => "");
        _clientIp = new Lazy<string>(() => config.Value.GetSection("clientIp").Value);
    }

    [AssemblyInitialize]
    public static async Task BeforeAllTestsInTheAssembly(TestContext context)
    {
        // this is where all the very expensive code goes.
        await EnsureTestDomain();
    }

    protected static async Task EnsureTestDomain()
    {
        if (_domainExists.HasValue && _domainExists == true)
        {
            return;
        }

        if (!_domainExists.HasValue)
        {
            try
            {
                var info = await _api.GetInfo(_domainName);
                _domainExists = true;
            }
            catch (ApplicationException) // TODO: where (e.ErrorCode == 2030166)
            {
                // likely the domain doesn't exist
                _domainExists = false;
            }
        }

        if (_domainExists == false)
        {
            _ = CreateTestDomain(); // might throw
        }
    }

    private static async Task<DomainCreateResult> CreateTestDomain()
    {
        // TODO: use different names for each type of contact to distinguish between them in testing
        var contact = new ContactInformation()
        {
            Address1 = "1 never never land",
            City = "New York",
            Country = "US",
            EmailAddress = "noreply@example.com",
            FirstName = TestUserFirstName,
            LastName = TestUserLastName,
            Phone = "+011.5555555555",
            PostalCode = "l5Z5Z5",
            StateProvince = "California"
        };

        var domain = await _api.Create(new DomainCreateRequest()
        {
            DomainName = _domainName,
            Admin = contact, 
            AuxBilling = contact,
            Registrant = contact,
            Tech = contact,
            Years = 1
        });

        return domain;
    }

    private static IConfiguration LoadSettings()
    {
        // home path is:
        // -Windows: c:\user\<user>\Documents 
        // - Unix: $HOME aka ~
        //         - /home/<user> on Linux
        //         - /Users/<user> on macOS
        var homePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        IConfiguration appSettings = new ConfigurationBuilder()
            .SetBasePath(homePath)
            .AddJsonFile("namecheapdotnet-settings.json", optional: true)
            .AddEnvironmentVariables("NAMECHEAPDOTNET_")
            .Build();

        return appSettings;
    }
}